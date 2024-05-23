using LibraryApi.Models;

namespace LibraryApi.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAll();
        Book GetById(int id);
        void Add(Book book);
        void Update(int id, Book updatedBook);
        void Delete(int id);
    }
}