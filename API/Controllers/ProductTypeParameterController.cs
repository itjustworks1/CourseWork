using API.DTO.Requests;
using API.NewDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeParameterController : ControllerBase
    {
        private readonly _113526KrylovKursovaiContext db;
        public ProductTypeParameterController(_113526KrylovKursovaiContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductTypeParameterRequest>>> GetList()
        {
            var list = new List<ProductTypeParameterRequest>();
            var listp = await db.Parameters.Include(x => x.ProductTypes).ToListAsync();//список всех параметров 
            foreach (var param in listp)//один параметр из списка всех параметров
            {
                foreach (var ptype in param.ProductTypes)//один тип из списка типов одного параметра из списка всех параметров
                {
                    var listt = await db.ProductTypes.Where(x => x.Id == ptype.Id).Include(x => x.Parameters).ToListAsync();//список типов совподающих id с одним типом из списка типов одного параметра из списка всех параметров
                    foreach (var type in listt)//один тип из списка типов совподающих id с одним типом из списка типов одного параметра из списка всех параметров
                    {
                        var request = new ProductTypeParameterRequest
                        {
                            ParameterId = param.Id,
                            ProductTypeId = type.Id
                        };
                        list.Add(request);
                    }
                }
            }
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductTypeParameterRequest request)
        {
            var obj = await db.Parameters.Include(x => x.ProductTypes).FirstOrDefaultAsync(x => x.Id == request.ParameterId);
            if (obj == null)
                return NotFound();

            var type = await db.ProductTypes.Include(x => x.Parameters).FirstOrDefaultAsync(x => x.Id == request.ProductTypeId);
            if (type == null)
                return NotFound();
            obj.ProductTypes.Add(type);

            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{idp}/{idt}")]
        public async Task<ActionResult> Delete(int idp, int idt)
        {
            var obj = await db.Parameters.Include(x => x.ProductTypes).FirstOrDefaultAsync(x => x.Id == idp);
            if (obj == null)
                return NotFound();

            var type = await db.ProductTypes.Include(x => x.Parameters).FirstOrDefaultAsync(x => x.Id == idt);
            if (type == null)
                return NotFound();
            obj.ProductTypes.Remove(type);

            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
