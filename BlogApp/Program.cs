using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(options =>
{
    var config = builder.Configuration;
    var connection = config.GetConnectionString("sql_connetion");
    options.UseSqlite(connection);

});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/User/Login"; // Yetkisiz kullanýcýlarýn yönlendirileceði yol
});

var app = builder.Build();

SeedData.TestVerileriniDoldur(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "post_by_tag",
    pattern: "posts/tag/{tag}",
    defaults: new { controller = "Posts", action = "Index" });

app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/details/{url}",
    defaults: new {controller = "Posts", action = "Details"});

app.MapControllerRoute(
    name: "profil",
    pattern: "user/profil/{profil}",
    defaults: new { controller = "User", action = "Profile" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

app.Run();
