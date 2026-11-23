using ServiSwap.DAL;
using ServiSwap.DAL.Interfaces;
using ServiSwap.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
});

string? connectionString = builder.Configuration.GetConnectionString("ServiSwapDatabase");

ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

builder.Services.AddSingleton<ICategoryDAL>(c => new CategoryDAL(connectionString));
builder.Services.AddSingleton<IPersonDAL>(c => new PersonDAL(connectionString));
builder.Services.AddSingleton<IServiceDAL>(c => new ServiceDAL(connectionString));
builder.Services.AddSingleton<IRequestedServiceDAL>(c => new RequestedServiceDAL(connectionString));

builder.Services.AddSingleton<PersonService>();
builder.Services.AddSingleton<HomePageService>();
builder.Services.AddSingleton<ServiceManager>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();


if (!app.Environment.IsDevelopment()) { app.UseHsts(); }

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
