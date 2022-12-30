using BookStore.DbOperations;
using System;
using System.Linq;

namespace BookStore.BookOperations
{
    public class CreateBookCommand
    {
        public  CreateBookModel Model { get; set; }

        private readonly BookStoreDbContext _dbContext;
        public CreateBookCommand(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var book = _dbContext.Books.FirstOrDefault(x => x.Title == Model.Title);

            if (book is not null)
                throw new InvalidOperationException("Kitap zaten mevcut");

            book = new Book();
            book.Title = Model.Title;
            book.PublishedDate = Model.PublishedDate;
            book.GenreId = Model.GenreId;
            book.PageCount= Model.PageCount;

            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
           
        }
    }

    public class CreateBookModel
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
