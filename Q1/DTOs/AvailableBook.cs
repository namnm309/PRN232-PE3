namespace Q1.DTOs
{
    public class AvailableBook
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PublicationYear {  get; set; }
        public int AvailableCopines {  get; set; }
    }
}
