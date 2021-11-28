using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using TT.Deliveries.Core.Exceptions;
using TT.Deliveries.Core.Interfaces;
using TT.Deliveries.Web.Api.Contracts;

namespace TT.Deliveries.Web.Api.Controllers
{
    [Route("deliveries")]
    [ApiController]
    [Produces("application/json")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IUrlsProvider _urlsProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveriesController> _logger;

        public DeliveriesController(IDeliveryService deliveryService,
            IUrlsProvider urlsProvider,
            IMapper mapper,
            ILogger<DeliveriesController> logger)
        {
            _deliveryService = deliveryService;
            _urlsProvider = urlsProvider;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Delivery), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        //[Authorize(Policy = )] - authorization policy would be set here to allow only certain roles or groups to make an api endpoint call.
        public async Task<IActionResult> GetDeliveryAsync([Required] Guid id)
        {
            try
            {
                var delivery = await _deliveryService.GetDeliveryAsync(id);

                var mappedDelivery = _mapper.Map<Delivery>(delivery);

                return Ok(mappedDelivery);
            }
            catch (Exception ex)
            {
                if (ex is DeliveryNotFoundException)
                {
                    _logger.LogWarning($"GetDeliveryAsync was requested with id {id} and resource was not found.");

                    return NotFound(ex.Message);
                }

                throw;
            }
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]        
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddDeliveryAsync([Required] [FromBody] Delivery delivery)
        {
            try
            {
                var mappedDelivery = _mapper.Map<Core.Entities.Delivery>(delivery);

                var deliveryId = await _deliveryService.AddDeliveryAsync(mappedDelivery);

                return Ok(_urlsProvider.GetDeliveryUrl(deliveryId));
            }
            catch (Exception ex)
            {
                if (ex is AutoMapperMappingException)
                {
                    return BadRequest($"The delivery has incorrect body specified. Error: {ex.Message}");
                }

                if (ex is DeliveryNullException)
                {
                    _logger.LogWarning($"AddDeliveryAsync was requested with null parameter.");

                    return StatusCode((int)HttpStatusCode.MethodNotAllowed, ex.Message);
                }

                if (ex is DeliveryAccessWindowException)
                {
                    _logger.LogWarning($"AddDeliveryAsync was requested but its access window was outside of current date.");

                    return StatusCode((int)HttpStatusCode.MethodNotAllowed, ex.Message);
                }

                _logger.LogError($"Error when calling AddDeliveryAsync.", ex);

                throw;
            }
        }

        [HttpGet("cancel/{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelDeliveryAsync([Required] Guid id)
        {
            try
            {
                await _deliveryService.CancelDeliveryAsync(id);

                return Ok(_urlsProvider.GetDeliveryUrl(id));
            }
            catch (Exception ex)
            {
                if (ex is DeliveryNotFoundException)
                {
                    _logger.LogWarning($"CancelDeliveryAsync was requested with id {id} and resource was not found.");

                    return NotFound(ex.Message);
                }

                if (ex is DeliveryStatusException)
                {
                    _logger.LogWarning($"CancelDeliveryAsync was requested with id {id} but its status does not allow cancellation.");

                    return StatusCode((int)HttpStatusCode.MethodNotAllowed, ex.Message);
                }

                _logger.LogError($"Error when calling CancelDeliveryAsync.", ex);

                throw;
            }
        }

        [HttpPatch("update/{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateDeliveryAsync([Required] Guid id, [Required] [FromBody] Delivery delivery)
        {
            try
            {
                var mappedDelivery = _mapper.Map<Core.Entities.Delivery>(delivery);
                
                await _deliveryService.UpdateDeliveryAsync(id, mappedDelivery);

                return Ok(_urlsProvider.GetDeliveryUrl(id));
            }
            catch (Exception ex)
            {
                if (ex is DeliveryNotFoundException)
                {
                    _logger.LogWarning($"UpdateDeliveryAsync was requested with id {id} and resource was not found.");

                    return NotFound(ex.Message);
                }

                if (ex is DeliveryAccessWindowException)
                {
                    _logger.LogWarning($"UpdateDeliveryAsync was requested with id {id} but its access window was outside of current date.");

                    return StatusCode((int)HttpStatusCode.MethodNotAllowed, ex.Message);
                }

                _logger.LogError($"Error when calling UpdateDeliveryAsync.", ex);

                throw;
            }
        }
    }
}
