using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using Size = SixLabors.ImageSharp.Size;

namespace Core.Services
{
    public class FilesService : IFilesService
    {
        private const string imageFolder = "uploads";
        private readonly IWebHostEnvironment _environment;
        int[] sizes = { 320, 600, 1200 };

        public FilesService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveProductImage(IFormFile file)
        {
            try
            {
                string root = _environment.WebRootPath;
                string newNameFile = Guid.NewGuid().ToString();
                string fileName = $"{newNameFile}.webp";

                foreach (int size in sizes)
                {
                    string fullFileName = $"{size}_{newNameFile}.webp";
                    string imagePath = Path.Combine(imageFolder, fullFileName);
                    string imageFullPath = Path.Combine(root, imagePath);
                    {
                        using (var image = Image.Load(file.OpenReadStream()))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(size, size),
                                Mode = ResizeMode.Max
                            }));
                            await image.SaveAsync(imageFullPath, new WebpEncoder());
                        }
                    }
                }
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні файлу {ex.Message}");
                return ex.Message;
            }
        }
        public async Task DeleteProductImage(string imagePath)
        {
            await Task.Run(() =>
            {
                string root = _environment.WebRootPath;
                foreach (int size in sizes)
                {
                    string fullRoot = Path.Combine(root, imageFolder);
                    string imageSizePath = $"{size}_{imagePath}";
                    string imageFullPath = Path.Combine(fullRoot, imageSizePath);
                    if (File.Exists(imageFullPath))
                    {
                        File.Delete(imageFullPath);
                    }
                }
            });
        }
        public async Task<string> UpdateProductImage(string fileName, IFormFile file)
        {
            string root = _environment.WebRootPath;
            string imageFullPath = Path.Combine(root, fileName);
            if (File.Exists(imageFullPath))
            {
                File.Delete(imageFullPath);
            }

            using (FileStream fileStream = new FileStream(imageFullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public async Task<string> SaveUserImageAsync(IFormFile file)
        {
            try
            {
                string root = _environment.WebRootPath;
                string newNameFile = Guid.NewGuid().ToString();
                string fileName = $"{newNameFile}.webp";

                string fullFileName = $"{newNameFile}.webp";
                string imagePath = Path.Combine(imageFolder, fullFileName);
                string imageFullPath = Path.Combine(root, imagePath);
                {
                    using (var image = Image.Load(file.OpenReadStream()))
                    {
                        //image.Mutate(x => x.Resize(new ResizeOptions
                        //{
                        //    Size = new Size(size, size),
                        //    Mode = ResizeMode.Max
                        //}));
                        await image.SaveAsync(imageFullPath, new WebpEncoder());
                    }
                }
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні файлу {ex.Message}");
                return ex.Message;
            }
        }
        public async Task DeleteUserImageAsync(string imagePath)
        {
            await Task.Run(() =>
            {
                string root = _environment.WebRootPath;
                string fullRoot = Path.Combine(root, imageFolder);
                string imageFullPath = Path.Combine(fullRoot, imagePath);
                if (File.Exists(imageFullPath))
                {
                    File.Delete(imageFullPath);
                }
            });
        }
        public async Task<string> SaveImageFromUrlAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var fileStream = await response.Content.ReadAsStreamAsync();
                            string root = _environment.WebRootPath;
                            string newNameFile = Guid.NewGuid().ToString();
                            string fileName = $"{newNameFile}.webp";
                            string imageFolder = "uploads"; // Папка для збереження зображень

                            string fullFileName = $"{newNameFile}.webp";
                            string imagePath = Path.Combine(imageFolder, fullFileName);
                            string imageFullPath = Path.Combine(root, imagePath);

                            using (var image = Image.Load(fileStream))
                            {
                                await image.SaveAsync(imageFullPath, new WebpEncoder());
                            }

                            return fileName;
                        }
                        else
                        {
                            throw new Exception("Не вдалося завантажити файл з вказаної URL.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні файлу з URL: {ex.Message}");
                return ex.Message;
            }
        }
    }
}
