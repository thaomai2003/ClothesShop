using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFinalExam.Data;
using MyFinalExam.Helpers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShopFixContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopManagerment"));
});

// Sessions 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register PayPal Client Singleton
builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOptions:AppId"],
    builder.Configuration["PaypalOptions:AppSecret"],
    builder.Configuration["PaypalOptions:Mode"]
));
// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Customer/Login";
    options.AccessDeniedPath = "/AccessDenied";
});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "AdminScheme";
//})
//.AddCookie("AdminScheme", options =>
//{
//    options.LoginPath = "/Admin/CustomerAdmin/AdminLogin"; // Đường dẫn đến trang đăng nhập của admin
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure to serve static files

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
    endpoints.MapDefaultControllerRoute();
});
app.Run();
