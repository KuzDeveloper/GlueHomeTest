using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using TT.Deliveries.Core.Interfaces;

namespace TT.Deliveries.Web.Api.Controllers
{
    [Route("maintenance")]
    [ApiController]
    [Produces("application/json")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public MaintenanceController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetStatesAsPerAccessWindowAsync()
        {
            await _deliveryService.SetStatesAsPerAccessWindowAsync();
            
            return Ok();
        }
    }
}
