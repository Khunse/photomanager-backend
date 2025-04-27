using System.Security.Claims;
using System.Text;
using Amazon.S3;
using imageuploadandmanagementsystem.Common;
using imageuploadandmanagementsystem.Common.Extension;
using imageuploadandmanagementsystem.Data;
using imageuploadandmanagementsystem.Data.Repository;
using imageuploadandmanagementsystem.Model;
using imageuploadandmanagementsystem.Service.Authentication;
using imageuploadandmanagementsystem.Service.ImageService;
using imageuploadandmanagementsystem.Service.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

EnvironmentValidator.ValidateEnvironmentVariables();
var jwtParameters = builder.Configuration.GetSection("JWTParameters");
var corsPolicyName = "AllowAllOrigins";
var allowedOrgins = builder.Configuration.GetSection("Origins:AllowedOrigins").Get<string[]>() ?? [];

var dbhostname = Environment.GetEnvironmentVariable("DB_HOST_NAME");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUserName = Environment.GetEnvironmentVariable("DB_USER_NAME");
var dbUserPassword = Environment.GetEnvironmentVariable("DB_USER_PASSWORD");
var connectionString = $"Server={dbhostname};Port={dbPort};Database={dbName};User Id={dbUserName};Password={dbUserPassword}";

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(corsPolicyName,
            builder =>
            {
                builder.WithOrigins(allowedOrgins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    }
);

builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtParameters["Issuer"],
                ValidAudience = jwtParameters["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY" ) ?? ""))
            };
        });

builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseNpgsql(connectionString);
});
builder.Services.AddAuthorization();

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IAuthenticationService, JWTAuthenticationService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IImageRepository,ImageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapPost("/register", (RequestModel<RegisterRequest> request, IUserService userService) =>
{
    if(!request.Req.ValidateRegisterRequestModel())
    {
        return Results.BadRequest("Invalid request");
    }
    var response =  userService.RegisterNewUser(request.Req.Email, request.Req.Password);

    return Results.Ok(response);
});

app.MapPost("/login", async (LoginRequest request, IAuthenticationService authenticationService) =>
{
    var response = await authenticationService.LoginAsync(request);
    return Results.Ok(response);
});

app.MapPost("/images/generateTempUploadUrl", [Authorize] async (RequestModel<ImageRequestModel> req,IImageService imageService,HttpContext _context) =>
{
   var userEmail = _context.User.FindFirst(ClaimTypes.Name)?.Value;
    if( userEmail is null)
    {
        return Results.BadRequest("User Not Found!");
    }
   
    if( req.Req.imageUrls.Count == 0)
    {
        return Results.BadRequest("No images to upload");
    }

    var url = await imageService.GenerateTempUploadLinkAsync(userEmail, req.Req.imageUrls);
    return Results.Ok(url);
}); 

app.MapPost("/images",[Authorize] async(RequestModel<ImageRequestModel> req,IImageService imageService,HttpContext _context) => {

    var userEmail = _context.User.FindFirst(ClaimTypes.Name)?.Value;
    if( userEmail is null)
    {
        return Results.BadRequest("User Not Found!");
    }

    var urls = await imageService.GetImagesListAsync(userEmail);
    return Results.Ok(urls);
});

app.MapPost("/images/delete",[Authorize] async(RequestModel<ImageRequestModel> req,IImageService imageService, HttpContext _context) => {

    var userEmail = _context.User.FindFirst(ClaimTypes.Name)?.Value;
    if( userEmail is null)
    {
        return Results.BadRequest("User Not Found!");
    }

    if(req.Req.imageUrls.Count == 0)
    {
        return Results.BadRequest("No images to delete");
    }

    var isDeleted = await imageService.DeleteImagesAsync(userEmail, req.Req.imageUrls.Select(x => x.name).ToList());
    return Results.Ok(isDeleted);
});

app.MapPost("/getcurrentuser", [Authorize] async (UserRequest req,IUserService userService, HttpContext _context) =>
{
    var userEmail = _context.User.FindFirst(ClaimTypes.Name)?.Value;
    if( userEmail is null)
    {
        return Results.BadRequest("User Not Found!");
    }

    var response = await userService.GetCurrentUserInfo(userEmail);
    return Results.Ok(response);
});

app.MapPost("/gettempurldownload", [Authorize] async (RequestModel<ImageRequestModel> req,IImageService imageService, HttpContext _context) =>
{
    var userEmail = _context.User.FindFirst(ClaimTypes.Name)?.Value;
    if( userEmail is null)
    {
        return Results.BadRequest("User Not Found!");
    }

    if(req.Req.imageUrls.Count == 0)
    {
        return Results.BadRequest("No images to download");
    }

    var urls = await imageService.GenerateTempGetLinkAsync(userEmail, req.Req.imageUrls);
    return Results.Ok(urls);
});

app.Run();
