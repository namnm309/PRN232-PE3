namespace Q1.DTOs
{
    public class BookPaginationResponse
    {
        //Tuân theo format reponse cảu đề
        public int TotalBooks { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<AvailableBook> Data { get; set; } = [];
    }
}
