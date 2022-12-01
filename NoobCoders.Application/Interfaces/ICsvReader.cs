using Microsoft.Extensions.Configuration;
using NoobCoders.Application.Models;

namespace NoobCoders.Application.Interfaces
{
    public interface ICsvReader
    {
        public List<RecordTemplate> GetRecordTemplates(IConfiguration configuration);
    }
}
