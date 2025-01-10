using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository
    {
       
        IQueryable<Comment> Comments { get;  } // bu liste içindir. sadece okunabilir. IEnumarable ile farkı, bu ilk filtreler ama ıEnuma tüm veriyi getirir sonra filtreler.
        void CreateComment(Comment comment);
    }
}
