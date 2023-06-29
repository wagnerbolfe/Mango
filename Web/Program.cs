using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Web.Services;
using Web.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();
//builder.Services.AddHttpClient<ICartService, CartService>();
//builder.Services.AddHttpClient<IAuthService, AuthService>();
//builder.Services.AddHttpClient<IOrderService, OrderService>();

StartDetail.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
StartDetail.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"];
StartDetail.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
StartDetail.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
StartDetail.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
//builder.Services.AddScoped<ITokenProvider, TokenProvider>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICartService, CartService>();
//builder.Services.AddScoped<IAuthService, AuthService>();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.ExpireTimeSpan = TimeSpan.FromHours(10);
//        options.LoginPath = "/Auth/Login";
//        options.AccessDeniedPath = "/Auth/AccessDenied";
//    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
