
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
        public long Role { get;set; }
        public int Salary { get; set; }

    }

    public class DocsService
    {
        ElectroDocumentContext context;
        IDistributedCache distributedCache;

        public DocsService(ElectroDocumentContext context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }

        public IEnumerable<Doc> GetDocs()
        {
            return context.Docs.AsEnumerable();
        }

        public IEnumerable<Doc> GetDocsByUserId(long id)
        {
            context.Employees.Load();
            return context.Docs.Where(doc => doc.EmployeeId == id);
        }

        public Doc? GetDocById(long id)
        {
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
            return context.Docs.Find(id);
        }

        public Doc? GetLastEmployeeContract(long UserId)
        {
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

        public void CreateDocument(long empId,ADocumentData rawdata)
        {
            if(rawdata is EmployeeContactData contactData)
            {
                Doc EmployeeContract = new Doc();
                EmployeeContract.Number = contactData.Number;
                EmployeeContract.Date = contactData.Date;
                EmployeeContract.Desc = GetRoleTitleById(contactData.Role);
                EmployeeContract.Sum = contactData.Salary;
                EmployeeContract.EmployeeId = empId;
                EmployeeContract.DocType = (int)DocumentTypes.EmploymentContract;
                EmployeeContract.Title = "Трудовой договор.";
                context.Docs.Add(EmployeeContract);
                context.SaveChanges();
            }
        }

        public async Task<Document> GenerateDocument(long id)
        {
            context.Docs.Load();
            context.Employees.Load();
            context.Roles.Load();
            context.Individuals.Load();
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
                        Replace(doc, @"ИМЯРОЛИ", rawDocument.Desc);
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
                        Replace(doc, @"СТАРАЯРОЛЬ", rawDocument.Desc);
                        Replace(doc, @"ПРИЧИНА", rawDocument.Reason);
                        Replace(doc, @"НОВАЯРОЛЬ", rawDocument.DescSecond);
                        DateOnly date = employeeContract.Date;

                        Replace(doc, @"ДТ", date.Day.ToString());
                        Replace(doc, @"МТ", date.Month.ToString());
                        Replace(doc, @"ГТ", date.Year.ToString());
                        Replace(doc, @"нТруд", employeeContract.Number.ToString());
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
                        Replace(doc, @"Причина", rawDocument.Reason);
                        Replace(doc, @"ДоксОснование", rawDocument.Desc);
                    }
                    break;
                case DocumentTypes.Weekend:
                    {
                        Doc employeeContract = GetLastEmployeeContract(rawDocument.EmployeeId.Value);
                        DateOnly date = employeeContract.Date;
                        DateTime startDay = rawDocument.Date.ToDateTime(new TimeOnly(0,0,0,0,0));
                        DateTime endDay = rawDocument.DateSecond.Value.ToDateTime(new TimeOnly(0, 0, 0, 0, 0));
                        int count = (int)(endDay - startDay).TotalDays; 

                        doc.LoadFromFile("Weekend.docx");
                        Replace(doc, @"нДок", rawDocument.Number.ToString());//3
                        Replace(doc, @"ДАТАС", DateTime.Now.ToString("dd.MM.yyyy"));//3
                        Replace(doc, @"ТБН", rawDocument.EmployeeId.ToString());//3
                        Replace(doc, @"ИМЯП", fullname);//3
                        Replace(doc, @"РОЛЬ", rawDocument.Employee.Role.Title);//3

                        if (rawDocument.Reason is null)
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

                            Replace(doc, @"ПРИЧИНА", rawDocument.Reason); //?3
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
                        doc.LoadFromFile("OD-SR-051.docx");
                        Replace(doc, @"ДОКН", rawDocument.Number.ToString());//3
                        Replace(doc, @"ДАТАС", DateTime.Now.ToString("dd.MM.yyyy"));//3
                        Replace(doc, @"РОЛЬНОВАЯ", rawDocument.Desc);//3
                        Replace(doc, @"ОКЛАД", rawDocument.Sum.ToString());//3
                        Replace(doc, @"ДАТАП", rawDocument.Date.ToString("dd.MM.yyyy"));//3
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
                        Replace(doc, @"РОЛЬ", rawDocument.DescSecond);//3
                        Replace(doc, @"ОПИСАНИЕ", rawDocument.Desc);//3
                        Replace(doc, @"СУММАПРОП", Utils.ConvertNumberToWords(sum));//3
                        Replace(doc, @"СУММАЦИФР", sum.ToString());//3
                        Replace(doc, @"ПРИЧИНА", rawDocument.Reason);//3
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
