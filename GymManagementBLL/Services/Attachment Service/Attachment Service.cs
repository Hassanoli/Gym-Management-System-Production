using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Attachment_Service
{
    public class Attachment_Service : IAttachment_Service
    {
        #region Fields
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructor
        public Attachment_Service(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Upload
        public string? Upload(string FolderName, IFormFile File)
        {
            try
            {
                if (FolderName is null || File is null || File.Length == 0) return null;

                if (File.Length > MaxFileSizeInBytes) return null;

                var Extension = Path.GetExtension(File.FileName).ToLower();

                if (!AllowedExtensions.Contains(Extension)) return null;

                var FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", FolderName);

                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                var FileName = Guid.NewGuid().ToString() + Extension;
                var FilePath = Path.Combine(FolderPath, FileName);

                using var Filestream = new FileStream(FilePath, FileMode.Create);
                File.CopyTo(Filestream);

                return FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder = {FolderName} : {ex}");
                return null;
            }
        }
        #endregion

        #region Delete
        public bool Delete(string FileName, string FolderName)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return false;

                var FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", FolderName, FileName);

                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder = {FolderName} : {ex}");
                return false;
            }
        }
        #endregion
    }
}
