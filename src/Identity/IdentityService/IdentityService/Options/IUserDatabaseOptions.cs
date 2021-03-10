using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Options
{
   public interface IUserDatabaseOptions
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string Database { get; set; }
    }
}
