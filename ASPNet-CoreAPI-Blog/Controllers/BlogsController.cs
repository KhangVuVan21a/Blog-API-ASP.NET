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

        public BlogsController(BlogContext context)
        {
            _repository = new BlogRepository(context);
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
            if(blog == null)
            {
                return NotFound();
            }
            return Accepted(blog);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, BlogDTO blogDTO,string listpos)
        {
            if (id != blogDTO.Id)
            {
                return BadRequest();
            }
            Blog blog = (Blog)_repository.EditBlogById(id, blogDTO, listpos);
            if(blog ==null)
            {
                return NotFound();
            }
            return Accepted("Edited!");
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
            Blog blog = (Blog)_repository.CreateBlog(blogDTO, listpos);

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
