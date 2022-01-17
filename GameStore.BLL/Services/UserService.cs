using System;
using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Services
{
    public class UserService : IUserService
    {
        public Result<UserDto> Add(UserDto itemDto)
        {
            throw new NotImplementedException();
        }

        public Result<UserDto> Update(UserDto itemDto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserDto> GetAll(bool includeIsDeleted = false)
        {
            throw new NotImplementedException();
        }

        public UserDto GetByName(string name)
        {
            return new UserDto();
        }

        public void Ban(UserDto user, DateTime until)
        {
            return;
        }
    }
}
