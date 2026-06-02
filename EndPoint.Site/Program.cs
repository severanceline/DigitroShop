using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Carts;
using Store.Application.Services.Common.Queries.GetCategory;
using Store.Application.Services.Common.Queries.GetMenuItem;
using Store.Application.Services.Finances.Commands.AddRequestPay;
using Store.Application.Services.Finances.Queries.GetAllRequestPayForAdmin;
using Store.Application.Services.Finances.Queries.GetRequestPay;
using Store.Application.Services.Finances.Queries.GetRequestPayForAdmin;
using Store.Application.Services.Finances.Queries.GetRequestPayForUser;
using Store.Application.Services.Finances.Queries.UpdateRequstPay;
using Store.Application.Services.HomePage.Commands.AddNewSlider;
using Store.Application.Services.HomePage.Commands.EditHomePageImage;
using Store.Application.Services.HomePage.Commands.EditSlider;
using Store.Application.Services.HomePage.Commands.RemoveHomePageImage;
using Store.Application.Services.HomePage.Commands.RemoveSlider;
using Store.Application.Services.HomePage.Queries.GetHomePageImages;
using Store.Application.Services.HomePage.Queries.GetSlider;
using Store.Application.Services.HomePages.AddHomePageImages;
using Store.Application.Services.Orders.Commands.AddOrder;
using Store.Application.Services.Orders.Commands.ChangeOrderState;
using Store.Application.Services.Orders.Queries.GetOrder;
using Store.Application.Services.Orders.Queries.GetOrderForAdmin;
using Store.Application.Services.Products.Commands.AddCategory;
using Store.Application.Services.Products.FacadPattern;
using Store.Application.Services.Users.Commands.ActiveUser;
using Store.Application.Services.Users.Commands.EditUser;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RemoveUser;
using Store.Application.Services.Users.Queries.GetRoles;
using Store.Application.Services.Users.Queries.GetUsers;
using Store.Application.Services.Users.Queries.LoginUser;
using Store.Common.Roles;
using Store.Persistance.Contexts;
using Zarinpal.AspNetCore.Consts;
using Zarinpal.AspNetCore.Enums;
using Zarinpal.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

.AddCookie(options =>
{
    options.LoginPath = "/Authentication/Signin";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.AccessDeniedPath = new PathString("/Authentication/Signin");
});

// Add services to the container.
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddScoped<IGetUsersService, GetUsersService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IGetUsersService, GetUsersService>();
builder.Services.AddScoped<IRemoveUserService, RemoveUserService>();
builder.Services.AddScoped<IAvtiveUserService, AvtiveUserService>(); 
builder.Services.AddScoped<IEditUserService, EditUserService>();
builder.Services.AddScoped<IGetRolesService, GetRolesService>();
builder.Services.AddScoped<ILoginUserService, LoginUserService>();
/////
builder.Services.AddScoped<IProductFacad, ProductFacad>();
////
builder.Services.AddScoped<IGetMenuItemService, GetMenuItemService>();
builder.Services.AddScoped<IGetCategoryService, GetCategoryService>();
builder.Services.AddScoped<IAddNewSliderService, AddNewSliderService>();
builder.Services.AddScoped<IGetSliderService, GetSliderService>();
builder.Services.AddScoped<IRemoveSliderService, RemoveSliderService>();
builder.Services.AddScoped<IEditSliderService, EditSliderService>();
builder.Services.AddScoped<IAddHomePageImagesService, AddHomePageImagesService>();
builder.Services.AddScoped<IGetHomePageImagesService, GetHomePageImagesService>();
builder.Services.AddScoped<IRemoveHomePageImageService, RemoveHomePageImageService>();
builder.Services.AddScoped<IEditHomePageImage, EditHomePageImage>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAddRequestPayService, AddRequestPayService>();
builder.Services.AddScoped<IGetRequestPayService, GetRequestPayService>();
builder.Services.AddScoped<IAddOrderService, AddOrderService>();
builder.Services.AddScoped<IUpdateRequestPayService, UpdateRequestPayService>();
builder.Services.AddScoped<IGetOrderUserService, GetOrderUserService>();
builder.Services.AddScoped<IGetRequestPayForUser, GetRequestPayForUser>();
builder.Services.AddScoped<IGetOrderForAdminService, GetOrderForAdminService>();
builder.Services.AddScoped<IChangeOrderState, ChangeOrderState>();
builder.Services.AddScoped<IGetRequestPayForAdminService, GetRequestPayForAdminService>();
builder.Services.AddScoped<IGetAllRequestPayForAdminSevice, GetAllRequestPayForAdminSevice>();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews();

builder.Services.AddZarinpal(options => 
{
    options.MerchantId = "00000000-0000-0000-0000-000000000000";
    options.ZarinpalMode = ZarinpalMode.Sandbox;
    options.Currency = ZarinpalCurrency.IRT;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserRoles.Admin, policy => policy.RequireRole("Admin"));
    options.AddPolicy(UserRoles.Customer, policy => policy.RequireRole("Customer"));
    options.AddPolicy(UserRoles.Operator, policy => policy.RequireRole("Operator"));
});

var app = builder.Build();

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
