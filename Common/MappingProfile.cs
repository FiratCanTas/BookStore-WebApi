using AutoMapper;
using BookStore.BookOperations.CreateBook;
using BookStore.BookOperations.GetBookDetail;
using BookStore.BookOperations.GetBooks;
using System.Collections.Generic;

namespace BookStore.Common
{
    public class MappingProfile:Profile //AutoMapper tarafından config sınıfı olarak görülmeye başlar
    {
        //Ne neye dönüşebilir bunun configlerini veriyoruz

        public MappingProfile()
        {
            CreateMap<CreateBookModel, Book>(); //İlk parametre kaynak ikinci hedef --> CreateBookModel objesi Book objesine map lenebilir olsun demiş olduk.

            CreateMap<Book, BookDetailViewModel>().ForMember(x=>x.Genre, y=> y.MapFrom(z=>((GenreEnum)z.GenreId).ToString())); //Mapleme işlemini yaparken Genre özelliğini enum olarak cast et bu formatta eşleme yap demiş olduk.

            CreateMap<Book, BooksViewModel>().ForMember(x => x.Genre, y => y.MapFrom(z => ((GenreEnum)z.GenreId).ToString()));
        }
    }
}
