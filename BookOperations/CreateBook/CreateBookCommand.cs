using AutoMapper;
using BookStore.DbOperations;
using System;
using System.Linq;

namespace BookStore.BookOperations.CreateBook
{
    public class CreateBookCommand
    {
        public CreateBookModel Model { get; set; }

        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public CreateBookCommand(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Handle()
        {
            var book = _dbContext.Books.FirstOrDefault(x => x.Title == Model.Title);

            if (book is not null)
                throw new InvalidOperationException("Kitap zaten mevcut");

            //CreateBookModel objesi Book objesine map lenebilir dönüştürülebilir dicez --> mapping classına bak

            book = _mapper.Map<Book>(Model);//new Book();      //Model(CreateBookModel) ile gelen veriyi book objesine convert et demiş olduk.
            //book.Title = Model.Title;
            //book.PublishedDate = Model.PublishedDate;
            //book.GenreId = Model.GenreId;
            //book.PageCount= Model.PageCount;

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
