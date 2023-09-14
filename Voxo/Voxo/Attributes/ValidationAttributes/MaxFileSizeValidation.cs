using System.ComponentModel.DataAnnotations;

namespace Voxo.Attributes.ValidationAttributes
{
    public class MaxFileSizeValidation:ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSizeValidation(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();

            if(value is IFormFile)
            {
                files.Add((IFormFile)value);
            }
            else if(value is List<IFormFile>)
            {
                files= value as List<IFormFile>;
            }

            foreach(var file in files)
            {
                if (file.Length > _maxSize)
                    return new ValidationResult("Filesize must be less than or equal to" + _maxSize + "byte");
            }

            return ValidationResult.Success;
        }
    }
}
