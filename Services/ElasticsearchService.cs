using LibraryApi.Models;
using Nest;

namespace LibraryApi.Services
{
    public class ElasticsearchService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task IndexBookAsync(Book book)
        {
            var indexName = "book"; // Elasticsearch'te kullanacağınız index adı

            var response = await _elasticClient.IndexAsync(book, idx => idx.Index(indexName));
            if (!response.IsValid)
            {
                throw new Exception("Failed to index document to Elasticsearch", response.OriginalException);
            }
        }

        public async Task<List<T>> GetAllDocumentsAsync<T>(string indexName) where T : class
        {
            var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .MatchAll()
            );

            if (!response.IsValid)
            {
                throw new Exception("Failed to retrieve documents from Elasticsearch", response.OriginalException);
            }

            return [.. response.Documents];
        }

    }
}