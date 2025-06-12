namespace Core.DTOs.Filter
{
    public class FilterDTO
    {
        public List<string>? Size { get; set; }
        public List<string>? Material { get; set; }
        public List<string>? Color { get; set; }
        public List<string>? Season { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
        public string? Language { get; set; }
        public string? MainCategory { get;set; }
        public List<string>? subName { get; set; }
        public List<string>? urlName { get;set ; }
    }
}