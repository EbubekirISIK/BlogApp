using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
       
        IQueryable<User> Users { get;  } // bu liste içindir. sadece okunabilir. IEnumarable ile farkı, bu ilk filtreler ama ıEnuma tüm veriyi getirir sonra filtreler.
        void CreatePost(User user);
    }
}
