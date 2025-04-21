using Cantina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.Interface
{
    public interface ITokenProvider
    {
        string GenerateToken(ApplicationUser user, List<string> roles);
    }
}
