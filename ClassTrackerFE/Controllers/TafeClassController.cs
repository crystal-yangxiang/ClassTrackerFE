using ClassTrackerFE.Models;
using ClassTrackerFE.Models.DTO;
using ClassTrackerFE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ClassTrackerFE.Controllers
{
    public class TafeClassController : Controller
    {
        private readonly IApiRequest<Teacher> _teacherService;
        private readonly IApiRequest<TafeClass> _tafeClassService;

 
        public TafeClassController(IApiRequest<Teacher> teacherService, IApiRequest<TafeClass> tafeClassService)
        {
            _tafeClassService = tafeClassService;
            _teacherService = teacherService;

        }

        // GET: TafeClassController
        public ActionResult Index()
        {
            var tafeClasses = _tafeClassService.GetAll("TafeClass");
            return View(tafeClasses);
        }

        // GET: TafeClassController/Details/5
        public ActionResult Details(int id)
        {
            var tafeClass = _tafeClassService.GetSingle("TafeClass", id);
            return View(tafeClass);
        }

        // GET: TafeClassController
        public ActionResult TafeClassesForTeacherId(int id)
        {
            var tafeClasses = _tafeClassService.GetChildrenForParentID("TafeClass", "GetForTeacherId", id);
            return View("Index", tafeClasses);
        }

        // GET: TafeClassController/Create
        public ActionResult Create()
        {
            // Get a list of all teachers
            var teachers = _teacherService.GetAll("Teacher");//get all the teachers

            // Create a list of selectlistitems from teacher list
            var teacherSelect = teachers.Select(c => new SelectListItem
            {
                Value = c.TeacherId.ToString(),
                Text = c.TeacherName
            }).ToList();

            // Not Linq
            /* var teacherSelect2 = new List<SelectListItem>();

             foreach (var teacher in teachers)
             {
                 var teacherItem = new SelectListItem 
                 { 
                     Value = teacher.TeacherId.ToString(),
                     Text = teacher.TeacherName 
                 };
                 teacherSelect2.Add(teacherItem);
             }*/

            // Store this list in memory

            // ViewBag - Dynamic Object. Works with everything. Is heavy and can be slow with lots of data.
            ViewBag.TeacherSelect = teacherSelect;

            // ViewData - Key/Value(String/Object) Dictionary. Have to box and unbox but is light.
            //ViewData.Add("TeacherSelect", teacherSelect);


            return View();
        }

        // POST: TafeClassController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TafeClassCreate tafeClass)
        {
            try
            {
                TafeClass newClass = new TafeClass
                {
                    TafeClassName = tafeClass.TafeClassName,
                    TafeClassLocation = tafeClass.TafeClassLocation,
                    TafeClassStartDT = tafeClass.TafeClassStartDT,
                    TeacherId = tafeClass.TeacherId
                };

                _tafeClassService.Create("TafeClass", newClass);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TafeClassController/Edit/5
        public ActionResult Edit(int id)
        {
            var tafeClassToEdit = _tafeClassService.GetSingle("TafeClass", id);
            return View(tafeClassToEdit);
        }

        // POST: TafeClassController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TafeClass tafeClass)
        {
            try
            {
                _tafeClassService.Edit("TafeClass", id, tafeClass);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TafeClassController/Delete/5
        public ActionResult Delete(int id)
        {
            var tafeClassToDelete = _tafeClassService.GetSingle("TafeClasss", id);
            return View(tafeClassToDelete);
        }

        // POST: TafeClassController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TafeClass tafeClass)
        {
            try
            {
                _tafeClassService.Delete("TafeClass", id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
