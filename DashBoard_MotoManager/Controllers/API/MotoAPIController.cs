using DashBoard_MotoManager.Datas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashBoard_MotoManager.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoAPIController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;

        public MotoAPIController(MotoWebsiteContext context, ILogger<MotoController> logger)
        {
            _db = context;
            _logger = logger;
        }
    }
}
