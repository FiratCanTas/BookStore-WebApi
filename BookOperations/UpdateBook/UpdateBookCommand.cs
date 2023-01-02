using BookStore.DbOperations;
using System;
using System.Linq;

namespace BookStore.BookOperations.UpdateBook
{
    public class UpdateBookCommand
    {
        private readonly BookStoreDbContext _dbContext;

        public int BookId { get; set; }
        public UpdateBookModel Model { get; set; }
        public UpdateBookCommand(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);

            if (book == null)
            {
                throw new InvalidOperationException("Güncellenecek Kitap Bulunamadı!!");
            }

            book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId; //int için default değer güncelleme yapılmadıysa 0 olarak kalır
            book.Title = Model.Title != default ? Model.Title : book.Title; //default string değeri empty ya da null gelir

            _dbContext.SaveChanges();
        }
    }

    public class UpdateBookModel
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
    }
}
