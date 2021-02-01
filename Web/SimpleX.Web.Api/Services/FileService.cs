using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Services
{
    public interface IFileService
    {
        Task SaveFile(IFormFile file, string subDirectory);
        Task SaveFiles(List<IFormFile> files, string subDirectory);
    }

    public class FileService : IFileService
    {
        public async Task SaveFile(IFormFile file, string subDirectory)
        {
            if (file.Length <= 0)
                return;

            subDirectory = subDirectory ?? string.Empty;
            var target = Path.Combine(Directory.GetCurrentDirectory(), subDirectory);

            Directory.CreateDirectory(target);

            var filePath = Path.Combine(target, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task SaveFiles(List<IFormFile> files, string subDirectory)
        {
            foreach (var file in files)
            {
                await SaveFile(file, subDirectory);
            }
        }

        public static string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            switch (fileSize)
            {
                case var _ when fileSize < kilobyte:
                    return $"Less then 1KB";
                case var _ when fileSize < megabyte:
                    return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
                case var _ when fileSize < gigabyte:
                    return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
                case var _ when fileSize >= gigabyte:
                    return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
                default:
                    return "n/a";
            }
        }
    }
}
