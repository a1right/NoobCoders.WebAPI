

using CsvHelper.Configuration.Attributes;

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
