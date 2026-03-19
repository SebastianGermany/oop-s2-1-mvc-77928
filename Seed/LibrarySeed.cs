using Library.Domain.Entities;
using Library.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.MVC.Seed
{
    public static class LibrarySeed
    {
        public static async Task SeedLibraryDataAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!context.Books.Any())
            {
                var books = new List<Book>
                {
                    new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Isbn = "9780261103344", Category = "Fantasy", IsAvailable = true },
                    new Book { Title = "Clean Code", Author = "Robert C. Martin", Isbn = "9780132350884", Category = "Programming", IsAvailable = true },
                    new Book { Title = "1984", Author = "George Orwell", Isbn = "9780451524935", Category = "Dystopian", IsAvailable = true },
                    new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Isbn = "9780201616224", Category = "Programming", IsAvailable = true },
                    new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Isbn = "9780061120084", Category = "Classic", IsAvailable = true }
                };

                context.Books.AddRange(books);
            }

            if (!context.Members.Any())
            {
                var members = new List<Member>
                {
                    new Member { FullName = "Sebastian Lopez", Email = "sebastian@example.com", Phone = "0891111111" },
                    new Member { FullName = "Maria Garcia", Email = "maria@example.com", Phone = "0892222222" },
                    new Member { FullName = "John Murphy", Email = "john@example.com", Phone = "0893333333" }
                };

                context.Members.AddRange(members);
            }

            await context.SaveChangesAsync();
        }
    }
}