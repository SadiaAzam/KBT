using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KBT.Data;
using KBT.Models;
using Microsoft.AspNetCore.Authorization;

namespace KBT.Controllers
{
    public class FruitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public IEnumerable<int> Vendors { get; private set; }

        public FruitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fruits
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewData["VendorsofFruits"] = await _context.VendorsofFruits.ToListAsync();
            ViewData["Vendors"] = await _context.Vendor.ToListAsync();
            var applicationDbContext = _context.Fruit.Include(f => f.Employees);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> FruitList()
        {
            return View(await _context.Fruit.ToListAsync());
        }
        public IActionResult SearchForm()
        {
            return View();
        }
        public async Task<IActionResult> SearchResult(string Tital)
        {
            ViewData["VendorsofFruits"] = await _context.VendorsofFruits.ToListAsync();
            ViewData["Vendors"] = await _context.Vendor.ToListAsync();
            var applicationDbContext = _context.Fruit.Include(f => f.Employees);
            return View("Index", await _context.Fruit.Where(a => a.Title.Contains(Tital)).ToListAsync());
        }
        public async Task<IActionResult> FruitDetails(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            ViewData["VendorsofFruits"] = await _context.VendorsofFruits.ToListAsync();
            ViewData["Vendors"] = await _context.Vendor.ToListAsync();
            var applicationDbContext = _context.Fruit.Include(f => f.Employees);
            return View("Index", await _context.Fruit.Where(a => a.Id == Id).ToListAsync());
        }

        // GET: Fruits/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name");
            ViewData["VendorId"] = new SelectList(_context.Vendor, "Id", "Name");
            return View();
        }

        // POST: Fruits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,URL,Price,EmployeeId")] Fruit fruit, List<int> Vendors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fruit);
                await _context.SaveChangesAsync();
                List<VendorsofFruits> vendorsofFruits = new List<VendorsofFruits>();
                foreach (int vendor in Vendors)
                {
                    vendorsofFruits.Add(new VendorsofFruits { VendorId = vendor, FruitId = fruit.Id });
                }
               _context.VendorsofFruits.AddRange(vendorsofFruits);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Id", fruit.EmployeeId);
            return View(fruit);
        }

        // GET: Fruits/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fruit = await _context.Fruit.FindAsync(id);
            if (fruit == null)
            {
                return NotFound();
            }
            IList<VendorsofFruits> vendorsofFruits = await _context.VendorsofFruits.Where<VendorsofFruits>(v => v.FruitId == fruit.Id).ToListAsync();
            IList<int> listVendors = new List<int>();
            foreach (VendorsofFruits vendorsofFruit in vendorsofFruits)
            {
              //listVendors.Add(vendorsofFruits.VendorId);
            }
            //  var authors = await _context.Vendor.Where(a=>a.Vendor_Id.Equals(listAuthors)).ToListAsync();

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", fruit.EmployeeId);
            ViewData["VendorId"] = new MultiSelectList(_context.Vendor, "Id", "Name", listVendors.ToArray());
            return View(fruit);
        }

        // POST: Fruits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,URL,Price,EmployeeId")] Fruit fruit)
        {
            var transaction = _context.Database.BeginTransaction();
            if (id != fruit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fruit);
                    await _context.SaveChangesAsync();
                    List<VendorsofFruits> vendorsofFruit = new List<VendorsofFruits>();
                    foreach (int vendor in Vendors)
                    {
                        vendorsofFruit.Add(new VendorsofFruits { VendorId = vendor, FruitId = fruit.Id });
                    }
                    List<VendorsofFruits> deleteVendorsofFruits = await _context.VendorsofFruits.Where<VendorsofFruits>(a => a.FruitId == fruit.Id).ToListAsync();
                    _context.VendorsofFruits.RemoveRange(deleteVendorsofFruits);
                    _context.SaveChanges();

                    _context.VendorsofFruits.UpdateRange(vendorsofFruit);
                    _context.SaveChanges();

                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FruitExists(fruit.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "Id", fruit.EmployeeId);
            return View(fruit);
        }

        // GET: Fruits/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fruit = await _context.Fruit
                .Include(f => f.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fruit == null)
            {
                return NotFound();
            }

            return View(fruit);
        }

        // POST: Fruits/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fruit = await _context.Fruit.FindAsync(id);
            _context.Fruit.Remove(fruit);
            await _context.SaveChangesAsync();
            List<VendorsofFruits> deleteVendorsofFruits = await _context.VendorsofFruits.Where<VendorsofFruits>(a => a.FruitId == fruit.Id).ToListAsync();
            _context.VendorsofFruits.RemoveRange(deleteVendorsofFruits);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool FruitExists(int id)
        {
            return _context.Fruit.Any(e => e.Id == id);
        }
    }
}
