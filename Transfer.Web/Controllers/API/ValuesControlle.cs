using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Transfer.Web.Controllers.API
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
