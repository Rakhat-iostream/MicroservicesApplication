using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
