using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            try
            {
                return Ok(_repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);
                if (order != null)
                {
                    return Ok(order);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Order model)
        {
            try
            {
                _repository.AddEntity(model);
                if (_repository.SaveAll())
                {
                    return Created($"/api/orders/{model.Id}", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }

            return BadRequest("Failed to save the order");
        }
    }
}