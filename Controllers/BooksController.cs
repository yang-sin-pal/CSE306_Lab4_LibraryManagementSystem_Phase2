using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BooksController : ControllerBase
    {
        //static List<Book> Books = new List<Book>
        //{
        //    new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925, Genre = "Novel" },
        //    new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960, Genre = "Novel" },
        //    new Book { Id = 3, Title = "1984", Author = "George Orwell", Year = 1949, Genre = "Dystopian" }
        //};

        //[HttpGet]
        //public ActionResult<List<Book>> GetAllBooks()
        //{
        //    return Books;
        //}

        //[HttpGet("{id}")]
        //public ActionResult<Book> tBookById(int id)
        //{ 
        //    var Book = Books.FirstOrDefault(x => x.Id == id);

        //    if (Book != null)
        //    {
        //        return Book;
        //    }
        //    else
        //    {
        //        return NotFound("Book not found");
        //    }
        //}

        //[HttpPost]
        //public ActionResult<Book> AddBook([FromBody]Book newBook)
        //{
        //    if (newBook.Id != Books.Max(x => x.Id + 1))
        //    {
        //        return BadRequest("The id of this new book must larger than the max id in the list 1 unit. " +
        //                            "Example, if the largest id is 3 then the id of this new book is 4 (3 + 1)." +
        //                            " Use GET /api/Books to know the lagrest id.");   
        //    }
        //    else
        //    {
        //        Books.Add(newBook);
        //        return newBook;
        //    }


        //}

        //[HttpPut("{id}")]
        //public ActionResult<Book> UpdateBook(int id, Book updateBook)
        //{
        //    var needUpdateBook = Books.FirstOrDefault(x => x.Id == id);

        //    if (needUpdateBook == null)
        //    {
        //        return NotFound("Book not found");
        //    }

        //    needUpdateBook.Author = updateBook.Author;
        //    needUpdateBook.Title = updateBook.Title;
        //    needUpdateBook.Year = updateBook.Year;
        //    needUpdateBook.Genre = updateBook.Genre;

        //    return needUpdateBook;
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteBook(int id)
        //{
        //    var wantedBook = Books.FirstOrDefault(x => x.Id == id);

        //    if (wantedBook == null)
        //    {
        //        return NotFound("Book not found");
        //    }

        //    Books.Remove(wantedBook);

        //    return NoContent();
        //}

    }
}
