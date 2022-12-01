

using CsvHelper.Configuration.Attributes;
using NoobCoders.Application.Interfaces;

namespace NoobCoders.Application.Models
{
    public class RecordTemplate
    {
        [Name("text")]
        public string Text { get; set; }
        [Name("created_date")]
        public string CreatedDate { get; set; }
        [Name("rubrics")]
        public string RubricsString { get; set; }
        public string[] Rubrics => RubricsString.Split(new[] { '"', '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
