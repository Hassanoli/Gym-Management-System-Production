using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Attachment_Service
{
    public interface IAttachment_Service
    {
        #region Upload
        string? Upload(string FolderName, IFormFile File);
        #endregion

        #region Delete
        bool Delete(string FileName, string FolderName);
        #endregion
    }
}
