using API.DTO.Raquests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Magaz_Stroitelya.NewDB;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public ParameterController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Parameter>>> GetList()
        {
            var list = await db.Parameters.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Parameter>> Get(int id)
        {
            var obj = await db.Parameters.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ParameterRequest request)
        {
            var obj = new Parameter
            {
                Title = request.Title,
            };
            db.Parameters.Add(obj);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromForm] ParameterRequest request)
        {
            var obj = await db.Parameters.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();

            obj.Title = request.Title;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var obj = await db.Parameters.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
                return NotFound();
            List<ProductParameter> structures = await db.ProductParameters.Where(x => x.ParameterId == obj.Id).ToListAsync();
            foreach (var structure in structures)
            {
                db.ProductParameters.Remove(structure);
            }
            db.Parameters.Remove(obj);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
