using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private readonly BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts; // toList demiyoruz burda. bunu controllerda yapcaz.

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
    }
}
