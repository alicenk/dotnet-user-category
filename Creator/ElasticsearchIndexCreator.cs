using Nest;
using LibraryApi.Models;

namespace LibraryApi.Creator
{
    public class ElasticsearchIndexCreator
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchIndexCreator(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void CreateIndices()
        {
            CreateBookIndex();
            // Diğer indexlerin oluşturulması...
        }

        private void CreateBookIndex()
        {
            var indexName = "book";

            // Eğer index varsa sil
            if (_elasticClient.Indices.Exists(indexName).Exists)
            {
                var response = _elasticClient.Indices.Delete(indexName);
                if (!response.IsValid)
                {
                    throw new Exception($"Failed to delete existing '{indexName}' index in Elasticsearch: {response.ServerError}");
                }
            }

            // Yeni index oluştur
            var createIndexResponse = _elasticClient.Indices.Create(indexName, c => c
                .Map<Book>(m => m
                    // Mapping ayarlarını buraya ekleyin
                )
            );

            if (!createIndexResponse.IsValid)
            {
                throw new Exception($"Failed to create '{indexName}' index in Elasticsearch: {createIndexResponse.ServerError}");
            }
        }
    }
}