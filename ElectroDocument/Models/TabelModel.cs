namespace ElectroDocument.Models
{
    public class TabelModel
    {
        public DateTime DT { get; set; }

        public DateTime Date { get { return DT.Date;  } }
    }
}
