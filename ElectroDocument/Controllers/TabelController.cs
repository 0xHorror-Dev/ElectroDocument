using Microsoft.AspNetCore.Mvc;
using ElectroDocument.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectroDocument.Controllers
{
    public class TabelController : Controller
    {
        public IActionResult Index()
        {
            DateTime dt = new DateTime(2024, 02, 02, 0, 0, 0, 0, 0);

			var model = new TabelModel { DT = dt };
            model.tabel = new List<TabelEmployee>();
            TabelEmployee employee = new TabelEmployee();
            employee.tabelMarks = new List<TabelMark>();
            employee.JobTitle = "Сваршик";
            employee.FullName = "Иван Иванов Иванович";

			int maxDays = DateTime.DaysInMonth(dt.Year, dt.Month);
            for (int i = 1; i < maxDays + 1; i++)
            {
                if(i % 2 == 0)
                {
				    employee.tabelMarks.Add(new TabelMark { Id = i, Mark= "Н" });
                }
                else
                {
				    employee.tabelMarks.Add(new TabelMark { Id = i, Mark= " " });
                }
            }

            model.tabel.Add(employee);

			return View(model);
        }
    }
}
