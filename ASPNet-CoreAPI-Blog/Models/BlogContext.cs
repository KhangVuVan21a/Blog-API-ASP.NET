using Microsoft.EntityFrameworkCore;

namespace ASPNet_CoreAPI_Blog.Models
{
    public class BlogContext:DbContext
    {
            public BlogContext(DbContextOptions<BlogContext> options)
                : base(options)
            {
            }

            public DbSet<Blog> Blogs { get; set; } = null!;
            public DbSet<Category> Categories { get; set; } = null!;
            public DbSet<Position> Positions { get; set; } = null!;
    }
}
