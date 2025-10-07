using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PP2.Models
{
    public class BinarioModel
    {
        [Display(Name = "a")]
        [Required(ErrorMessage = "El campo 'A' es obligatorio.")]
        [BinaryStringValidation]
        public string a { get; set; }

        [Display(Name = "b")]
        [Required(ErrorMessage = "El campo 'B' es obligatorio.")]
        [BinaryStringValidation]
        public string b { get; set; }

        public class BinaryStringValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var s = value as string;

                if (string.IsNullOrEmpty(s))
                    return new ValidationResult("El valor no puede estar vacío.");

                if (s.Length > 8)
                    return new ValidationResult("El valor no puede tener más de 8 caracteres.");

                if (s.Length % 2 != 0)
                    return new ValidationResult("La longitud debe ser múltiplo de 2 (2, 4, 6 u 8).");

                if (!Regex.IsMatch(s, "^[01]+$"))
                    return new ValidationResult("El valor solo puede contener 0s y 1s.");

                return ValidationResult.Success;
            }
        }
    }
}