namespace aatest.Models
{
    public class Car
    {
        public int? Id { get; set; }
        public int BrandId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool InStock { get; set; }
    }
}
