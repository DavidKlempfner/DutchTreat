using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get(bool includeItems = true)
        {
            try
            {
                var result = _repository.GetAllOrders(includeItems);

                var mappedResult = _mapper.Map<IEnumerable<OrderViewModel>>(result);
                return Ok(mappedResult);
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
                    var orderViewModel = _mapper.Map<Order, OrderViewModel>(order);
                    return Ok(orderViewModel);
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
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<Order>(model);

                    if (newOrder.OrderDate == default)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    _repository.AddEntity(newOrder);
                    if (_repository.SaveAll())
                    {
                        var newViewModel = _mapper.Map<OrderViewModel>(newOrder);
                        return Created($"/api/orders/{newOrder.Id}", newViewModel);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
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