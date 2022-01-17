using System.IO;

namespace GameStore.WEB.Interfaces
{
    public interface IFileService
    {
        MemoryStream Download(string fileName, string fileType);
        bool CheckIfFileExists(string fileName, string fileType);
    }
}
