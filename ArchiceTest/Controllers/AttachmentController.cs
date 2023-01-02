using ArchiceTest.Models;
using ArchiceTest.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ArchiceTest.Controllers
{
    public class AttachmentController : Controller
    {
        
        private readonly Context _context;
        public AttachmentController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files, Attachment attachment)
        {
            long size = files.Sum(f => f.Length);
            string filename = string.Empty;
            
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    // full path to file in temp location
                    var filePath = Path.GetFullPath( Path.Combine(Directory.GetCurrentDirectory(),formFile.FileName));
                    
                    filePaths.Add(filePath);
                    
                    attachment.AttachPath = filePath;
                    filename = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
                    _context.Add(attachment);
                  if (formFile.ContentType.Contains("image") ) {  
                    using (var image = Image.Load(formFile.OpenReadStream()))
                    {
                        string newsize = ImageResize(image, 1500, 1500);
                        string[] sizearray = newsize.Split(',');
                        image.Mutate(x => x.Resize(Convert.ToInt32(sizearray[1]), Convert.ToInt32(sizearray[0])));
                        image.Save(filePath);

                    }
                    }
                    else { 
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await formFile.CopyToAsync(stream);
                    }
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public string ImageResize(Image img,int MaxWidth,int MaxHeight)
        {
            if(img.Width>MaxWidth || img.Height > MaxHeight)
            {
                double widthratio = (double)img.Width / (double)MaxWidth;
                double heightratio = (double)img.Height / (double)MaxHeight;
                double ratio = Math.Max(widthratio, heightratio);
                int newWidth = (int)(img.Width / ratio);
                int newHeight = (int)(img.Height / ratio);
                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() + "," + img.Width.ToString();
            }
        }
    }

}