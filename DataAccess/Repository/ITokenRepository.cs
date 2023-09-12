using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RestoAppAPI.Repository
{
     public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}