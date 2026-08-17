using AmusementPark.Application.Interfaces;

namespace AmusementPark.Web.Services
{
    public class GameImageService : IGameImageService
    {
        private readonly IWebHostEnvironment _environment;

        public GameImageService(
            IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string? GetImageUrl(string? gamePic)
        {
            if (string.IsNullOrWhiteSpace(gamePic))
                return null;

            string imageName = gamePic.Trim();

            // اگر دیتابیس مسیر داده باشد
            imageName = imageName
                .Replace("\\", "/")
                .Replace("/images/", "")
                .Replace("images/", "");

            // Query String حذف شود
            imageName = imageName.Split('?')[0];

            string imagesPath =
                Path.Combine(
                    _environment.WebRootPath,
                    "images");

            if (!Directory.Exists(imagesPath))
                return null;

            // -----------------------------------------
            // اول: بررسی نام کامل فایل
            // مثال:
            // 17.png
            // -----------------------------------------

            string exactPath =
                Path.Combine(
                    imagesPath,
                    imageName);

            if (File.Exists(exactPath))
            {
                return "/images/" +
                       Path.GetFileName(exactPath);
            }

            // -----------------------------------------
            // دوم: بررسی نام بدون پسوند
            // مثال:
            // DB = 17
            // File = 17.png
            // -----------------------------------------

            string fileName =
                Path.GetFileNameWithoutExtension(
                    imageName);

            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            string? matchedFile =
                Directory
                    .GetFiles(imagesPath)
                    .FirstOrDefault(file =>
                        string.Equals(
                            Path.GetFileNameWithoutExtension(file),
                            fileName,
                            StringComparison.OrdinalIgnoreCase));

            if (matchedFile == null)
                return null;

            return "/images/" +
                   Path.GetFileName(matchedFile);
        }
    }
}