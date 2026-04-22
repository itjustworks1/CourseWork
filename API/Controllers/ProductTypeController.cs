using API.DTO.Raquests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.NewDB;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public ProductTypeController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductType>>> GetList()
        {
            var list = await db.ProductTypes.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<ProductType>>> Get(int id)
        {
            var obj = await db.ProductTypes.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ProductTypeRequest request)
        {
            var obj = new ProductType
            {
                Title = request.Title,
            };
            db.ProductTypes.Add(obj);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromForm] ProductTypeRequest request)
        {
            var obj = await db.ProductTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();

            obj.Title = request.Title;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var obj = await db.ProductTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();
            List<Product> structures = await db.Products.Where(x => x.ProductTypeId == obj.Id).ToListAsync();
            foreach (var structure in structures)
            {
                db.Products.Remove(structure);
            }
            db.ProductTypes.Remove(obj);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}

