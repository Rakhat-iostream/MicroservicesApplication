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
        Task<IEnumerable<Catalog>> GetCatalogByCategory(string category);
        Task<Catalog> GetCatalog(string id);
        Task<Catalog> CreateCatalog(Catalog model);
    }
}
