namespace BlogApp.Entity
{
    public enum TagsColor { success, info, warning, danger, secondary}
    public class Tag
    {
        public int TagId { get; set; }
        public string? Text { get; set; }
        public string? Url { get; set; }
        public TagsColor Color { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
