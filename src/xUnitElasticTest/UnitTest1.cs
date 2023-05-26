using Elastic.Clients.Elasticsearch;
using static System.Net.Mime.MediaTypeNames;
using ElasticTest;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Data;
using ElasticTest.Configuration;
using System.Net;
using System.Reflection;

namespace xUnitElasticTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(1, "Tim Update", 2003, 3, 3, "update-index")]
        public async Task TestIndexDocUsingObject(int Id, string User, int Year, int Month, int day, string Index)
        {
            // Arrange
            var apiKey = string.Empty;
            var cloudId = string.Empty;

            using (var configReader = new ConfigReader("configuration.json"))
            {
                await configReader.OpenFileAsync();
                configReader.TryGetValue("CloudId", out cloudId);
                configReader.TryGetValue("Key", out apiKey);
            }

            var client = new ElasticClient(cloudId, apiKey);

            //Act
            var tweet = new Tweet
            {
                Id = Id,
                User = User,
                PostDate = new DateTime(Year, Month, day),
                Message = "Trying out the client, so far so good?"
            };
            var result = await client.IndexDoc(tweet, Index);
            
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsValidResponse);
            Assert.True(result.ApiCallDetails.HttpStatusCode == (int)HttpStatusCode.Created); //for reference
            var resultDelete = await client.DeleteDoc(Index);
            Assert.NotNull(resultDelete);
            Assert.True(resultDelete.ApiCallDetails.HttpStatusCode == 200);
        }

        [Theory]
        [InlineData("index-get")]
        public async Task TestGetDoc(string Index)
        {
            //
            // Arrange
            //           
            var apiKey = string.Empty;
            var cloudId = string.Empty;
            using (var configReader = new ConfigReader("configuration.json"))
            {
                await configReader.OpenFileAsync();
                configReader.TryGetValue("CloudId", out cloudId);
                configReader.TryGetValue("Key", out apiKey);
            }

            var client = new ElasticClient(cloudId, apiKey);

            var tweet = new Tweet
            {
                Id = 1,
                User = "Tim Get",
                PostDate = new DateTime(2002, 2, 2),
                Message = "This is get index"
            };
            var result = await client.IndexDoc(tweet,Index);
            //
            // Act
            //    
            var resultget = await client.GetDoc<Tweet>(Index);
            
            //
            // Assert
            //
            Assert.NotNull(result);
            Assert.True(result.IsValidResponse);
            Assert.NotNull(resultget);
            Assert.True(resultget.Index == Index);

            var resultDelete = await client.DeleteDoc(Index);
            Assert.NotNull(resultDelete);
            Assert.True(resultDelete.ApiCallDetails.HttpStatusCode == 200);
        }

        [Theory]
        [InlineData("index-search","Tim Search")]
        public async Task TestSearchDoc(string index,string user)
        {
            //
            // Arrange
            //           
            var apiKey = string.Empty;
            var cloudId = string.Empty;
            using (var configReader = new ConfigReader("configuration.json"))
            {
                await configReader.OpenFileAsync();
                configReader.TryGetValue("CloudId", out cloudId);
                configReader.TryGetValue("Key", out apiKey);
            }

            var client = new ElasticClient(cloudId, apiKey);

            var tweet = new Tweet
            {
                Id = 1,
                User = "Tim search",
                PostDate = new DateTime(2002, 2, 2),
                Message = "This is get index"
            };
            var result = await client.IndexDoc(tweet, index);
            //
            // Act
            //    
            var resultsearch = await client.SearchDoc(index,user);

            //
            // Assert
            //
            Assert.NotNull(result);
            Assert.True(result.IsValidResponse);
            Assert.NotNull(resultsearch);
            
            var resultDelete = await client.DeleteDoc(index);
            Assert.NotNull(resultDelete);
            Assert.True(resultDelete.ApiCallDetails.HttpStatusCode == 200);
        }

        [Theory]
        [InlineData(1,"Tim Update",2003,3,3,"update-index")]
        public async Task TestUpdateDocUsingObject(int Id ,string User,int year,int Month,int day, string index)
        {
            //
            // Arrange
            //
            var apiKey = string.Empty;
            var cloudId = string.Empty;
            using (var configReader = new ConfigReader("configuration.json"))
            {
                await configReader.OpenFileAsync();
                configReader.TryGetValue("CloudId", out cloudId);
                configReader.TryGetValue("Key", out apiKey);
            }

            var client = new ElasticClient(cloudId, apiKey);

            var tweet = new Tweet
            {
                Id = Id,
                User = User,
                PostDate = new DateTime(year, Month, day),
                Message = "This is update index"
            };
            var result = await client.IndexDoc(tweet, index);

            //
            // Act
            //
            var updatingTweet = new Tweet
            {
                Message = "This is an updated message"
            };
            var resultupdate = await client.UpdateDoc<Tweet, Tweet>(index, updatingTweet);
            var resultget = await client.GetDoc<Tweet>(index);
            //
            // Assert
            //
            Assert.NotNull(result);
            Assert.NotNull(resultupdate);

            var resultDelete = await client.DeleteDoc(index);
            Assert.NotNull(resultDelete);
            Assert.True(resultDelete.ApiCallDetails.HttpStatusCode == 200);
        }

        [Theory]
        [InlineData("delete-index")]
        public async Task TestDeleteDoc(string index)
        {
            //Arrange
            var apiKey = string.Empty;
            var cloudId = string.Empty;
            using (var configReader = new ConfigReader("configuration.json"))
            {
                await configReader.OpenFileAsync();
                configReader.TryGetValue("CloudId", out cloudId);
                configReader.TryGetValue("Key", out apiKey);
            }

            var client = new ElasticClient(cloudId, apiKey);

            var tweet = new Tweet
            {
                Id = 1,
                User = "Tim Delete",
                PostDate = new DateTime(2003, 3, 3),
                Message = "This is Delete index"
            };

            var result = await client.IndexDoc(tweet, index);            
            //Act
            var resultDelete = await client.DeleteDoc(index);
            //Assert
            Assert.NotNull(result);
            Assert.True(resultDelete.ApiCallDetails.HttpStatusCode == 200);

        }
    }
}