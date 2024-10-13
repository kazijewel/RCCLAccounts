using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCCLAccounts.WebUi.Common
{

    public class FileUpload
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private IHttpContextAccessor _accessor;
        public FileUpload(IWebHostEnvironment hostEnvironment, IHttpContextAccessor accessor)
        {
            _hostEnvironment = hostEnvironment;
            _accessor = accessor;
        }
        public string getUploadUrl(string fileUrl,string filePath,string filePath2,string fileName,string fileId)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = _accessor.HttpContext.Request.Form.Files;
            string UploadUrl="";
            if(files != null)
            {
                int fileIndex = -1, fileCount = files.Count;
                if(fileId == "")
                {
                    fileIndex = files.Count-1;
                }
                else
                {
                    for (int i = 0; i < fileCount; i++)
                    {
                        if (files[i].Name.Equals(fileId))
                        {
                            fileIndex = i;
                            break;
                        }
                    }
                }

                if (fileIndex != -1)
                {
                    var uploads = Path.Combine(webRootPath, filePath);
                    var extenstion = Path.GetExtension(files[fileIndex].FileName);
                    //Check if directory exist
                    if (!System.IO.Directory.Exists(uploads))
                    {
                        System.IO.Directory.CreateDirectory(uploads); //Create directory if it doesn't exist
                    }
                    if (fileUrl != null)
                    {
                        var path = Path.Combine(webRootPath, fileUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[fileIndex].CopyTo(filesStreams);
                    }
                    UploadUrl = filePath2 + fileName + extenstion;
                    
                }
            }
            
                
            return UploadUrl;
        }
        public string getUploadUrl(string fileUrl, string filePath, string filePath2, string fileName, IList<IFormFile> file)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = file;
            string UploadUrl = "";
            int fileIndex = -1, fileCount = files.Count;
            if(files != null)
            {
                for (int i = 0; i < fileCount; i++)
                {
                    fileIndex = i;
                    break;
                }
                Console.WriteLine(files[0].Name);
                if (fileIndex != -1)
                {
                    var uploads = Path.Combine(webRootPath, filePath);
                    var extenstion = Path.GetExtension(files[fileIndex].FileName);
                    //Check if directory exist
                    if (!System.IO.Directory.Exists(uploads))
                    {
                        System.IO.Directory.CreateDirectory(uploads); //Create directory if it doesn't exist
                    }
                    if (fileUrl != null)
                    {
                        var path = Path.Combine(webRootPath, fileUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[fileIndex].CopyTo(filesStreams);
                    }
                    UploadUrl = filePath2 + fileName + extenstion;
                }
            }
            

            return UploadUrl;
        }


    }
}
