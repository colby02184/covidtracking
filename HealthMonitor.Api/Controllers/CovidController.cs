using HealthMonitor.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthMonitor.Api.Controllers
{
    [ApiController]
    [Route("api/covid")]
    public class CovidController(ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCovidData()
        {
            var data = await dbContext.CovidData.Take(100).ToListAsync();
            return Ok(data);
        }
    }
}
