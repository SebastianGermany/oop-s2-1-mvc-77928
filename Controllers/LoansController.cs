using Library.Domain.Entities;
using Library.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var loans = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .OrderByDescending(l => l.LoanDate);

            return View(await loans.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null) return NotFound();

            return View(loan);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropDowns();
            return View(new Loan
            {
                LoanDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            var book = await _context.Books.FindAsync(loan.BookId);

            if (book == null)
            {
                ModelState.AddModelError("BookId", "Selected book does not exist.");
            }
            else if (!book.IsAvailable)
            {
                ModelState.AddModelError("BookId", "This book is already on loan.");
            }

            if (loan.DueDate < loan.LoanDate)
            {
                ModelState.AddModelError("DueDate", "Due date cannot be before loan date.");
            }

            if (ModelState.IsValid)
            {
                book!.IsAvailable = false;
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadDropDowns();
            return View(loan);
        }

        public async Task<IActionResult> Return(int? id)
        {
            if (id == null) return NotFound();

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null) return NotFound();

            return View(loan);
        }

        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null) return NotFound();

            if (loan.ReturnedDate == null)
            {
                loan.ReturnedDate = DateTime.Today;
                if (loan.Book != null)
                {
                    loan.Book.IsAvailable = true;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropDowns()
        {
            ViewData["BookId"] = new SelectList(
                await _context.Books.Where(b => b.IsAvailable).ToListAsync(),
                "Id",
                "Title");

            ViewData["MemberId"] = new SelectList(
                await _context.Members.OrderBy(m => m.FullName).ToListAsync(),
                "Id",
                "FullName");
        }
    }
}