using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNet_CoreAPI_Blog.Models;
using ASPNet_CoreAPI_Blog.Models.DTO;

namespace ASPNet_CoreAPI_Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogContext _context;
        private List<string> listposition = new List<string>() { "Viet Nam", "Chau A", "Chau Au", "Chau My"};

        public BlogsController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetBlogs()
        {
          if (_context.Blogs == null)
          {
              return NotFound();
          }
            /*return await _context.Blogs.ToListAsync();*/
            var blogs =  from blog in _context.Blogs select new{
                blog.Id,
                blog.title,
                blog.description,
                blog.detail,
                blog.thumb,
                blog.datePublic,
                blog.status,
                blog.category,
                blog.Positions
            };
            return blogs.ToList();
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetBlog(int id)
        {
          if (_context.Blogs == null)
          {
              return NotFound();
          }
            /*var blog = await _context.Blogs.Where(x => x.Id == id)
                         .Include(x => x.category).Include(x=>x.Positions)
                         .FirstOrDefaultAsync();*/
            var test = from blog in _context.Blogs where blog.Id == id select new
            {
                blog.Id,
                blog.title,
                blog.description,
                blog.detail,
                blog.thumb,
                blog.datePublic,
                blog.status,
                blog.CategoryId,
                blog.category,
                blog.Positions
            } ;
            if (test == null)
            {
                return NotFound();
            }

            return Accepted(test.First());
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, BlogDTO blogDTO,string listpos)
        {
            List<int> list = listpos.Split(',').Select(Int32.Parse).ToList();
            if (id != blogDTO.Id)
            {
                return BadRequest();
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
                    Name = listposition[item - 1].ToString(),
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
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(BlogDTO blogDTO, string listpos)
        {
           List<int> list = listpos.Split(',').Select(Int32.Parse).ToList();
          if (_context.Blogs == null)
          {
              return Problem("Entity set 'BlogContext.Blogs'  is null.");
          }
            Blog blog = new Blog()
            {
                Id= 0,
                title=blogDTO.title,
                description=blogDTO.description,
                detail=blogDTO.detail,
                thumb=blogDTO.thumb,
                datePublic=blogDTO.datePublic,
                status=blogDTO.status,
                CategoryId=blogDTO.CategoryId,
                category=_context.Categories.Find(blogDTO.CategoryId),
                Positions=null,
            };
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            var idblog = blog.Id;
            foreach (var id in list)
            {
                Position position=new Position()
                {
                    Id = 0,
                    Name = listposition[id - 1].ToString(),
                    BlogId = blog.Id
                };
                _context.Positions.Add(position) ;
            }
            _context.SaveChanges();

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Positions.RemoveRange(_context.Positions.Where(x => x.BlogId == blog.Id).ToList());
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
