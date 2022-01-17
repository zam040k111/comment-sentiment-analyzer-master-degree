using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Services.Validation
{
    public class Result<T>
    {
        public bool IsValid => !Errors.Any();

        public bool IsRestored { get; set; }

        public T Value { get; set; }

        public Dictionary<string, string> Errors { get; }

        public Result()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}
