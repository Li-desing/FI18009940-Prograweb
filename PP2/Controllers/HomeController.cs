using Microsoft.AspNetCore.Mvc;
using PP2.Models;
using PP2.Helpers;

namespace PP2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new BinarioModel());
        }

        [HttpPost]
        public IActionResult Index(BinarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = new ResultadoModel
            {
                A = model.a,
                B = model.b,
                A_Padded = BinaryHelper.PadToByte(model.a),
                B_Padded = BinaryHelper.PadToByte(model.b),
                BinaryAnd = BinaryHelper.BinaryAnd(model.a, model.b),
                BinaryOr = BinaryHelper.BinaryOr(model.a, model.b),
                BinaryXor = BinaryHelper.BinaryXor(model.a, model.b),
                BinarySum = BinaryHelper.BinarySum(model.a, model.b),
                BinaryMul = BinaryHelper.BinaryMul(model.a, model.b)
            };

            return View("Resultado", resultado);
        }
    }
}
