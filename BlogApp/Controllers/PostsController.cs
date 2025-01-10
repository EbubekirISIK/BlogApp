using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private ITagRepository _tagRepository;


        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts;
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
            }
            return View(await posts.Include(x => x.Tags).ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository.Posts.Include(x => x.User).Include(x => x.Tags).Include(x => x.Comments).ThenInclude(x => x.User).FirstOrDefaultAsync(p => p.Url == url));
        }
        
        [HttpPost]
        public IActionResult AddComment(int PostId, string CommentText, string Url)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userImg = User.FindFirstValue(ClaimTypes.UserData);


            var entity = new Comment
            {
                CommentText = CommentText,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userId ?? ""),

                User = new User { UserName = userName, Images = userImg }
            };
            _commentRepository.CreateComment(entity);
            return Redirect("/posts/details/" + Url);
        }
        
        public IActionResult CreatePost()
        {
            CreatePostViewModel model = new CreatePostViewModel { Tags = _tagRepository.Tags.ToList() };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model, IFormFile Images)
        {
            // Geçerli uzantılar listesi
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            string randomFileName = null; // Varsayılan dosya ismi

            // Images null değilse işleme başla
            if (Images != null && Images.Length > 0)
            {
                var extension = Path.GetExtension(Images.FileName); // Uzantıyı al

                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
                }
                else
                {
                    randomFileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    // Resmi kaydet
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Images.CopyToAsync(stream);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Lütfen bir resim dosyası yükleyiniz.");
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var post = new Post
                {
                    Title = model.Title,
                    Description = model.Description,
                    Url = model.Url,
                    Images = randomFileName, // Eğer resim yüklenmediyse null olabilir
                    PublishesOn = DateTime.Now,
                    Content = model.Content,
                    UserId = int.Parse(userId ?? ""),
                    Tags = new List<Tag>()
                };

                // Seçilen tagleri ekle
                if (model.SelectedTagIds != null && model.SelectedTagIds.Any())
                {
                    var selectedTags = _tagRepository.Tags
                        .Where(t => model.SelectedTagIds.Contains(t.TagId))
                        .ToList();

                    post.Tags.AddRange(selectedTags);
                }

                _postRepository.CreatePost(post);
                return RedirectToAction("Index");
            }

            // Hata durumunda mevcut tagleri tekrar yükle
            model.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }

    }
}
