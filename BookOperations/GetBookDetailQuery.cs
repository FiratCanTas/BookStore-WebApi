using BookStore.Common;
using BookStore.DbOperations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;

namespace BookStore.BookOperations
{
    public class GetBookDetailQuery
    {

        private readonly BookStoreDbContext _dbContext;

        public int BookId { get; set; }
        public GetBookDetailQuery(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BookDetailViewModel Handle()
        {
            var book = _dbContext.Books.Where(book => book.Id == BookId).SingleOrDefault();
            if (book == null)
            {
                throw new InvalidOperationException("Kitap Bulunamadı!!");
            }
            
            BookDetailViewModel vm = new BookDetailViewModel();
            vm.Title = book.Title;
            vm.PublishedDate = book.PublishedDate.Date.ToString("dd-mm-yyyy");
            vm.PageCount= book.PageCount;
            vm.Genre = ((GenreEnum)book.GenreId).ToString();
            return vm;
        }
    }

    public class BookDetailViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public string PublishedDate { get; set; }
    }
}
