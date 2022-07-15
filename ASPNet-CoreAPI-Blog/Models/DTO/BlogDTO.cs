namespace ASPNet_CoreAPI_Blog.Models.DTO
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string detail { get; set; }
        public string? thumb { get; set; }
        public DateTime datePublic { get; set; }
        public bool status { get; set; }
        public int CategoryId { get; set; }
    }
}
