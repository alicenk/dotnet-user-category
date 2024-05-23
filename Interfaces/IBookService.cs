using LibraryApi.Models;

namespace LibraryApi.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAll();
        Book GetById(int id);
        Task<List<Book>> GetAllBooksFromElasticsearchAsync();
        Task Update(int id, Book updatedBook);
        Task Delete(int id);
        Task AddAsync(Book book);
    }
}