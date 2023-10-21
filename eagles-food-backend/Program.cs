global using AutoMapper;

using System.Reflection;
using System.Text;

using Asp.Versioning;

using eagles_food_backend;
using eagles_food_backend.Data;
using eagles_food_backend.Services;
using eagles_food_backend.Services.EmailService;
using eagles_food_backend.Services.LunchRepository;
using eagles_food_backend.Services.OrganizationRepository;
using eagles_food_backend.Services.ResponseService;
using eagles_food_backend.Services.UriService;
using eagles_food_backend.Services.UserServices;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "Server=MYSQL8002.site4now.net;Database=db_a9ef58_teagle;Uid=a9ef58_teagle;Pwd=c7b9@r.jDR7.ZKW";

builder.Services.AddDbContext<LunchDbContext>(options =>
    options.UseMySql(
        connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.AddControllers();
var config = builder.Configuration;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:secretKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddScoped<ILunchRepository, LunchService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<AuthenticationClass>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSwaggerGen(opts =>
{
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please enter a valid token",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id="Bearer",
                    Type=ReferenceType.SecurityScheme
                },
            },
            new string[]{}
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eagles Food API",
        Version = "v1"
    });
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services
    .AddApiVersioning(o =>
    {
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.ReportApiVersions = true;
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        // the default is ToString(), but we want "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";
        // if we have both parts, decided how to format the group
        // from the example: "Sales - v1"
        options.FormatGroupName = (group, version) => $"{group} - {version}";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<LunchDbContext>();
    dataContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
Console.WriteLine("Connected to db: " + connectionString);

app.Run();
