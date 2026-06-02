using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Simulation2.DAL;
using Simulation2.Models;
using Simulation2.Utilities.Enums;
using Simulation2.Utilities.Extentions;
using Simulation2.ViewModels.Worker;
using static System.Net.Mime.MediaTypeNames;

namespace Simulation2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class WorkerController : Controller
    {

        public readonly AppDbContext _context;
        public readonly IWebHostEnvironment _env;

        public WorkerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env= env;

        }

        public async Task<IActionResult> Index()
        {
            List<Worker> workers = await _context.workers.ToListAsync();
            return View(workers);
        }




        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateVM createVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if(!createVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Type ERROR");
                return View();
            }

            if (!createVM.Photo.ValidateSize(3, FileSize.MB))
            {
                ModelState.AddModelError("Photo", "Size ERROR");
                return View();
            }

            Worker worker = new Worker
            {
                Name=createVM.Name,
                Job=createVM.Job,
                Image=await createVM.Photo.CreateFile(_env.WebRootPath, "assets", "images"),
            };
            await _context.workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Worker worker = _context.workers.FirstOrDefault(w => w.Id == id);

            if (worker == null) return NotFound();
            return View(worker);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Worker worker = _context.workers.FirstOrDefault(w => w.Id == id);

            if (worker == null) return NotFound();
            worker.Image.DeleteFile(_env.WebRootPath, "assets", "images");
            _context.workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Worker worker = _context.workers.FirstOrDefault(w => w.Id == id);

            if (worker == null) return NotFound();

            UpdateVM updateVM = new UpdateVM
            {
                Name= worker.Name,
                Job= worker.Job,
                Image= worker.Image,
            };
            return View(updateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateVM updateVM)
        {
            if (id == null || id < 1) return BadRequest();

            Worker worker = _context.workers.FirstOrDefault(w => w.Id == id);

            if (worker == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }


            if(updateVM.Photo !=null)
            {
                if (!updateVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "Type ERROR");
                    return View();
                }

                if (!updateVM.Photo.ValidateSize(3, FileSize.MB))
                {
                    ModelState.AddModelError("Photo", "Size ERROR");
                    return View();
                }

                worker.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                worker.Image = await updateVM.Photo.CreateFile(_env.WebRootPath, "assets", "images");
            }
            worker.Name = updateVM.Name;
            worker.Job = updateVM.Job;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
