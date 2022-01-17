using System.Collections.Generic;
using System.IO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Services.Models;

namespace GameStore.WEB.Services
{
    public class FileService : IFileService
    {
        private static readonly Dictionary<string, string> StrOfType = new Dictionary<string, string> { { FileType.Bin, ".bin" } };

        public MemoryStream Download(string fileName, string fileType)
        {
            if (!CheckIfFileExists(fileName, fileType))
            {
                throw new NotFoundException();
            }

            var memory = new MemoryStream();
            using var stream = new FileStream(GetFullPath(fileName, StrOfType[fileType]), FileMode.Open);
            stream.CopyToAsync(memory);

            memory.Position = 0;

            return memory;
        }
        
        private string GetFullPath(string fileName, string fileType) => 
            Path.Combine(Directory.GetCurrentDirectory(), "Content", fileName + fileType);

        public bool CheckIfFileExists(string fileName, string fileType) => 
            File.Exists(GetFullPath(fileName, StrOfType[fileType]));
    }
}
