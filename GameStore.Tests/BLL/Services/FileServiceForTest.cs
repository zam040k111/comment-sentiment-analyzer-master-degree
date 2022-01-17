using System.IO;

namespace GameStore.Tests.BLL.Services
{
    public class FileServiceForTest
    {
        private string DirectoryName { get; }

        public FileServiceForTest(string gameName = "testName", string fileType = ".bin", string directoryName = "Content")
        {
            DirectoryName = directoryName;

            var dir = Path.Combine(Directory.GetCurrentDirectory(), directoryName);
            var pathToFile = Path.Combine(dir, gameName + fileType);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var f = File.Create(pathToFile);
            f.Close();
        }

        public void RemoveCreatedDirectory()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), DirectoryName);
            var file = Directory.GetFiles(dir);
            File.Delete(file[0]);
            Directory.Delete(dir);
        }
    }
}
