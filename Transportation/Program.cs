using BusinessLogic.DTOs.SendEmail;
using BusinessLogic.Filter;
using BusinessLogic.Interfaces;
using BusinessLogic.Public;
using BusinessLogic.Services;
using BusinessLogic.Services.Account;
using DataAccess.DataContext;
using DataAccess.IRepositories;

using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using Transportation.Hubs;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
/*services.AddControllers(options =>
{
    options.Filters.Add(typeof(DemoAuthorizeActionFilter));
});*/
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; 
    });

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Giữ nguyên tên thuộc tính
        options.JsonSerializerOptions.WriteIndented = true; // JSON dễ đọc hơn
    });
// Thêm Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Transportation API", Version = "v1" });

    // Support for file upload
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    // Nếu cần hỗ trợ form có nhiều file:
    options.MapType<IFormFileCollection>(() => new OpenApiSchema
    {
        Type = "array",
        Items = new OpenApiSchema { Type = "string", Format = "binary" }
    });
});
;



builder.Services.AddDbContext<MyDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Mydb"));
  
});
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisCacheUrl"]; });
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ trong để lưu session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn Session (30 phút)
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập qua HTTP, tăng bảo mật
    options.Cookie.IsEssential = true; // Session hoạt động ngay cả khi người dùng từ chối cookie
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                context.Token = token; // Lấy token từ cookie
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))

    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IGoogleAuthHelper, GoogleAuthHelperService>();
builder.Services.AddScoped<IGooogleAuthorization, GoogleAuthorizationService>();
//chỉ dùng khi fronend tách rien cồng khác
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
          
    });
});*/

builder.Services.AddSignalR();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddTransient<IShiftRepository, ShiftRepository>();
builder.Services.AddTransient<IShiftService, ShiftService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountRepo, AccountRepo>(); 
builder.Services.AddTransient<IShippingRequestService, ShippingRequestService>(); 
 builder.Services.AddTransient<IWarehouseService, WarehouseService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITripService, TripService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<ITruckService, TruckService>();
builder.Services.AddTransient<IDispatchService, DispatchService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseSession();
// Bật Swagger UI trong mọi môi trường
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transportation API v1");
    options.RoutePrefix = "swagger"; // Định tuyến Swagger UI tại /swagger
});

app.UseHttpsRedirection();
app.UseStaticFiles(); // Dòng này giữ lại

/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});*/

//app.UseCors("AllowAll");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<LocationHub>("/locationHub");



// Định tuyến API (không cần action mặc định "Index")
app.MapControllerRoute(
    name: "api",
    pattern: "api/{area=}/{controller}/{action?}/{id?}");

// Định tuyến cho MVC với Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");

// Định tuyến mặc định cho MVC (không có area)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Tự động ánh xạ API Controllers
app.MapControllers();

app.Run();

/*Add-Migration Updatfg -Project DataAccess -StartupProject Transportation
Update-Database -Project DataAccess -StartupProject Transportation*/

