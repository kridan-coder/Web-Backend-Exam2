using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackendTest2.Data;
using BackendTest2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BackendTest2.Services;
using BackendTest2.Models.ViewModels;

namespace BackendTest2.Controllers
{
    [Authorize]
    public class HogTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserPermissionsService userPermissions;
        public HogTasksController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            this.userPermissions = userPermissions;
            this.signInManager = signInManager;
        }

        // GET: HogTasks
        public async Task<IActionResult> Index()
        {
            //use this for adding theme and tasks
            var theme = new Theme() { Name = "Сортировки", MinimumPoints = 15 };
            var taskTMP = new HogTask() { Cost = 3, Theme = theme, ThemeId = theme.Id,  FileNameIn = "stdIn", FileNameOut = "stdOut", FilePathIn="/", FilePathOut="/", Description = "Сортировка пузырьком - простой алгоритм сортировки. Для понимания и реализации этот алгоритм — простейший, но эффективен он лишь для небольших массивов.", Name = "Сортировка пузырьком", Postmoderation = false };

           try
            {
                this._context.Add(theme);
                this._context.Add(taskTMP);
                await this._context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return this.RedirectToAction("Index", "Error", new { error = e.Message });
            }
            //use this for adding theme and tasks
            var allTasks = await _context.HogTasks.Include(h => h.Theme).ToListAsync();
            ViewBag.AllTasks = allTasks;
            if (signInManager.IsSignedIn(User)){
                var userId = await userPermissions.GetCurrentUserId();
                var userHogTasks = await  _context.TaskUsers.Where(x => x.UserId == userId).ToListAsync();
                ViewBag.AllUserTasks = userHogTasks;
                Int64 points = 0;
                foreach (var item in userHogTasks)
                {
                    if (item.Verdict == "OK")
                    {
                        var task = allTasks.Find(x => x.Id == item.HogTaskId);
                        points += task.Cost;
                    }
                }
                ViewBag.Points = points;
            }
            var allThemes = await _context.Themes.ToListAsync();
/*            var allThemesNames = new List<String>();
            foreach (var item in allThemes)
            {
                allThemesNames.Add(item.Name);
            }*/
            this.ViewData["Themes"] = new SelectList(allThemes, "Id", "Name");
/*            ViewBag.Themes = allThemesNames;*/
            return View();
        }


        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Index(TasksFilterViewModel model)
        {

            var allTasks =_context.HogTasks
                .Include(x => x.Theme)
   .Where(u => u.Name == model.Name || u.Theme.Name == model.Name).ToList();
            if (model.Name == null && model.Theme == null)
            {
                allTasks = _context.HogTasks.ToList();
            }




            ViewBag.AllTasks = allTasks;

            if (signInManager.IsSignedIn(User))
            {
                var userId = await userPermissions.GetCurrentUserId();
                var userHogTasks = await _context.TaskUsers.Where(x => x.UserId == userId).ToListAsync();
                ViewBag.AllUserTasks = userHogTasks;
                Int64 points = 0;
                foreach (var item in userHogTasks)
                {
                    if (item.Verdict == "OK")
                    {
                        var task = allTasks.Find(x => x.Id == item.HogTaskId);
                        points += task.Cost;
                    }
                }
                ViewBag.Points = points;
            }
            var allThemes = await _context.Themes.ToListAsync();
            /*            var allThemesNames = new List<String>();
                        foreach (var item in allThemes)
                        {
                            allThemesNames.Add(item.Name);
                        }*/
            this.ViewData["Themes"] = new SelectList(allThemes, "Id", "Name");
            return this.View();
        }

        // GET: HogTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hogTask = await _context.HogTasks
                .Include(h => h.Theme)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hogTask == null)
            {
                return NotFound();
            }

            return View(hogTask);
        }

        // GET: HogTasks/Create
        public IActionResult Create()
        {
            ViewData["ThemeId"] = new SelectList(_context.Themes, "Id", "Name");
            return View();
        }

        // POST: HogTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ThemeId,FileNameIn,FilePathIn,FileNameOut,FilePathOut,Name,Description,Cost,Postmoderation")] HogTask hogTask)
        {
            if (ModelState.IsValid)
            {
                hogTask.Id = Guid.NewGuid();
                _context.Add(hogTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "Id", "Name", hogTask.ThemeId);
            return View(hogTask);
        }

        // GET: HogTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hogTask = await _context.HogTasks.SingleOrDefaultAsync(m => m.Id == id);
            if (hogTask == null)
            {
                return NotFound();
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "Id", "Name", hogTask.ThemeId);
            return View(hogTask);
        }

        // POST: HogTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ThemeId,FileNameIn,FilePathIn,FileNameOut,FilePathOut,Name,Description,Cost,Postmoderation")] HogTask hogTask)
        {
            if (id != hogTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hogTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HogTaskExists(hogTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "Id", "Name", hogTask.ThemeId);
            return View(hogTask);
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var task = await _context.HogTasks.SingleOrDefaultAsync(m => m.Id == id);
                _context.HogTasks.Remove(task);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return this.RedirectToAction("Index", "Error", new { error = e.Message });
            }

            return this.RedirectToAction(nameof(Index));
        }

        private bool HogTaskExists(Guid id)
        {
            return _context.HogTasks.Any(e => e.Id == id);
        }
    }
}
