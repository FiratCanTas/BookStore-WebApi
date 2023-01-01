using AutoMapper;
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

        private readonly IMapper _mapper;
        public BookController(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;  //ctor da inject edilen instance ı atadım
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_dbContext,_mapper);
            var result = query.Handle();
            return Ok(result);
        }

        [HttpGet("{id}")] //route-path den id değerini aldık ve Query den değer almaya göre daha dorğu bir yaklaşımdır.

        public IActionResult GetBookById(int id)
        {
            BookDetailViewModel result;
            try
            {
                GetBookDetailQuery query = new GetBookDetailQuery(_dbContext,_mapper);
                query.BookId = id;
                result = query.Handle();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
      
            return Ok(result);
          
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
            CreateBookCommand command = new CreateBookCommand(_dbContext,_mapper);

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

        public IActionResult UpdateBook(int id, [FromBody] UpdateBookModel updatedBook) //Body den kitap bilgileri girilecek  ve route dan id ile güncellenecek kitap bilgisi geliyor olacak
        {
            try
            {
                UpdateBookCommand command = new UpdateBookCommand(_dbContext);
                command.BookId = id;
                command.Model = updatedBook;
                command.Handle();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
          
            _dbContext.SaveChanges();

            return Ok();

            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                DeleteBookCommand command = new DeleteBookCommand(_dbContext);
                command.BookId = id;
                command.Handle();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
         
            return Ok();

        }
        
    }
}
