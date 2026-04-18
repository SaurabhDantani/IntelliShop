using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Infrastructure: EF Core DbContext (PostgreSQL) ────────────────────────────
builder.Services.AddDbContext<EcommerceContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));

// ── Identity: ASP.NET Core Identity wired to EcommerceContext ─────────────────
builder.Services.AddIdentity<AspNetUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    //options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<EcommerceContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Index";
    options.AccessDeniedPath = "/User/Index";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});
// ── MVC ───────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();


// ── Register Application Services here ───────────────────────────────────────
// builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpClient<Ecommerce.Services.CerebrasChatService>();
builder.Services.AddScoped<Ecommerce.Services.CerebrasChatService>();

var app = builder.Build();

// ── HTTP Pipeline ─────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   // <-- must come before UseAuthorization
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
