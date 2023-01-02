using AutoMapper;
using BookStore.BookOperations.CreateBook;
using BookStore.BookOperations.DeleteBook;
using BookStore.BookOperations.GetBookDetail;
using BookStore.BookOperations.GetBooks;
using BookStore.BookOperations.UpdateBook;
using BookStore.DbOperations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
                validator.ValidateAndThrow(query);
               
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
                //Model maplendikten sonra validasyonu yapmamız lazım
                CreateBookCommandValidator validator = new CreateBookCommandValidator();
                /* ValidationResult result  = validator.Validate(command);*/ //--> sonuçları görmek istediğimiz için değişkene attık

                //if (!result.IsValid) //bütün kurallardan true geldiyse is valid döner
                //{
                //    foreach (var item in result.Errors)
                //    {
                //        Console.WriteLine($"Özellik : {item.PropertyName} - Hata Mesajı : {item.ErrorMessage}");
                //    }
                //}

                validator.ValidateAndThrow(command); //Hatayı yakala ve fırlat, fırlatılan bu hata catchteki exception tarafından yakalanıp yazdırılacak

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
                UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
                validator.ValidateAndThrow(command);
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
                DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
                validator.ValidateAndThrow(command);

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
