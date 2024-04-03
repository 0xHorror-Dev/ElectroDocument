namespace ElectroDocument.Models
{
    public class TabelMark
    {
        public int Id { get; set; }
        public string Mark{ get; set; }

    }

    public class TabelEmployee
    {
		public string JobTitle { get; set; }
		public string FullName { get; set; }
        public List<TabelMark> tabelMarks { get; set; } 

	}


	public class TabelModel
    {
        public DateTime DT { get; set; }

        public DateTime Date { get { return DT.Date;  } }

        public List<TabelEmployee>? tabel;

    }
}
