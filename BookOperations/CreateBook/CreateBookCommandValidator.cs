using FluentValidation;
using System;

namespace BookStore.BookOperations.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand> //Create Book Command sınıfı objelerini valide eder 
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Model.GenreId).GreaterThan(0); //GenreId 0 dan büyük olmalı
            RuleFor(x=>x.Model.PageCount).GreaterThan(0);
            RuleFor(x=>x.Model.PublishedDate.Date).NotEmpty().LessThan(DateTime.Now.Date); //.Date saati kırparak aldığımız tarih
            RuleFor(x=>x.Model.Title).NotEmpty().MinimumLength(4);
        }
    }
}
