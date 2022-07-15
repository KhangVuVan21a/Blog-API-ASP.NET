namespace ASPNet_CoreAPI_Blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
