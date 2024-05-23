using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Interfaces;


namespace LibraryApi.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Book> GetAll() {
            List<Book> books = [.. _context.Books];
            return books;
        }

        public Book GetById(int id) {
            return _context.Books.FirstOrDefault(i => i.Id == id);
        }

        public void Add(Book book)
        {
           // Veritabanına yeni bir kitap ekleyin
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(int id, Book updatedBook)
        {
            var existingBook = _context.Books.Find(id) ?? throw new ArgumentException("Book not found");

            // Kitabın özelliklerini güncelleyin
            existingBook.Name = updatedBook.Name;
            existingBook.Description = updatedBook.Description;

             // Değişiklikleri kaydedin
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            // Silinecek kitabı bulun
            var bookToDelete = _context.Books.Find(id) ?? throw new ArgumentException("Book not found");

            // Kitabı veritabanından silin
            _context.Books.Remove(bookToDelete);
            _context.SaveChanges();
        }
    }
}