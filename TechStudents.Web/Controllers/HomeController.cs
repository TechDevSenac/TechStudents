using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechStudents.Utils.Entities;
using TechStudents.Web.Models;

namespace TechStudents.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (LoginControl._UserLogged == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Login()
        {
            if (LoginControl._UserLogged != null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            var user = new User(loginModel.User);
            var status = user.ValidarSenha(loginModel.Password);

            switch (status)
            {
                case LoginStatus.Sucess:
                    LoginControl._UserLogged = user;
                    return RedirectToAction("Index");
                    break;
                case LoginStatus.InvalidPassword:
                    loginModel.Message = "Usuário ou senha inválidos!";
                    break;
                case LoginStatus.Blocked:
                    loginModel.Message = "Usuário bloqueado. Entre em contato com o administrador.";
                    break;
            }

            return View(loginModel);
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
    }
}