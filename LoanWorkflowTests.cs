using Library.Domain.Entities;
using Library.MVC.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Library.Tests
{
    public class LoanWorkflowTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreatingLoan_ShouldMakeBookUnavailable()
        {
            using var context = GetDbContext();

            var book = new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Isbn = "9780261103344",
                Category = "Fantasy",
                IsAvailable = true
            };

            var member = new Member
            {
                FullName = "Maria Garcia",
                Email = "maria@example.com",
                Phone = "0892222222"
            };

            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                LoanDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14)
            };

            book.IsAvailable = false;
            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            Assert.False(book.IsAvailable);
            Assert.Single(context.Loans);
        }

        [Fact]
        public async Task ReturningLoan_ShouldMakeBookAvailable()
        {
            using var context = GetDbContext();

            var book = new Book
            {
                Title = "1984",
                Author = "George Orwell",
                Isbn = "9780451524935",
                Category = "Dystopian",
                IsAvailable = false
            };

            var member = new Member
            {
                FullName = "John Murphy",
                Email = "john@example.com",
                Phone = "0893333333"
            };

            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                LoanDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                ReturnedDate = null
            };

            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            loan.ReturnedDate = DateTime.Today;
            book.IsAvailable = true;
            await context.SaveChangesAsync();

            Assert.True(book.IsAvailable);
            Assert.NotNull(loan.ReturnedDate);
        }
    }
}
