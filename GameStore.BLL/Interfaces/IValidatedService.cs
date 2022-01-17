using System.Collections.Generic;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Interfaces
{
    public interface IValidatedService<TModelDto> where TModelDto : class
    {
        Result<TModelDto> Add(TModelDto itemDto);
        Result<TModelDto> Update(TModelDto itemDto);
        void Delete(int id);
        TModelDto GetById(int id);
        List<TModelDto> GetAll(bool includeIsDeleted = false);
    }
}
