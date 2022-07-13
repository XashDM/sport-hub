using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SportHub.Config.JwtAuthentication;
using SportHub.Domain;
using SportHub.Services;
using System.Net;
using System.Net.Mail;
using SportHub.Services.ArticleServices;
using SportHub.Services.Interfaces;
using SportHub.Services.NavigationItemServices;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata;
using SportHub.Services.Services;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

BlobContainerClient blobContainerClient = new BlobContainerClient(
    builder.Configuration.GetConnectionString("BLOBConnectionString"), 
    builder.Configuration.GetSection("BLOBContainerName").Value
    );

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddControllers();

builder.Services.AddControllers();
builder.Services.AddDbContext<SportHubDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SportHubDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IJwtSigner, JwtSigner>();
builder.Services.AddTransient<IConfigureOptions<JwtBearerOptions>, JwtConfigurer>();
builder.Services.AddScoped<INavigationItemService, MainNavigationItemService>();
builder.Services.AddScoped<IGetArticleService, GetArticleService>();
builder.Services.AddScoped<IGetAdminArticlesService, GetAdminArticlesService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IImageService>(x => new ImageService(blobContainerClient));
builder.Services
    .AddFluentEmail("sporthub.mailservice@gmail.com", "SportHub")
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient("smtp.live.com")
    {
        UseDefaultCredentials = false,
        Port = 587,
        Credentials = new NetworkCredential("sportshub.service@hotmail.com", "steamisjustavaporizedwater123"),
        EnableSsl = true
    });
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

//localization
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
    opt =>
    {
        var supportedCultres = new List<CultureInfo>
        {
            new CultureInfo ("en"),
            new CultureInfo("uk-UA")
        };
        opt.DefaultRequestCulture = new RequestCulture("en");
        opt.SupportedCultures = supportedCultres;
        opt.SupportedUICultures = supportedCultres;
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

var app = builder.Build();

//localization
var options = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Localizaton}/{action=LocalizatonIndex}/{id?}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/Errors/{0}");
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
