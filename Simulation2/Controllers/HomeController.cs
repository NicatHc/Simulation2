using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation2.DAL;
using Simulation2.Models;

namespace Simulation2.Controllers
{
    public class HomeController : Controller
    {

        public readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Worker> workers = await _context.workers.ToListAsync();
            return View(workers);
        }
    }
}
