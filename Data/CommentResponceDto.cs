namespace E_commerce.Data
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } 
        public int ProductId { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
