using ImageMagick;
using MedinaApi.DTO;
using MedinaApi.Helpers;

namespace SS_Online_API.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// 
        /// Upload Normal files, ex./CV
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<UploadFileDto> UploadFile(IFormFile? file, string path)
        {
            try
            {
                if (file == null)
                {
                    return new UploadFileDto
                    {
                        FileName = null,
                    };
                }
                var folderName = Path.Combine("Files/" + path);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return new UploadFileDto
                    {
                        FileName = fileName,
                    };

                }
                else
                {
                    return new UploadFileDto
                    {
                        FileName = null,
                    };
                }
            }
            catch (Exception)
            {
                return new UploadFileDto
                {
                    FileName = null,
                };
            }
        }
        public static async Task<UploadFileDto> UploadFileWithFullPath(IFormFile? file, string FullPath)
        {
            try
            {
                if (file == null)
                {
                    return new UploadFileDto
                    {
                        FileName = null,
                    };
                }

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), FullPath);
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(fullPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return new UploadFileDto
                    {
                        FileName = fileName,
                    };

                }
                else
                {
                    return new UploadFileDto
                    {
                        FileName = null,
                    };
                }
            }
            catch (Exception)
            {
                return new UploadFileDto
                {
                    FileName = null,
                };
            }
        }

        public static async Task<ImageUploadDTO> UploadImage(IFormFile? file, string path, bool shouldResize = true, int width = 1080, int height = 1920, int quality = 50)
        {
            try
            {
                if (file == null)
                {
                    return new ImageUploadDTO
                    {
                        Name = "",
                        Uploaded = false,
                        WasEmpty = true,
                    };
                }
                var folderName = Path.Combine("Files/" + path);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        using MagickImage image = new(stream.ToArray());

                        if (shouldResize)
                        {
                            if (image.Width > width)
                            {
                                image.Resize(width, image.Height);
                            }
                            if (image.Height > height)
                            {
                                image.Resize(image.Width, height);

                            }
                        }

                        image.Quality = quality; // This is the Compression level.
                        image.Write(fullPath);
                    }
                    return new ImageUploadDTO
                    {
                        Uploaded = true,
                        WasEmpty = false,
                        Name = fileName,
                    };
                }
                else
                {
                    return new ImageUploadDTO
                    {
                        Name = "",
                        Uploaded = false,
                        WasEmpty = true,
                    };
                }
            }
            catch (Exception)
            {
                return new ImageUploadDTO
                {
                    Name = "",
                    Uploaded = false,
                };
            }
        }
        public static async Task<ICollection<string>> UploadImages(ICollection<IFormFile?> files, string path, bool shouldResize = true, int width = 1080, int height = 1920, int quality = 50)
        {
            List<string> results = new();
            foreach (var file in files)
            {
                var res = await UploadImage(file, path, shouldResize, width, height, quality);
                if (res.ValidateImageUploadResult())
                {
                    results.Add(res.Name);
                }
            }
            return results;
        }

        public static void DeleteFile(string? name, string path)
        {
            if (name is null or "" or " ")
            {
                return;
            }
            var folderName = Path.Combine("Files/" + path + "/");
            var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), folderName + name);

            FileInfo file = new FileInfo(pathToDelete);

            if (file.Exists)
            {
                file.Delete();
                Console.WriteLine("reached Delete");

            }
        }

        public static void DeleteFileWithFullPath(string name, string fullPath)
        {
            var folderName = Path.Combine(fullPath + "/");
            var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), folderName + name);

            FileInfo file = new FileInfo(pathToDelete);

            if (file.Exists)
            {
                file.Delete();
                Console.WriteLine("reached Delete");

            }
        }
    }
}
