using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Filter
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName); //123.jpg
                string[] extensions = { "jpg", "png", "jpg" };

                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result) {
                 return new ValidationResult("allowed extensioms are jpg or png or jpg");

                }
            }
           return ValidationResult.Success;
        }
    }
}
