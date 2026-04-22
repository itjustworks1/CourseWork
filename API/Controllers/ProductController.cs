using API.DTO.Raquests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.NewDB;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public ProductController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetList()
        {
            var list = await db.Products.ToListAsync();
            //var list = await db.Products.Include(s => s.ProductType).ThenInclude(s => new ProductType
            //{
            //    Id = s.Id,
            //    Parameters = s.Parameters,
            //    Products = s.Products,
            //    Title = s.Title
            //}).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<Product>>> Get(int id)
        {
            var obj = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ProductRequest request)
        {
            var obj = new Product
            {
                Title = request.Title,
                Quantity = request.Quantity,
                Value = request.Value,
                ProductTypeId = request.ProductTypeId
            };
            db.Products.Add(obj);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromForm] ProductRequest request)
        {
            var obj = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();

            obj.Title = request.Title;
            obj.Quantity = request.Quantity;
            obj.Value = request.Value;
            obj.ProductTypeId = request.ProductTypeId;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var obj = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();
            List<ProductParameter> structures = await db.ProductParameters.Where(x => x.ProductId == obj.Id).ToListAsync();
            foreach (var structure in structures)
            {
                db.ProductParameters.Remove(structure);
            }
            db.Products.Remove(obj);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}

