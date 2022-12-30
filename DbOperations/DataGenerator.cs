using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BookStore.DbOperations
{
    public class DataGenerator
    {
        //Burada aldığımız servis provider aracılığıyla program cs içeirisne bağlayıp uygulama ayağa kalktıgında hep çalışacak bir yapı kuruyorum servis provider kullanarak.
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any()) //Uygulama ayağa kalktıgında Db de data varsa ekleme yapmasına gerek yok
                {
                    return; //Veri çoktan seed lenmiş 
                }

                context.Books.AddRange   //Burada AddRange ile book tipinde bir dizi gönderebiliriz.
                (         
                new Book() { Title = "Lean Startup", GenreId = 1 /*Personal Growth*/, PageCount = 200, PublishedDate = new DateTime(2001, 06, 12) },
                new Book() { Title = "Herland", GenreId = 2 /*Science Fiction*/, PageCount = 250, PublishedDate = new DateTime(2010, 05, 23) },
                new Book() { Title = "Dune", GenreId = 2 /*Science Fiction*/, PageCount = 540, PublishedDate = new DateTime(2001, 12, 21) }
                );

                context.SaveChanges();
            }
        }
    }
}
