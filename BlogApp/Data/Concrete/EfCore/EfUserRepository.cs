using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        private readonly BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<User> Users => _context.Users; // toList demiyoruz burda. bunu controllerda yapcaz.

        public void CreatePost(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
