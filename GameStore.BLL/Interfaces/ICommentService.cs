using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService : IService<CommentDto>
    {
        List<CommentDto> GetAllByGameKey(string key, bool includeIsDeleted = false);
    }
}
