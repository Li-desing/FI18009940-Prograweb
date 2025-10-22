using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Linq;

namespace MVC.Controllers{

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TheModel());
        }

        [HttpPost]
        public IActionResult Index(TheModel model)
        {
            ViewBag.Valid = ModelState.IsValid;

            if (ViewBag.Valid && model.Phrase != null)
            {

                model.Counts = new Dictionary<char, int>();
                model.Lower = string.Empty;
                model.Upper = string.Empty;

                var chars = model.Phrase.Where(c => !char.IsWhiteSpace(c));

                foreach (var c in chars)
                {

                    if (!model.Counts.ContainsKey(c))
                        model.Counts[c] = 0;

                    model.Counts[c]++;

                    model.Lower += char.ToLower(c);
                    model.Upper += char.ToUpper(c);
                }
            }

            return View(model);
        }
    }
}

//Me guie un poco con Copilot 