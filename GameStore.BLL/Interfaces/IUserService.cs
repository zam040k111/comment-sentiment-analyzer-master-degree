using System;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IUserService : IValidatedService<UserDto>
    {
        UserDto GetByName(string name);
        void Ban(UserDto user, DateTime until);
    }
}
