namespace Lab4.DTOs
{
    public class BookInputDto 
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public int FreeCopies { get; set; }
        public bool IsDigitized { get; set; }
    }
}
