using System;

namespace GameStore.BLL.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public DateTime BannedUntil { get; set; }
    }
}
