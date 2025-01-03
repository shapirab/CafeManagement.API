using CafeManagement.Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(UserEntity user);
    }
}
