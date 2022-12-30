using BookStore.BookOperations;
using BookStore.DbOperations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BookStore.Controllers
{
    [ApiController] //Api a Http Response döneceğini belirttim 
    [Route("[controller]s")] // Hangi end pointte buraya nasıl erişileceğini belirtecek bir Route ekledim
    public class BookController : Controller
    {

        private readonly BookStoreDbContext _dbContext; //readonly yaptım uygulama içerisinden değiştirilemesin sadece Ctor içinde set edilsin diye. private sadece burada kullanacağım için.

        public BookController(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;  //ctor da inject edilen instance ı atadım
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_dbContext);
            var result = query.Handle();
            return Ok(result);
        }

        [HttpGet("{id}")] //route-path den id değerini aldık ve Query den değer almaya göre daha dorğu bir yaklaşımdır.

        public Book GetBookById(int id)
        {
            var book = _dbContext.Books.Where(x=>x.Id == id).FirstOrDefault();

            return book;
        }

        //[HttpGet] //Bir source yani controller içerisinde iki tane aynı şekilde Get işlemi olursa Conflict hatası yer dikkat et!!

        //public Book GetById([FromQuery]int id) //id değerini query den alıyoruz.
        //{
        //    var book = _bookList.Where(x => x.Id == id).FirstOrDefault();

        //    return book;
        //}

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook) //post ta dönüş tipi void değil IActionResult yaptım cunku metot içinde validasyon ile hata veya ok mesajı döndürdüm. 
        {
            CreateBookCommand command = new CreateBookCommand(_dbContext);

            try
            {
                command.Model = newBook;
                command.Handle();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
         
            return Ok();  
        }

        [HttpPut("{id}")]

        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook) //Body den kitap bilgileri girilecek  ve route dan id ile güncellenecek kitap bilgisi geliyor olacak
        {
            var book = _dbContext.Books.FirstOrDefault(x=> x.Id == id);

            if (book == null)
                return BadRequest();

            book.GenreId= updatedBook.GenreId != default ? updatedBook.GenreId : book.GenreId; //int için default değer güncelleme yapılmadıysa 0 olarak kalır
            book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
            book.PublishedDate = updatedBook.PublishedDate != default ? updatedBook.PublishedDate : book.PublishedDate;
            book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title; //default string değeri empty ya da null gelir

            _dbContext.SaveChanges();
            return Ok();

            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);

            if (book is null)
                return BadRequest();

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
            return Ok();

        }
        
    }
}
