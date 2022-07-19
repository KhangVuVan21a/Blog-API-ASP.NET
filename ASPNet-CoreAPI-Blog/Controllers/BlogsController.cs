using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNet_CoreAPI_Blog.Models;
using ASPNet_CoreAPI_Blog.Models.DTO;
using ASPNet_CoreAPI_Blog.Respository;

namespace ASPNet_CoreAPI_Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogContext _context;
        private BlogRepository _repository;
        private IBlogRepository blogRepository;

        public BlogsController(BlogContext context)
        {
            _repository = new BlogRepository(context);
            blogRepository = new BlogRepository(context);
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
            IEnumerable<Blog> blogs = (IEnumerable<Blog>)_repository.GetAllBlogs();
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

            Blog blog = (Blog)_repository.GetBlogById(id);
/*            Blog blog1 = (Blog)blogRepository.DeleteBlogById(id);*/
            
            /*var test = new
            { 
                id = blog.Id,
                title = blog.title,
                detail = blog.detail,
                pos = blog.Positions,
                check = "asdasd"
            };*/
            if(blog == null)
            {
                return NotFound();
            }
            return Accepted(blog);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, BlogDTO blogDTO)
        {
            if (id != blogDTO.Id)
            {
                return BadRequest();
            }
            Blog blog = (Blog)_repository.EditBlogById(id, blogDTO);
            if(blog ==null)
            {
                return NotFound();
            }
            return Accepted("Edited!");
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(BlogDTO blogDTO)
        {
          if (_context.Blogs == null)
          {
              return Problem("Entity set 'BlogContext.Blogs'  is null.");
          }
            Blog blog = (Blog)_repository.CreateBlog(blogDTO);

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
            Blog blogdeleted =(Blog)_repository.DeleteBlogById(id);
            if (blogdeleted == null)
            {
                return NotFound();
            }
            return Accepted("deleted!");

        }

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
