using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("GetData/{someData}")]
        public async Task<long> GetSomeData([Required] long someData)
        {
            var res = await Task.Run(() => someData * 2);
            return res;
        }
    }
}
