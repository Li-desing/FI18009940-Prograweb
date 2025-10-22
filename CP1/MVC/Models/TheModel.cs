using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVC.Models{

    public class TheModel
    {
        [Required(ErrorMessage = "La frase es obligatoria.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "La frase debe tener entre 5 y 25 caracteres.")]
        public string? Phrase { get; set; }

        public Dictionary<char, int>? Counts { get; set; }

        public string Lower { get; set; } = string.Empty;

        public string Upper { get; set; } = string.Empty;
    }
}

//Me guie un poco con Copilot y Chatgpt 