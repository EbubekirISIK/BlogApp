using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ITagRepository
    {
       
        IQueryable<Tag> Tags { get;  } // bu liste içindir. sadece okunabilir. IEnumarable ile farkı, bu ilk filtreler ama ıEnuma tüm veriyi getirir sonra filtreler.
        void CreatePost(Tag tag);
    }
}
