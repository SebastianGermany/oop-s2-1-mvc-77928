using Library.Domain.Entities;
using Xunit;

namespace Library.Tests
{
    public class BookTests
    {
        [Fact]
        public void NewBook_ShouldBeAvailable_WhenCreated()
        {
            var book = new Book
            {
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Isbn = "9780132350884",
                Category = "Programming"
            };

            Assert.True(book.IsAvailable);
        }

        [Fact]
        public void Book_ShouldStoreValuesCorrectly()
        {
            var book = new Book
            {
                Title = "1984",
                Author = "George Orwell",
                Isbn = "9780451524935",
                Category = "Dystopian",
                IsAvailable = true
            };

            Assert.Equal("1984", book.Title);
            Assert.Equal("George Orwell", book.Author);
            Assert.Equal("9780451524935", book.Isbn);
            Assert.Equal("Dystopian", book.Category);
            Assert.True(book.IsAvailable);
        }
    }
}