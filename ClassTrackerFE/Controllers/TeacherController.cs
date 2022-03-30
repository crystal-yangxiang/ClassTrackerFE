using ClassTrackerFE.Models;
using ClassTrackerFE.Models.DTO;
using ClassTrackerFE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClassTrackerFE.Controllers
{
    public class TeacherController : Controller
    {
        // Pull service from Startup Class
        private readonly IApiRequest<Teacher> _teacherService;
        private string controllerName = "Teacher";
        public TeacherController(IApiRequest<Teacher> teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: TeacherController
        public ActionResult Index()
        {
            var teachers = _teacherService.GetAll(controllerName);
            return View(teachers);
        }

        // GET: TeacherController/Details/5
        public ActionResult Details(int id)
        {
            // Check to see if there is a token, if not - redirect to the login page

            //FE login check
            if (Helpers.AuthHelper.IsNotLoggedIn(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            //if(!HttpContext.Session.Keys.Any(c => c.Equals("Token")))
            //{
            //    return RedirectToAction("Login", "Auth");
            //}

            var teacher = _teacherService.GetSingle(controllerName, id);
            return View(teacher);
        }

        // GET: TeacherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeacherCreate teacher)
        {
            try
            {
                Teacher newTeacher = new Teacher()
                {
                    TeacherName = teacher.TeacherName,
                    TeacherEmail = teacher.TeacherEmail,
                    TeacherPhone = teacher.TeacherPhone,
                };
                _teacherService.Create(controllerName, newTeacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherController/Edit/5
        public ActionResult Edit(int id)
        {
            if (Helpers.AuthHelper.IsNotLoggedIn(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(_teacherService.GetSingle(controllerName, id));
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Teacher teacher)
        {
            try
            {
                if (Helpers.AuthHelper.IsNotLoggedIn(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _teacherService.Edit(controllerName, id, teacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherController/Delete/5
        public ActionResult Delete(int id)
        {
            if (Helpers.AuthHelper.IsNotLoggedIn(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(_teacherService.GetSingle(controllerName, id));
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Teacher teacher)
        {
            try
            {
                if (Helpers.AuthHelper.IsNotLoggedIn(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var test = HttpContext.Request;
                _teacherService.Delete(controllerName, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
