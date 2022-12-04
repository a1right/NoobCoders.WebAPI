using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoobCoders.Repository;

namespace NoobCoders.Tests.Common
{
    public abstract class TestRecordsBase : IDisposable
    {
        protected readonly PostsDbContext _context;

        public TestRecordsBase()
        {
            _context = PostsContextFactory.Create();
        }

        public void Dispose()
        {
            PostsContextFactory.Destroy(_context);
        }
    }
}
