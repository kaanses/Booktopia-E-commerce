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
            return BadRequest("Kayıtlı kitap bulunmamaktadır.");
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
            return NotFound("Girdiğiniz başlıkta bir kitap bulunamamaktadır");
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
            return Ok("Kitap eklendi");
        }
        [HttpPut ("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Update(int id, string title, string author, string imageUrl, decimal price, int publicationYear)
        {
            if (id <= 0)
            {
                return BadRequest("Girdiğiniz id 0'dan büyük olmalıdır.");
            }
            var book = _books.Find(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Girdiğiniz ID'ye sahip bir kitap bulunamamaktadır");
            }
            book.Title = FormatName(title);
            book.Author = FormatName(author);
            book.ImageUrl = imageUrl;
            book.Price = price;
            book.PublicationYear = publicationYear;
            return Ok("Kitap başarıyla güncellendi");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Girdiğiniz id 0'dan büyük olmalıdır.");
            }
            var book = _books.Find(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Girdiğiniz ID'ye sahip bir kitap bulunamamaktadır");
            }
            _books.Remove(book);
            return Ok("Kitap başarıyla silindi");
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
