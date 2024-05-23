using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Interfaces;

namespace LibraryApi.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _context;
        private readonly ElasticsearchService _elasticsearchService;

        public BookService(
            LibraryDbContext context, 
            ElasticsearchService elasticsearchService
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _elasticsearchService = elasticsearchService ?? throw new ArgumentNullException(nameof(elasticsearchService));
        }

        public List<Book> GetAll() {
            List<Book> books = [.. _context.Books];
            return books;
        }

        public Book GetById(int id) {
            return _context.Books.FirstOrDefault(i => i.Id == id);
        }

        public async Task AddAsync(Book book)
        {
           // Veritabanına yeni bir kitap ekleyin
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            await _elasticsearchService.IndexBookAsync(book);
        }

        public async Task<List<Book>> GetAllBooksFromElasticsearchAsync(){
            var books = await _elasticsearchService.GetAllDocumentsAsync<Book>("book");
            return books;
        }

        public async Task Update(int id, Book updatedBook)
        {
            var existingBook = _context.Books.Find(id) ?? throw new ArgumentException("Book not found");

            // Kitabın özelliklerini güncelleyin
            existingBook.Name = updatedBook.Name;
            existingBook.Description = updatedBook.Description;

             // Değişiklikleri kaydedin
            await _context.SaveChangesAsync();
            await _elasticsearchService.IndexBookAsync(existingBook);
        }

        public async Task Delete(int id)
        {
            // Silinecek kitabı bulun
            var bookToDelete = _context.Books.Find(id) ?? throw new ArgumentException("Book not found");

            // Kitabı veritabanından silin
            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }
    }
}