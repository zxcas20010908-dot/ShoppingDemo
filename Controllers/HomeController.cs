using System.Diagnostics;
using Demo1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo1.Controllers
{
    public class HomeController : Controller
    {
        ShoppingContext db = new ShoppingContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string kind)
        {
            var categories = db.Product.Select(p => p.kind).Distinct().ToList();
            ViewBag.CategoriesList = categories;
            var pro = db.Product.Where(p => p.kind == kind || string.IsNullOrEmpty(kind)).ToList();
            //判斷是否為Ajax請求
            bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_ProductListPartial", pro);
            }
            return View(pro);
        }
        public IActionResult Shopping(string kind)
        {
            var categories = db.Product.Select(p => p.kind).Distinct().ToList();
            ViewBag.CategoriesList = categories;
            var pro = db.Product.Where(p => p.kind == kind || string.IsNullOrEmpty(kind)).ToList();
            //判斷是否為Ajax請求
            bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_ProductListPartial", pro);
            }
            return View(pro);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string account, string password)
        {
            var member = db.Customer.Where(m => m.account == account && m.password == password).FirstOrDefault();
            if (member == null)
            {
                ViewBag.Message = "帳號或密碼錯誤";
                return View("Login");
            }
            HttpContext.Session.SetString("account", "password");
            ViewBag.Message = "登入成功";
            return RedirectToAction("Shopping");
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string account, string password, string name, string tel, string address)
        {
            var acc = db.Customer.Where(mbox => mbox.account == account).FirstOrDefault();
            if (acc != null)
            {
                TempData["AlertMessage"] = "註冊失敗，請檢查資料是否正確。"; //Bootstrap
                TempData["AlertType"] = "danger";
                return View("Register");
            }
            Customer member = new Customer();
            member.account = account;
            member.password = password;
            member.name = name;
            member.tel = tel;
            member.address = address;
            db.Customer.Add(member);
            db.SaveChanges();
            ViewBag.message = "註冊成功，請重新登入";
            return RedirectToAction("Login");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ShoppingCar()
        {
            return View();
        }
    }
}
