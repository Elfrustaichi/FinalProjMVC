using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voxo.DAL;
using Voxo.Models;
using Voxo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

//Sql database start
builder.Services.AddDbContext<VoxoContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 8;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
}).AddDefaultTokenProviders().AddEntityFrameworkStores<VoxoContext>();
//Sql database end

//Added services start
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
//Added services end

//Login cookie config start
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
        {
            var redirectUri = new Uri(context.RedirectUri);
            context.Response.Redirect("/manage/account/login" + redirectUri.Query);
        }
        else
        {
            var redirectUri = new Uri(context.RedirectUri);
            context.Response.Redirect("/account/login" + redirectUri.Query);
        }

        return Task.CompletedTask;
    };
});
//Login cookie config end

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//Route control start
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//Route control end


app.Run();
