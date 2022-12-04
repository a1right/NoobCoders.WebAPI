using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NoobCoders.Tests.Common;
using Xunit;

namespace NoobCoders.Tests.RecordsService
{
    public class DeletePostTest : TestRecordsBase
    {
        [Fact]
        public async Task DeletePost_Success()
        {
            //Arrange
            var recordsService = new Application.Services.RecordsService(_context, null);
            

            //Act
            await recordsService.DeletePost(PostsContextFactory.postIdForDelete, CancellationToken.None);

            //Assert

            Assert.Null(
                _context.Posts.SingleOrDefault(post => post.Id == PostsContextFactory.postIdForDelete)
                );
        }
    }
}
