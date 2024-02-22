using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly AppDbContext? _context;

        public CategorysController(AppDbContext? context)
        {
            _context = context;
        }

        [HttpGet("CategorysProducts")]
        public ActionResult<IEnumerable<Category>> GetCategorysProducts()
        {
            return _context.Categorys.Include(p => p.Products).AsNoTracking().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categorys = _context.Categorys.AsNoTracking().ToList();
            if (categorys is null)
            {
                return NotFound("Categorys not found!!!");
            }
            return categorys;
        }

        [HttpGet("{id:int}", Name = "FindCategory")]
        public ActionResult<Category> Get(int id)
        {
            var categorys = _context.Categorys.AsNoTracking().FirstOrDefault(p => p.CategoryId == id);
            if (categorys is null)
            {
                return NotFound("Category not fund!!!");
            }
            return categorys;
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
                return BadRequest();

            _context.Categorys.Add(category);
            _context.SaveChanges();

            return new CreatedAtRouteResult("FindCategory",
                new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categorys.FirstOrDefault(p => p.CategoryId == id);

            if (category is null)
            {
                return NotFound("Category not fund!!!");
            }

            _context.Categorys.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}
