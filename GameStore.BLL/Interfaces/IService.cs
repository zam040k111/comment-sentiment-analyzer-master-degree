
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IService<TModelDto> where TModelDto : class
    {
        TModelDto Add(TModelDto itemDto);
        TModelDto Update(TModelDto itemDto);
        void Delete(int id);
        TModelDto GetById(int id);
        List<TModelDto> GetAll(bool includeIsDeleted = false);
    }
}
