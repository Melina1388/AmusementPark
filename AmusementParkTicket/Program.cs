using AmusementPark.Application.Interfaces;
using AmusementPark.Application.Services;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Repositories;
using AmusementPark.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();


// =====================================================
// Session
// =====================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.SameSite = SameSiteMode.Lax;
});


// =====================================================
// Authentication
// =====================================================
//
// Session برای اطلاعات موقت استفاده می‌شود.
// Cookie برای نگه داشتن Login کاربر بعد از
// بستن و باز کردن دوباره مرورگر استفاده می‌شود.
//

builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name =
            "AmusementPark.Auth";

        options.Cookie.HttpOnly = true;

        options.Cookie.IsEssential = true;

        options.Cookie.SameSite =
            SameSiteMode.Lax;

        // کاربر تا زمان Logout باقی می‌ماند.
        options.ExpireTimeSpan =
            TimeSpan.FromDays(3650);

        options.SlidingExpiration = true;

        options.LoginPath =
            "/Login/Login";

        options.LogoutPath =
            "/Login/Logout";
    });


// =====================================================
// Repository Dependency Injection
// =====================================================

builder.Services.AddScoped<
    IPlayerRepository,
    PlayerRepository>();

builder.Services.AddScoped<
    IGameRepository,
    GameRepository>();

builder.Services.AddScoped<
    ITicketRepository,
    TicketRepository>();

builder.Services.AddScoped<
    ITransactionRepository,
    TransactionRepository>();


// =====================================================
// Application Services
// =====================================================

builder.Services.AddScoped<
    IPlayerService,
    PlayerService>();

builder.Services.AddScoped<
    IOtpService,
    OtpService>();

builder.Services.AddScoped<
    IGameService,
    GameService>();

builder.Services.AddScoped<
    ITicketService,
    TicketService>();

builder.Services.AddScoped<
    ITransactionService,
    TransactionService>();


// =====================================================
// API Service
// =====================================================
builder.Services.AddHttpClient<APIService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

var app = builder.Build();


// =====================================================
// HTTP Pipeline
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


// Authentication باید قبل از Authorization باشد.
app.UseAuthentication();

app.UseSession();

app.UseAuthorization();


// =====================================================
// Default Route
// =====================================================
//
// سایت از Home شروع می‌شود.
//
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");


app.Run();