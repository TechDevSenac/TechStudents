using Microsoft.AspNetCore.Mvc;
using TechStudents.Utils.Entities;
using TechStudents.Web.Models;

namespace TechStudents.Web.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            var model = new UsersModel();
            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(UserModel model)
        {
            var user = new User()
            {
                Name = model.Name,
                NickName = model.NickName,
                LoginType = model.LoginType,
                Password = model.Password,
                Blocked = false,
                LoginCount = 0,
                PersonID = 1,
            };

            user.Create();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var user = new User(id);
            var model = new UserModel(user);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UserModel model)
        {
            var user = model.GenerateUser();
            user.Update();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = new User(id);
            var model = new UserModel(user);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(UserModel model)
        {
            var user = model.GenerateUser();
            user.Delete();

            return RedirectToAction("Index");
        }
    }
}
