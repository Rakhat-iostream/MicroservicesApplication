using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Options
{
    public class UserDatabaseOptions : IUserDatabaseOptions
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string Database{ get; set; }
    }
}
