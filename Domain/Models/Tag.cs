namespace Domain.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public TagType Type { get; set; }
        public List<string> CellNames { get; set; }
    }
}
