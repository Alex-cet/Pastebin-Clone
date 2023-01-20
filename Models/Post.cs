namespace PastebinClone.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }
}
