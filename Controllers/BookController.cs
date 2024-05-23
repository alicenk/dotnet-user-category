using Microsoft.AspNetCore.Mvc;
using LibraryApi.Models;
using LibraryApi.Services;
using LibraryApi.Interfaces;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/Book")]
    public class BookController: ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get() => _bookService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            var book = _bookService.GetById(id);
            if (book == null) return NotFound();
            return book;
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            _bookService.Add(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book book)
        {
            if (id != book.Id) return BadRequest();
            var existingBook = _bookService.GetById(id);
            if (existingBook == null) return NotFound();
            _bookService.Update(id, book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookService.GetById(id);
            if (book == null) return NotFound();
            _bookService.Delete(id);
            return NoContent();
        }
    }
}