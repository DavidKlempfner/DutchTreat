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

        public IEnumerable<Order> GetAllOrdersByUser(bool includeItems, string username)
        {
            return GetOrders(includeItems, username);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            return GetOrders(includeItems);
        }

        private IEnumerable<Order> GetOrders(bool includeItems, string username = null)
        {
            if (includeItems)
            {
                var orders = _ctx
                    .Orders
                    .Where(o => string.IsNullOrEmpty(username) || o.User.UserName == username)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Product)
                    .Include(u => u.User)
                    .ToList();
                return orders;
            }
            return _ctx
                .Orders
                .Where(o => string.IsNullOrEmpty(username) || o.User.UserName == username)
                .ToList();
        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                .SingleOrDefault(o => o.Id == id && o.User.UserName == username);
        }

        public void AddEntity<T>(T model)
        {
            _ctx.Add(model);
        }
    }
}