
using ElectroDocument.Controllers.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection.Metadata;

using Spire.Doc;
using System.Text.RegularExpressions;
using Spire.Doc.Layout;
using Document = Spire.Doc.Document;
using Microsoft.AspNetCore.Mvc;
using ElectroDocument.Models;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.InkML;
using System.Security.Policy;
using OpenXmlPowerTools;
using System.Collections;


namespace ElectroDocument.Controllers.Services
{
    enum DocumentTypes : int
    {
        EmploymentContract = 0,
        Moved = 1,
        Dismissed = 2,
        Weekend = 3,
        AddRole = 4,
        Encouragement = 5
    }

    public abstract class ADocumentData
    {
        public string Title { get; set; }
        public long Number { get; set; }
        public DateOnly Date { get; set; }
    }

    public class EmployeeContactData : ADocumentData 
    {
        public string Role { get;set; }
        public int Salary { get; set; }

    }

    public class RoleMoveData : ADocumentData
    {
        public string OldRole { get; set; }
        public string NewRole { get; set; }
        public string Reason{ get; set; }
        public long Salary { get; set; }


    }

    public class DismissedData : ADocumentData
    {
        public string Reason { get; set; }
        public string Desc { get; set; }
    }



    public class WeekendData : ADocumentData
    {
        public DateOnly End { get; set; }
        public string? Reason {  get; set; }
    }

    public class RoleCreationData : ADocumentData
    {
        public string NewRole { get; set; }
        public int Salary{ get; set; }
        public long Resposible { get; set; }
    }

    public class EncourageData : ADocumentData
    {
        public string Role { get; set; }
        public string Desc{ get; set; }
        public string Reason{ get; set; }
        public int Salary { get; set; }
    }


    public class DocsService
    {
        ElectroDocumentDevContext context;
        IDistributedCache distributedCache;

        public DocsService(ElectroDocumentDevContext context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }

        public IEnumerable<Doc> GetDocs()
        {
            context.DocDetails.Load();
            context.Docs.Load();
            return context.Docs.AsEnumerable();
        }

        public IEnumerable<Doc> GetDocsByUserId(long id)
        {
            context.DocDetails.Load();
            context.Employees.Load();
            return context.Docs.Where(doc => doc.EmployeeId == id);
        }

        private bool FullDocsStatement(long id, Doc doc)
        {
            if (doc.EmployeeId == id) return true;
            foreach (DocumentVersion version in doc.DocumentVersionDocs)
            {
                if (version.EditorId == id) return true;
            }

            return false;
        }


        public IEnumerable<Doc> GetFullDocsByUserId(long id)
        {
            context.DocDetails.Load();
            context.Employees.Load();
            context.DocumentVersions.Load();
            List<Doc> docs = new List<Doc>(context.Docs.Where(doc => doc.EmployeeId == id));
            IEnumerable<DocumentVersion> versions = context.DocumentVersions.Where(ver=>ver.EditorId == id);
            foreach(DocumentVersion ver in versions)
            {
                if(ver.NewDoc is not null)
                    docs.Add(ver.NewDoc);
            }

            for (int i = 0; i < docs.Count; i++)
            {
                Doc newDoc = GetLastDocVersionById(docs[i].Id);
                docs[i] = newDoc;
            }

            List<long> ids = new List<long>();

            docs.RemoveAll(doc =>
            {
                if (!ids.Contains(doc.Id))
                {
                    ids.Add(doc.Id);
                    return false;
                }
                else
                {
                    return true;
                }
            });

            return docs;
        }

        public void UpdateRole(long userId, long roleId) 
        {
            context.DocDetails.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Employees.Find(userId).RoleId = roleId;
            context.SaveChanges();
        }

        public Doc? GetDocById(long id)
        {
            context.DocDetails.Load();
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
            return context.Docs.Find(id);
        }

        public DocumentVersion? GetDocumentVersion(long id)
        {
            context.DocDetails.Load();
            context.DocumentVersions.Load();
            List<DocumentVersion> docsVersions = new List<DocumentVersion>(context.DocumentVersions);
            foreach(DocumentVersion version  in docsVersions)
            {
                if (version.NewDocId != null && version.NewDocId == id) return version;
                else if (version.DocIdSrc == id) return version;
            }
            return null;
        }

        public Doc? GetLastDocVersionById(long id)
        {
            context.DocDetails.Load();
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
            Doc doc = context.Docs.Find(id);
            DocumentVersion docVer = GetDocumentVersion(id);
            if(docVer.DocIdSrc == id)
            {
                List<DocumentVersion> docsVersions = new List<DocumentVersion>(doc.DocumentVersionDocs);
                docsVersions.AddRange(doc.DocumentVersionNewDocs);
                docsVersions.AddRange(doc.DocumentVersionDocIdSrcNavigations);
                IEnumerable<DocumentVersion> docs = docsVersions.OrderBy(ver => ver.Id);

                return docs.Last().NewDoc is not null ? docs.Last().NewDoc : docs.Last().Doc;
            }

            return GetLastDocVersionById(docVer.DocIdSrc);
        }

        public Doc? GetFirstDocVersionById(long id)
        {
            context.DocDetails.Load();
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
            DocumentVersion docVer = GetDocumentVersion(id);
            if (docVer.DocIdSrc == id)
            {
                return docVer.DocIdSrcNavigation;
            }

            return GetFirstDocVersionById(docVer.DocIdSrc);
        }

        public Doc? GetLastEmployeeContract(long UserId)
        {
            context.DocDetails.Load();
            context.Employees.Load();
            context.Docs.Load();
            context.Individuals.Load();

            return context.Docs.Where(d => d.EmployeeId == UserId).Where(d => d.DocType == (int)DocumentTypes.EmploymentContract).OrderBy(d => d.Date).Last();

        }
        public string GetRoleTitleById(long id)
        {
            context.Roles.Load();
            return context.Roles.Find(id).Title;
        }

        public Doc FillStruct(long empId, ADocumentData rawdata)
        {
            if (rawdata is EmployeeContactData contactData)
            {
                Doc EmployeeContract = new Doc();
                EmployeeContract.Number = contactData.Number;
                EmployeeContract.Date = contactData.Date;
                EmployeeContract.DocDetails = new DocDetail { Desc = contactData.Role };
                EmployeeContract.Sum = contactData.Salary;
                EmployeeContract.EmployeeId = empId;
                EmployeeContract.DocType = (int)DocumentTypes.EmploymentContract;
                EmployeeContract.Title = "Трудовой договор.";

                return EmployeeContract;
            }
            else if (rawdata is RoleMoveData roleMoveData)
            {
                Doc moved = new Doc();
                moved.Date = roleMoveData.Date;
                moved.Number = roleMoveData.Number;
                moved.EmployeeId = empId;
                moved.DocDetails = new DocDetail { Desc = roleMoveData.OldRole, DescSecond = roleMoveData.NewRole, Reason = roleMoveData.Reason };
                moved.Sum = (int)roleMoveData.Salary;
                moved.DocType = (int)DocumentTypes.Moved;
                moved.Title = "Перевод сотрудника.";

                return moved;
            }
            else if (rawdata is DismissedData dismissedData)
            {
                Doc dis = new Doc();
                dis.Date = dismissedData.Date;
                dis.Number = dismissedData.Number;
                dis.EmployeeId = empId;
                dis.DocDetails = new DocDetail { Desc = dismissedData.Desc, Reason = dismissedData.Reason };

                dis.DocType = (int)DocumentTypes.Dismissed;
                dis.Title = "Расторжение трудового договора.";

                return dis;

            }
            else if (rawdata is WeekendData weekend)
            {
                Doc weekendDoc = new Doc();
                weekendDoc.Date = weekend.Date;
                weekendDoc.Number = weekend.Number;
                weekendDoc.EmployeeId = empId;
                weekendDoc.DocDetails = new DocDetail { Reason = weekend.Reason };
                weekendDoc.Date = weekend.Date;
                weekendDoc.DateSecond = weekend.End;

                {
                    int currentYear = DateTime.Now.Year;

                    Doc employeeContract = GetLastEmployeeContract(empId);
                    int diff = (currentYear - 1) - employeeContract.Date.Year;

                    weekendDoc.DateThird = employeeContract.Date.AddYears(diff);
                }


                weekendDoc.DocType = (int)DocumentTypes.Weekend;
                weekendDoc.Title = "Предоставление отпуска.";

                return weekendDoc;
            }
            else if (rawdata is EncourageData encourageData)
            {
                Doc encourage = new Doc();
                encourage.Number = encourageData.Number;
                encourage.EmployeeId = empId;
                encourage.DocDetails = new DocDetail { Reason = encourageData.Reason, Desc = encourageData.Desc, DescSecond = encourageData.Role };
                encourage.Date = encourageData.Date;
                encourage.Sum = encourageData.Salary;

                encourage.DocType = (int)DocumentTypes.Encouragement;
                encourage.Title = "Поощерение.";

                return encourage;
            }
            else if (rawdata is RoleCreationData roleDate)
            {
                Doc role = new Doc();
                role.Date = roleDate.Date;
                role.Number = roleDate.Number;
                role.EmployeeId = empId;
                role.Sum = roleDate.Salary;
                role.DocDetails = new DocDetail { Desc = roleDate.NewRole };

                role.DocType = (int)DocumentTypes.AddRole;
                role.Title = "О внесение изменений в штатное расписание.";

                role.Responsible = roleDate.Resposible;

                return role;
            }

            return null;
        }

        public bool isFirstDoc(long docId)
        {
            var seq = context.DocumentVersions.Where(v => v.NewDocId == docId);
            int c = seq.Count();
            return c < 1;
        }

        public bool isFirstNewDocVersion(long docId)
        {
            var seq = context.DocumentVersions.Where(v => v.DocId == docId);
            int c = seq.Count();
            return c < 1;
        }


        public void EditDocument(long empId, long editorId, long docId, ADocumentData rawData)
        {
            context.Docs.Load();
            context.DocDetails.Load();
            Doc srcDoc = GetFirstDocVersionById(docId);
            
            Doc doc = FillStruct(empId, rawData);
            context.Docs.Add(doc);
            context.SaveChanges();

            context.DocumentVersions.Load();
            long newDocId = doc.Id;

            DocumentVersion version = new DocumentVersion();
            //context.Docs.Find(docId);

            version.NewDocId = newDocId;
            version.DocIdSrc = srcDoc.Id;
            version.Date = DateTime.Now;
            version.DocId = docId;
            version.EditorId = editorId;


            context.DocumentVersions.Add(version);
            context.SaveChanges();
        }

        
        public void CreateDocument(long empId,ADocumentData rawdata)
        {
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
            context.DocDetails.Load();
            context.DocumentVersions.Load();

            long docId = 0;

            if (rawdata is EmployeeContactData contactData)
            {
                Doc EmployeeContract = new Doc();
                EmployeeContract.Number = contactData.Number;
                EmployeeContract.Date = contactData.Date;
                EmployeeContract.DocDetails = new DocDetail { Desc = contactData.Role };
                EmployeeContract.Sum = contactData.Salary;
                EmployeeContract.EmployeeId = empId;
                EmployeeContract.DocType = (int)DocumentTypes.EmploymentContract;
                EmployeeContract.Title = "Трудовой договор.";
                context.Docs.Add(EmployeeContract);
                context.Docs.Add(EmployeeContract);
                context.SaveChanges();
                docId = EmployeeContract.Id;
            }
            else if(rawdata is RoleMoveData roleMoveData)
            {
                Doc moved = new Doc();
                moved.Date = roleMoveData.Date;
                moved.Number = roleMoveData.Number;
                moved.EmployeeId = empId;
                moved.DocDetails = new DocDetail 
                { 
                    Desc = roleMoveData.OldRole, 
                    DescSecond = roleMoveData.NewRole, 
                    Reason = roleMoveData.Reason 
                };
                moved.Sum = (int)roleMoveData.Salary;
                moved.DocType = (int)DocumentTypes.Moved;
                moved.Title = "Перевод сотрудника.";

                context.Docs.Add(moved);
                context.SaveChanges();
                docId = moved.Id;
            }
            else if (rawdata is DismissedData dismissedData)
            {
                Doc dis = new Doc();
                dis.Date = dismissedData.Date;
                dis.Number = dismissedData.Number;
                dis.EmployeeId = empId;
                dis.DocDetails = new DocDetail { Desc = dismissedData.Desc, Reason = dismissedData.Reason };

                dis.DocType = (int)DocumentTypes.Dismissed;
                dis.Title = "Расторжение трудового договора.";

                context.Docs.Add(dis);
                context.SaveChanges();
                docId = dis.Id;
            }
            else if (rawdata is WeekendData weekend)
            {
                Doc weekendDoc = new Doc();
                weekendDoc.Date = weekend.Date;
                weekendDoc.Number = weekend.Number;
                weekendDoc.EmployeeId = empId;
                weekendDoc.DocDetails = new DocDetail { Reason = weekend.Reason };
                weekendDoc.Date = weekend.Date;
                weekendDoc.DateSecond = weekend.End;

                {
                    int currentYear = DateTime.Now.Year;
                
                    Doc employeeContract = GetLastEmployeeContract(empId);
                    int diff = (currentYear - 1) - employeeContract.Date.Year;

                    weekendDoc.DateThird = employeeContract.Date.AddYears(diff);
                }


                weekendDoc.DocType = (int)DocumentTypes.Weekend;
                weekendDoc.Title = "Предоставление отпуска.";

                context.Docs.Add(weekendDoc);
                context.SaveChanges();

                docId = weekendDoc.Id;
            }
            else if (rawdata is EncourageData encourageData)
            {
                Doc encourage = new Doc();
                encourage.Number = encourageData.Number;
                encourage.EmployeeId = empId;
                encourage.DocDetails = new DocDetail { Reason = encourageData.Reason };
                encourage.Date = encourageData.Date;
                encourage.DocDetails.DescSecond = encourageData.Role;
                encourage.DocDetails.Desc = encourageData.Desc;
                encourage.Sum = encourageData.Salary;

                encourage.DocType = (int)DocumentTypes.Encouragement;
                encourage.Title = "Поощерение.";

                context.Docs.Add(encourage);
                context.SaveChanges();

                docId = encourage.Id;
            }
            else if (rawdata is RoleCreationData roleDate)
            {
                Doc role = new Doc();
                role.Date = roleDate.Date;
                role.Number = roleDate.Number;
                role.EmployeeId = empId;
                role.Sum = roleDate.Salary;
                role.DocDetails = new DocDetail { Desc = roleDate.NewRole };

                role.DocType = (int)DocumentTypes.AddRole;
                role.Title = "О внесение изменений в штатное расписание.";

                role.Responsible = roleDate.Resposible;
                context.Docs.Add(role);
                context.SaveChanges();

                docId = role.Id;
            }

            DocumentVersion documentVersion = new DocumentVersion();
            documentVersion.DocId = docId;
            documentVersion.Date = DateTime.Now;
            documentVersion.EditorId = empId;
            documentVersion.DocIdSrc = docId;

            context.DocumentVersions.Add(documentVersion);

            context.SaveChanges();
        }

        public async Task<Document> GenerateDocument(long id)
        {
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
            context.DocDetails.Load();
            Doc rawDocument = await context.Docs.Where(d => d.Id == id).FirstAsync();
            DocumentTypes type = (DocumentTypes)rawDocument.DocType;
            Document doc = new Document();

            string fullname = $"{rawDocument.Employee.Individual.Name} {rawDocument.Employee.Individual.Surname} {rawDocument.Employee.Individual.Patronymic}";
            switch (type)
            {
                case DocumentTypes.EmploymentContract:
                    {

                        doc.LoadFromFile("TrudDocx.docx");
                        Replace(doc, @"нДГ", rawDocument.Number.ToString());
                        Replace(doc, @"ДТСЕЙЧАС", DateTime.Now.ToString("dd.MM.yyyy"));
                        Replace(doc, @"ИМЯПОЛНОЕ", fullname);
                        Replace(doc, @"ИМЯРОЛИ", rawDocument.DocDetails.Desc);
                        Replace(doc, @"ДАТАПЕРВЫЙД", rawDocument.Date.ToString("dd.MM.yyyy"));
                        Replace(doc, @"ДАТАПЕРВЫЙДK", rawDocument.Date.ToString("dd.MM.yyyy"));
                        Replace(doc, @"АДРЕСС", rawDocument.Employee.Individual.Address);
                        Replace(doc, @"ОКЛАД1", rawDocument.Sum.ToString());
                    }
                    break;
                case DocumentTypes.Moved:
                    {

                        Doc employeeContract = GetLastEmployeeContract(rawDocument.EmployeeId.Value);
                        
                        doc.LoadFromFile("Perevod.docx");
                        Replace(doc, @"ДАТАС", DateTime.Now.ToString("dd.MM.yyyy"));
                        Replace(doc, @"нДок", rawDocument.Number.ToString());
                        Replace(doc, @"ДАТАП", rawDocument.Date.ToString("dd.MM.yyyy"));
                        Replace(doc, @"нРаб", rawDocument.EmployeeId.Value.ToString());
                        Replace(doc, @"ИМЯП", fullname);
                        Replace(doc, @"СТАРАЯРОЛЬ", rawDocument.DocDetails.Desc);
                        Replace(doc, @"ПРИЧИНА", rawDocument.DocDetails.Reason);
                        Replace(doc, @"НОВАЯРОЛЬ", rawDocument.DocDetails.DescSecond);
                        DateOnly date = employeeContract.Date;

                        Replace(doc, @"ДТ", date.Day.ToString());
                        Replace(doc, @"МТ", date.Month.ToString());
                        Replace(doc, @"ГТ", date.Year.ToString());
                        Replace(doc, @"нТруд", employeeContract.Number.ToString());
                        Replace(doc, @"ОКЛАД1", rawDocument.Sum.ToString());
                    }
                    break;
                case DocumentTypes.Dismissed:
                    {

                        Doc employeeContract = GetLastEmployeeContract(rawDocument.EmployeeId.Value);
                        DateOnly date = employeeContract.Date;
                        DateOnly dismissDate = rawDocument.Date;

                        doc.LoadFromFile("DismissedEmployee.docx");
                        Replace(doc, @"нумерок", rawDocument.Number.ToString());
                        Replace(doc, @"блинбдата", DateTime.Now.ToString("dd.MM.yyyy"));
                        Replace(doc, @"день", date.Day.ToString());
                        Replace(doc, @"месяц", date.Month.ToString());
                        Replace(doc, @"год", date.Year.ToString());
                        Replace(doc, @"трудовой", employeeContract.Number.ToString());
                        Replace(doc, @"День", dismissDate.Day.ToString());
                        Replace(doc, @"Месяц", dismissDate.Month.ToString());
                        Replace(doc, @"Год", dismissDate.Year.ToString());
                        Replace(doc, @"ФИО", fullname);
                        Replace(doc, @"табель", rawDocument.EmployeeId.Value.ToString());
                        Replace(doc, @"РОЛЬ", rawDocument.Employee.Role.Title);
                        Replace(doc, @"Причина", rawDocument.DocDetails.Reason);
                        Replace(doc, @"ДоксОснование", rawDocument.DocDetails.Desc);
                    }
                    break;
                case DocumentTypes.Weekend:
                    {
                        Doc employeeContract = GetLastEmployeeContract(rawDocument.EmployeeId.Value);
                        DateOnly date = employeeContract.Date;
                        DateTime startDay = rawDocument.Date.ToDateTime(new TimeOnly(0,0,0,0,0));
                        DateTime endDay = rawDocument.DateSecond.Value.ToDateTime(new TimeOnly(0, 0, 0, 0, 0));
                        int count = (int)(endDay - startDay).TotalDays;

                        doc.LoadFromFile("Weekend1.docx");
                        Replace(doc, @"нДок", rawDocument.Number.ToString());//3
                        Replace(doc, @"ДАТАС", DateTime.Now.ToString("dd.MM.yyyy"));//3
                        Replace(doc, @"ТБН", rawDocument.EmployeeId.ToString());//3
                        Replace(doc, @"ИМЯП", fullname);//3
                        Replace(doc, @"РОЛЬ", rawDocument.Employee.Role.Title);//3

                        {
                            DateOnly startPeriod;
                            
                            {
                                int currentYear = DateTime.Now.Year;
                                int diff = currentYear == employeeContract.Date.Year ? 0 : (currentYear - 1) - employeeContract.Date.Year;
                                startPeriod = rawDocument.DateThird.GetValueOrDefault(employeeContract.Date.AddYears(diff));
                            }

                            Replace(doc, @"ДПД", startPeriod.Day.ToString());
                            Replace(doc, @"ДПМ", startPeriod.Month.ToString());
                            Replace(doc, @"ДПГ", startPeriod.Year.ToString());

                            DateOnly endPeriod = startPeriod.AddYears(1);
                            endPeriod = endPeriod.AddDays(-1);

                            Replace(doc, @"ДИПД", endPeriod.Day.ToString());
                            Replace(doc, @"ДИПМ", endPeriod.Month.ToString());
                            Replace(doc, @"ДИПД", endPeriod.Year.ToString());
                        }

                        if (rawDocument.DocDetails.Reason is null)
                        {
                            Replace(doc, @"АДН", count.ToString()); //n 

                            Replace(doc, @"ДА",  startDay.Day.ToString());//n
                            Replace(doc, @"МА",  startDay.Month.ToString());//n
                            Replace(doc, @"ГА",  startDay.Year.ToString());//n
                            Replace(doc, @"ДКА", endDay.Day.ToString());//n
                            Replace(doc, @"МКА", endDay.Month.ToString());//n
                            Replace(doc, @"ГКА", endDay.Year.ToString());//n

                            Replace(doc, @"ПРИЧИНА", ""); //?3
                            Replace(doc, @"БДН", ""); //
                            Replace(doc, @"ДБ", "");//
                            Replace(doc, @"МБ", "");//
                            Replace(doc, @"ГБ", "");//
                            Replace(doc, @"ДКБ", "");//
                            Replace(doc, @"МКБ", "");//
                            Replace(doc, @"ГКБ", "");//
                        }
                        else
                        {
                            Replace(doc, @"АДН", ""); //n 

                            Replace(doc, @"ДА", "");//n
                            Replace(doc, @"МА", "");//n
                            Replace(doc, @"ГА", "");//n
                            Replace(doc, @"ДКА", "");//n
                            Replace(doc, @"МКА", "");//n
                            Replace(doc, @"ГКА", "");//n

                            Replace(doc, @"ПРИЧИНА", rawDocument.DocDetails.Reason); //?3
                            Replace(doc, @"БДН", count.ToString()); //
                            Replace(doc, @"ДБ", startDay.Day.ToString());//
                            Replace(doc, @"МБ", startDay.Month.ToString());//
                            Replace(doc, @"ГБ", startDay.Year.ToString());//
                            Replace(doc, @"ДКБ", endDay.Day.ToString());//
                            Replace(doc, @"МКБ", endDay.Month.ToString());//
                            Replace(doc, @"ГКБ", endDay.Year.ToString());//
                        }
                    }
                    break;
                case DocumentTypes.AddRole:
                    {
                        string ResponsibleFullname = $"{rawDocument.ResponsibleNavigation.Individual.Name} {rawDocument.ResponsibleNavigation.Individual.Surname} {rawDocument.ResponsibleNavigation.Individual.Patronymic}";
                        //rawDocument.ResponsibleNavigation.Role.
                        doc.LoadFromFile("OD-SR-051-1.docx");
                        Replace(doc, @"ДОКН", rawDocument.Number.ToString());//3
                        Replace(doc, @"ДАТАС", DateTime.Now.ToString("dd.MM.yyyy"));//3
                        Replace(doc, @"РОЛЬНОВАЯ", rawDocument.DocDetails.Desc);//3
                        Replace(doc, @"ОКЛАД", rawDocument.Sum.ToString());//3
                        Replace(doc, @"ДАТАП", rawDocument.Date.ToString("dd.MM.yyyy"));//3
                        Replace(doc, @"ResponsibleFN", ResponsibleFullname);//3
                        Replace(doc, @"ResponsibleRole", rawDocument.ResponsibleNavigation.Role.Title);//3
                    }
                    break;
                case DocumentTypes.Encouragement:
                    {
                        doc.LoadFromFile("POOSHERENIE.doc");

                        int sum = rawDocument.Sum.GetValueOrDefault(0);

                        Replace(doc, @"ДОКН", rawDocument.Number.ToString());//3
                        Replace(doc, @"ДАТАС", DateTime.Now.ToString("dd.MM.yyyy"));//3
                        Replace(doc, @"ТАБЕЛЬН", rawDocument.EmployeeId.ToString());//3
                        Replace(doc, @"ФИО", fullname);//3
                        Replace(doc, @"РОЛЬ", rawDocument.DocDetails.DescSecond);//3
                        Replace(doc, @"ОПИСАНИЕ", rawDocument.DocDetails.Desc);//3
                        Replace(doc, @"СУММАПРОП", Utils.ConvertNumberToWords(sum));//3
                        Replace(doc, @"СУММАЦИФР", sum.ToString());//3
                        Replace(doc, @"ПРИЧИНА", rawDocument.DocDetails.Reason);//3
                    }
                    break;
            }

            return doc;
        }

        private void Replace(Document doc, string rpattern, string replaceString)
        {
            Regex regex = new Regex(rpattern);
            doc.Replace(regex, replaceString);
        }
    }
}
