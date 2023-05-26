using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using static System.Net.Mime.MediaTypeNames;
using Elastic.Clients.Elasticsearch.Core.Reindex;

namespace ElasticTest
{
    public class ElasticClient
    {
        private readonly string _cloudId;
        private readonly string _apiKey;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param Names="connectionString"></param>
        public ElasticClient(string cloudId, string apiKey)
        {
            _cloudId = cloudId;
            _apiKey = apiKey;
        }

        public async Task<IndexResponse> IndexDoc(object document, string indexName)
        {
            var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

            var result = await client.IndexAsync(document, indexName);

            return result;
        }

        /*public async Task<IndexResponse> IndexDoc(int id, string name, int year, int month, int days, string message, string indexName)
        {
            var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

            var tweet = new Tweet
            {
                Id = id,
                User = name,
                PostDate = new DateTime(year, month, days),
                Message = message,

            };

            var IndexName = indexName;

            var result = await client.IndexAsync(tweet, "" + IndexName);

            return result;
        }*/

        public async Task<GetResponse<TModel>> GetDoc<TModel>(string index)
        {
            var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

            var response = await client.GetAsync<TModel>(1, idx => idx.Index(index));

            return response;

        }

        public async Task<SearchResponse<Tweet>> SearchDoc(string index, string user)
        {
            var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

            var response = await client.SearchAsync<Tweet>(s => s.Index(index).From(0).Size(10).Query(q => q.Term(t => t.User, user)));

            return response;
        }

        //public async Task<UpdateResponse<Tweet>> UpdateDoc(string index, string message)
        //{
        //    var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

        //    var tweet = new Tweet
        //    {
        //        Message = "Message"
        //    };

        //    var response = await client.UpdateAsync<Tweet, Tweet>("" + index, 1, u => u
        //    .Doc(tweet));

        //    return response;
        //}

        public async Task<UpdateResponse<TResponse>> UpdateDoc<TResponse, TRequest>(string index, TRequest document)
        {
            var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

            var response = await client.UpdateAsync<TResponse, TRequest>(index, 1, u => u.Doc(document));

            return response;
        }

        public async Task<DeleteResponse> DeleteDoc(string index)
        {

            var client = new ElasticsearchClient(_cloudId, new ApiKey(_apiKey));

            var response = await client.DeleteAsync("" + index, 1);

            return response;
        }
    }
}