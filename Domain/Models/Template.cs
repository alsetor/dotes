namespace Domain.Models
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Tags { get; set; }
        public byte[] File { get; set; }
    }
}
