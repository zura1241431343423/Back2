namespace E_commerce.Data
{
    public class SubcategoryDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryWithSubcategoriesDto
    {
        public string Category { get; set; } = string.Empty;
        public List<SubcategoryDto> Subcategories { get; set; } = new();
    }
}