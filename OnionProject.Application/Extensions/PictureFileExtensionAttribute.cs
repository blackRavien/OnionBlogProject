using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Extensions
{
    public class PictureFileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName).ToLower(); //png jpeg gibi dosya formatı kısmını alabiliriz.

            string[] extensions = { "jpg", "png", "jpeg" };

            bool result = extensions.Any(x => extension.EndsWith(x));

            if (!result)
            {
                return new ValidationResult("Geçerli formatta bir dosya yükleyin! (\"jpg\", \"png\", \"jpeg\")");
            }

            return ValidationResult.Success;
        }
    }
}
