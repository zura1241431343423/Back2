namespace E_commerce.Data
{
    public class CommentDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } 
    }
}
