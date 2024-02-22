using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using CatalogAPI.Models;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext? _context;

        public ProductsController(AppDbContext? context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _context.Products.AsNoTracking().ToList();
            if (products is null)
            {
                return NotFound("Products not found!!!");
            }
            return products;
        }

        [HttpGet("{id:int}", Name="FindProduct")]
        public ActionResult<Product> Get(int id)
        {
            var products = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
            if(products is null)
            {
                return NotFound("Product not fund!!!");
            }
            return products;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest();

            _context.Products.Add(product);
            _context.SaveChanges();

            return new CreatedAtRouteResult("FindProduct",
                new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if(id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if(product is null)
            {
                return NotFound("Product not fund!!!");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(product);
        }
    }
}
