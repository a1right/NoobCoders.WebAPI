using Moq;
using Nest;
using NoobCoders.Domain;

namespace NoobCoders.Tests.Common
{
    public class MockElasticClientFactory
    {
        public static Mock<IElasticClient> Create()
        {
            var posts = Enumerable.Range(1, 10).Select(i => new Post() {Id = i}).ToList();
            var mockSearchResponse = new Mock<ISearchResponse<Post>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(posts);

            var mockElasticClient = new Mock<IElasticClient>();
            mockElasticClient.Setup(client => client
                    .Search(It.IsAny<Func<SearchDescriptor<Post>, ISearchRequest>>()))
                .Returns(mockSearchResponse.Object);
            return mockElasticClient;
            //var result = mockElasticClient.Object.Search<Post>(s => s);
        }
    }
}
