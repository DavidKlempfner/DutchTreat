using System.Collections.Generic;
using System.Linq;
using DutchTreat.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            _logger.LogInformation($"{nameof(GetAllProducts)} was called");
            _logger.LogDebug($"{nameof(GetAllProducts)} was called");
            return _ctx.Products.OrderBy(p => p.Title).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products.Where(p => p.Category == category).ToList();
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _ctx
                .Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                .SingleOrDefault(o => o.Id == id);
        }

        public void AddEntity<T>(T model)
        {
            _ctx.Add(model);
        }
    }
}