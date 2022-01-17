using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class PlatformTypeDto
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public List<int?> GamePlatformTypesId { get; set; }
    }
}
