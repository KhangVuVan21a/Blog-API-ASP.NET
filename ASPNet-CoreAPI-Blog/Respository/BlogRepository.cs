using ASPNet_CoreAPI_Blog.Models;
using ASPNet_CoreAPI_Blog.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ASPNet_CoreAPI_Blog.Respository
{
    public class BlogRepository : IBlogRepository,IDisposable
    {
        private readonly BlogContext _context;
        private static readonly List<string> LISTPOS = new List<string>(new[] { "Viet Nam", "Chau A", "Chau Au", "Chau My" });
        public BlogRepository(BlogContext context)
        {
            _context = context;
        }
        //CreateBlog called Blogcontroller PostPosition() / CreateBlog(blogDto)
        //return blogcreated
        public object CreateBlog(BlogDTO blogDTO)
        {
            List<int> listposition = blogDTO.listposition.Split(',').Select(Int32.Parse).ToList();
            Blog blog = new Blog()
            {
                Id = 0,
                title = blogDTO.title,
                description = blogDTO.description,
                detail = blogDTO.detail,
                thumb = blogDTO.thumb,
                datePublic = blogDTO.datePublic,
                status = blogDTO.status,
                CategoryId = blogDTO.CategoryId,
                category = _context.Categories.Find(blogDTO.CategoryId),
                Positions = null,
            };
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            foreach (var id in listposition)
            {
                if (id < LISTPOS.Count)
                {
                    Position position = new Position()
                    {
                        Id = 0,
                        Name = LISTPOS[id - 1].ToString(),
                        BlogId = blog.Id
                    };
                    _context.Positions.Add(position);
                }
            }
            _context.SaveChanges();
            return blog;
        }


        //Delete blog called Blogcontroller DeleteBlog(id)
        //return blog
        public object DeleteBlogById(int id)
        {
            Blog blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return null;
            }
            _context.Positions.RemoveRange(_context.Positions.Where(x => x.BlogId == id).ToList());
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return blog;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        //Edit blog called Blogcontroller PutBlog(id,blogDTO)
        //return blog
        public object EditBlogById(int id, BlogDTO blogDTO)
        {
            List<int> listposition = blogDTO.listposition.Split(',').Select(Int32.Parse).ToList();
            if (id != blogDTO.Id)
            {
                return null;
            }
            Blog blog = new Blog()
            {
                Id = blogDTO.Id,
                title = blogDTO.title,
                description = blogDTO.description,
                detail = blogDTO.detail,
                thumb = blogDTO.thumb,
                datePublic = blogDTO.datePublic,
                status = blogDTO.status,
                CategoryId = blogDTO.CategoryId,
                category = _context.Categories.Find(blogDTO.CategoryId),
                Positions = null,
            };
            _context.Entry(blog).State = EntityState.Modified;
            _context.Positions.RemoveRange(_context.Positions.Where(x => x.BlogId == id).ToList());
            foreach (var item in listposition)
            {
                Position position = new Position()
                {
                    Id = 0,
                    Name = LISTPOS[item - 1].ToString(),
                    BlogId = blog.Id
                };
                _context.Positions.Add(position);
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Blogs.Find(id)==null)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return blog;

        }
        //Get all blog called Blogcontroller GetBlog()
        //return list blog
        public IEnumerable<object> GetAllBlogs()
        {
            if (_context.Blogs == null)
            {
                return null;
            }
            return _context.Blogs.Include(x=>x.category).Include(x=>x.Positions).ToList();
        }
        //Get blog by id called Blogcontroller GetBlog(id)
        //return blog
        public object GetBlogById(int id)
        {
            if (_context.Blogs == null)
            {
                return null;
            }
            return _context.Blogs.Where(x => x.Id == id).Include(x => x.category).Include(x => x.Positions).First();
        }
        //save changes in database
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
