using System.ComponentModel.DataAnnotations;

namespace ASPNet_CoreAPI_Blog.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string detail { get; set; }
        public string? thumb   { get; set; }
        public DateTime datePublic { get; set; }
        public bool status { get; set; }
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public virtual ICollection<Position> Positions { get; set; }


    }
}
