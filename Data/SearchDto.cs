namespace E_commerce.Data
{
    public class SearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string[] Images { get; set; }
        public string Category { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}
