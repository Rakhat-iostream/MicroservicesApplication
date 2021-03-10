using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repositories.Interfaces
{
    public interface IJwtBuilder
    {
        string GetToken(Guid userId);
        Guid ValidateToken(string token);
    }
}
