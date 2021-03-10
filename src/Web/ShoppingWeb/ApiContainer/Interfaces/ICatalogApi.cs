using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiContainer.Interfaces
{
    public interface ICatalogApi
    {
        Task<IEnumerable<Catalog>> GetCatalog();
        
        Task<Catalog> CreateCatalog(Catalog model);
        Task<Catalog> GetProduct(string id);
        Task<IEnumerable<Catalog>> GetProductByCategory(string categoryName);
        Task<IEnumerable<Catalog>> GetProductByPage(int pageNumber);
        Task<IEnumerable<Catalog>> GetFilteredProducts(string productName);
    }
}
