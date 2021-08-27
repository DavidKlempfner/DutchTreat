using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
        IEnumerable<Order> GetAllOrdersByUser(bool includeItems, string username);
        IEnumerable<Order> GetAllOrders(bool includeItems);
        Order GetOrderById(string username, int id);
        void AddEntity<T>(T model);
    }
}