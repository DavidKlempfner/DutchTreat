using System.Linq;
using System;
using System.Collections.Generic;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderid:int}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        public OrderItemsController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderItemViewModel>> Get(int orderId)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);
                if (order != null)
                {
                    var orderItemViewModels = _mapper.Map<IEnumerable<OrderItemViewModel>>(order.Items);
                    return Ok(orderItemViewModels);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<OrderItemViewModel> Get(int orderId, int id)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);
                if (order != null)
                {
                    var orderItem = order.Items.SingleOrDefault(x => x.Id == id);
                    if (orderItem != null)
                    {
                        var orderItemViewModels = _mapper.Map<OrderItemViewModel>(orderItem);
                        return Ok(orderItemViewModels);
                    }
                }

                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}