using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        static private readonly List<book> _books = new List<book>();
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
        public IActionResult Add (string title, string author, string imageUrl, decimal price, int publicationYear)
        {
            if (string.IsNullOrEmpty(title.Trim()) || 
                string.IsNullOrEmpty(author.Trim())
                || string.IsNullOrEmpty(imageUrl.Trim()) 
                || price <= 0 || publicationYear <= 0)
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
                PublicationYear = publicationYear
            });
            return Ok("the book successfully added.");
        }
        [HttpPut ("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Update(int id, string title, string author, string imageUrl, decimal price, int publicationYear)
        {
            if (id <= 0)
            {
                return BadRequest("id must be greater than zero");
            }
            var book = _books.Find(b => b.Id == id);
            if (book == null)
            {
                return NotFound("there is no such book with given id.");
            }
            book.Title = FormatName(title);
            book.Author = FormatName(author);
            book.ImageUrl = imageUrl;
            book.Price = price;
            book.PublicationYear = publicationYear;
            return Ok("the book successfully updated.");
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
