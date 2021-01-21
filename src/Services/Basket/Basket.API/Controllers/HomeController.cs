using Microsoft.AspNetCore.Mvc;

namespace Services.Basket.API.Controllers
{
    public class HomeController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
