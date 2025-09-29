using E_commerce.Models;

namespace E_commerce.Abstractions
{
    public static class RatingAbstractions
    {
       
        private static readonly int[] ProductIds =
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20
        };

        private static readonly int[] UserIds = { 1, 2, 3, 4, 5, 6 };

        public static List<Rating> GetPredefinedRatings()
        {
            var ratings = new List<Rating>();
            var random = new Random(42); 
            var baseDate = DateTime.UtcNow.AddDays(-90); 
            
            var ratingPatterns = new Dictionary<int, int[]>
            {
               
                { 1, new[] { 5, 5, 4, 5, 4, 5 } },
                { 2, new[] { 5, 4, 5, 4, 5, 4 } },
                { 3, new[] { 4, 5, 5, 5, 4, 5 } },
                
             
                { 4, new[] { 4, 5, 4, 3, 4, 5 } },
                { 5, new[] { 5, 4, 4, 4, 3, 4 } },
                { 6, new[] { 4, 4, 5, 4, 4, 3 } },
                { 7, new[] { 3, 4, 5, 4, 4, 4 } },
                
                
                { 8, new[] { 3, 4, 4, 3, 4, 3 } },
                { 9, new[] { 4, 3, 4, 3, 5, 3 } },
                { 10, new[] { 3, 3, 4, 4, 3, 4 } },
                { 11, new[] { 4, 3, 3, 4, 4, 3 } },
                
             
                { 12, new[] { 3, 2, 3, 3, 2, 4 } },
                { 13, new[] { 2, 3, 3, 2, 3, 3 } },
                { 14, new[] { 3, 3, 2, 4, 2, 3 } },
                { 15, new[] { 2, 4, 3, 2, 3, 2 } },
                
                
                { 16, new[] { 2, 1, 2, 3, 1, 2 } },
                { 17, new[] { 1, 2, 2, 1, 3, 2 } },
                { 18, new[] { 2, 2, 1, 2, 1, 3 } },
                
                
                { 19, new[] { 1, 1, 2, 1, 2, 1 } },
                { 20, new[] { 1, 2, 1, 1, 1, 2 } }
            };

            foreach (var productId in ProductIds)
            {
                var pattern = ratingPatterns[productId];

                for (int i = 0; i < UserIds.Length; i++)
                {
                    ratings.Add(new Rating
                    {
                        ProductId = productId,
                        UserId = UserIds[i],
                        Value = pattern[i],
                        RatedAt = baseDate.AddDays(random.Next(0, 90)).AddHours(random.Next(0, 24))
                    });
                }
            }

            return ratings;
        }
    }
}