using CsvHelper.Configuration.Attributes;

namespace NoobCoders.WebAPI.Data.Mapping
{
    public class RecordTemplate
    {
        [Name("text")]
        public string Text { get; set; }
        [Name("created_date")]
        public string CreatedDate { get; set; }
        [Name("rubrics")]
        public string Rubrics { get; set; }
    }
}
