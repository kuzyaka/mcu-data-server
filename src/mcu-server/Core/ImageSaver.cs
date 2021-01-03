using System;
using System.IO;
using System.Threading.Tasks;

namespace McuServerApi.core
{
    public interface IImageSaver
    {
        public Task SaveImage(ImageInfo image);
    }
    
    class FileImageSaver : IImageSaver
    {
        private readonly string imageDirectoryPath;
        private readonly string imageDateTimeFormat;

        public FileImageSaver(string imageDirectoryPath, string imageDateTimeFormat)
        {
            this.imageDirectoryPath = imageDirectoryPath;
            this.imageDateTimeFormat = imageDateTimeFormat;
        }

        public async Task SaveImage(ImageInfo image)
        {
            var filePath = Path.Combine(imageDirectoryPath, $"{image.ClientName}_{image.DateTime.ToString(imageDateTimeFormat)}");
            await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(image.Base64Data));
        }
    }
}