using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BookController : ControllerBase
    {
        static private readonly List<book> _books = new List<book>(){
            new book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", PublicationYear
 = 1925, Price = 8, ImageUrl = "https://m.media-amazon.com/images/I/71ptoyWH8dL._AC_UF1000,1000_QL80_.jpg" },
            new book { Id = 2, Title = "1984", Author = "George Orwell", PublicationYear
 = 1949, Price = 9, ImageUrl = "https://target.scene7.com/is/image/Target/GUEST_4b5087d0-2ca3-4e48-a041-31d29bf8979e?wid=488&hei=488&fmt=pjpeg" },
            new book { Id = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", PublicationYear
 = 1960, Price = 12, ImageUrl = "https://m.media-amazon.com/images/I/81r81MTfTuL._AC_UF1000,1000_QL80_.jpg" },
            new book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", PublicationYear
 = 1813, Price = 15, ImageUrl = "https://m.media-amazon.com/images/I/61vrbLFc8oL._AC_UF1000,1000_QL80_.jpg" },
            new book { Id = 5, Title = "Moby-Dick", Author = "Herman Melville", PublicationYear
 = 1851, Price = 7, ImageUrl = "https://img.kitapyurdu.com/v1/getImage/fn:11303181/wh:true/wi:800" },
            new book { Id = 6, Title = "War and Peace", Author = "Leo Tolstoy", PublicationYear
 = 1869, Price = 19, ImageUrl = "https://m.media-amazon.com/images/I/71wXZB-VtBL._AC_UF1000,1000_QL80_.jpg" },
            new book { Id = 7, Title = "The Odyssey", Author = "Homer", PublicationYear
 = -800, Price = 9, ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSLNwCYnwlt3sqyFaPrFYvnp2pB7MvCTkI2rQ&s" },
            new book { Id = 8, Title = "The Catcher in the Rye", Author = "J.D. Salinger", PublicationYear
 = 1951, Price = 13, ImageUrl = "https://m.media-amazon.com/images/I/614LavjvoLL._AC_UF1000,1000_QL80_.jpg" },
            new book { Id = 9, Title = "Brave New World", Author = "Aldous Huxley", PublicationYear
 = 1932, Price = 18, ImageUrl = "https://m.media-amazon.com/images/I/91D4YvdC0dL._AC_UF1000,1000_QL80_.jpg" }
        };
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<book>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Get()
        {
            if (_books.Count > 0)
            {
                return Ok(_books);
            }
            return BadRequest("there is no book to show.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<book>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Get(int id)
        {
            var book = _books.Find(b => b.Id == id);
            if (book!= null)
            {
                return Ok(book);
            }
            return NotFound("there is no such book with given id.");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Add (string title, string author, string imageUrl, decimal price, int PublicationYear)
        {
            if (string.IsNullOrEmpty(title.Trim()) || 
                string.IsNullOrEmpty(author.Trim())
                || string.IsNullOrEmpty(imageUrl.Trim()) 
                || price <= 0 || PublicationYear <= 0)
            {
                return BadRequest();
            }
            
            title = FormatName(title);
            author = FormatName(author);
            _books.Add(new book()
            {
                Id = _books.Count + 1,  
                Title = title,
                Author = author,
                ImageUrl = imageUrl,
                Price = price,
                PublicationYear = PublicationYear
            });
            return Ok("the book successfully added.");
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        public IActionResult Update(int id, [FromQuery] book updatedBook)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be greater than zero.");
            }

            var book = _books.Find(b => b.Id == id);
            if (book == null)
            {
                return NotFound("There is no such book with the given ID.");
            }

            // Update the book details
            book.Title = FormatName(updatedBook.Title);
            book.Author = FormatName(updatedBook.Author);
            book.ImageUrl = updatedBook.ImageUrl;
            book.Price = updatedBook.Price;
            book.PublicationYear = updatedBook.PublicationYear;

            // Return a success response with the updated book details
            return Ok(new 
            { 
                message = "The book was successfully updated.", 
                updatedBook = book 
            });
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("id must be greater than 0.");
            }
            var book = _books.Find(b => b.Id == id);
            if (book == null)
            {
                return NotFound("there is no such book with given id.");
            }
            _books.Remove(book);
            return Ok("The book successfully deleted.");
        }
        private string FormatName(string name)
        {
            string[] words = name.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Join(" ", words);
        }
    }
}
