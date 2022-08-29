using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}
