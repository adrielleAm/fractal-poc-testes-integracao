using System;
using Xunit;
using Flurl;
using Flurl.Http;
using System.Net.Http;
using System.Collections.Generic;

namespace poc_integration_tests
{
    public class IntegrationTests
    {
        [Fact]
        public async void CompositionTestAsync()
        {
            HttpResponseMessage image = await "https://aws.random.cat/meow".GetAsync();
            HttpResponseMessage label = await "http://yerkee.com/api/fortune".GetAsync();

            Assert.True(image.IsSuccessStatusCode);
            Assert.True(label.IsSuccessStatusCode);
        }

        [Fact]
        public async void ValidationTestAsync()
        {
            dynamic result = await "https://openlibrary.org/api"
                .AppendPathSegment("books")
                .SetQueryParams(new { 
                    bibkeys = "ISBN:0684153637", 
                    jscmd = "data",
                    format = "json"
                })
                .GetJsonAsync();
            var book = ((IDictionary<String, dynamic>)result)["ISBN:0684153637"];

            var title = book.title;
            var author = book.authors[0].name;

            Assert.Equal(title, "The old man and the sea");
            Assert.Equal(author, "Ernest Hemingway");
        }
    }
}
