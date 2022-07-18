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
        //Create blog 
        //BlogDTO + string position   (ex : blogDTO{...} + "1,2,3"
        //return Blog
        public object CreateBlog(BlogDTO blogDTO, string listpos)
        {
            List<int> list = listpos.Split(',').Select(Int32.Parse).ToList();
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
            foreach (var id in list)
            {
                Position position = new Position()
                {
                    Id = 0,
                    Name = LISTPOS[id - 1].ToString(),
                    BlogId = blog.Id
                };
                _context.Positions.Add(position);
            }
            _context.SaveChanges();
            return blog;
        }
        //Delete blog by id 
        // return blog deleted 
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
        //Edit blog by id 
        //params id +BlogDTO + String listpos 
        //return Blog
        public object EditBlogById(int id, BlogDTO blogDTO,string listpos)
        {
            List<int> list = listpos.Split(',').Select(Int32.Parse).ToList();
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
            foreach (var item in list)
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
        //Getallblog
        //return list<Object>
        public IEnumerable<object> GetAllBlogs()
        {
            if (_context.Blogs == null)
            {
                return null;
            }
            return _context.Blogs.Include(x=>x.category).Include(x=>x.Positions).ToList();
        }
        //get blog by id 
        // params: id
        // return fisrt blog equal by id 
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
