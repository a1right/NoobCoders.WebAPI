using NoobCoders.Domain;

namespace NoobCoders.Application.Interfaces
{
    public interface IRecordsService
    {
        public Task<List<Post>> GetPosts(CancellationToken cancellationToken = default(CancellationToken));
        public Task<Post?> GetPostDetais(long id, CancellationToken cancellationToken = default(CancellationToken));

        public Task<long> CreatePost(string text, long rubricId, CancellationToken cancellationToken = default(CancellationToken));
        public Task DeletePost(long id, CancellationToken cancellationToken = default(CancellationToken));
        public Task<List<Post>> GetPostsContainsSubstring(string text, CancellationToken cancellationToken = default(CancellationToken));
        public Task<List<Rubric>> GetRubrics(CancellationToken cancellationToken = default(CancellationToken));

        public Task<Rubric> GetRubricDetails(long id, CancellationToken cancellationToken = default(CancellationToken));
        public Task DeleteRubric(long id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
