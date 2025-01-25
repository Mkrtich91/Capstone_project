// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text;
using System.Text.Json.Serialization;
using GameStore;
using GameStore.BusinessLayer.AuthServices;
using GameStore.BusinessLayer.Facade;
using GameStore.BusinessLayer.Filters;
using GameStore.BusinessLayer.Interfaces.Configuration;
using GameStore.BusinessLayer.Interfaces.DataProvider;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.IAuthServices;
using GameStore.BusinessLayer.Interfaces.IFacade;
using GameStore.BusinessLayer.Interfaces.Mappings;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.BusinessLayer.Pagination;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Sorting;
using GameStore.DataAccessLayer.Data;
using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using GameStore.DataAccessLayer.Repositories;
using GameStore.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Entities.Converter;
using MongoDB.Entities.MongoDbContext;
using MongoDB.Repositories.IRepository;
using MongoDB.Repositories.Repository;
using MongoDB.Services.IServices;
using MongoDB.Services.Mapper;
using MongoDB.Services.Services;
using Serilog;
using Serilog.Events;
var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(typeof(string), new PostalCodeConverter());
BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeConverter());
builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .WriteTo.Console()
           .WriteTo.Logger(lc => lc
               .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error || evt.Level == LogEventLevel.Fatal)
               .WriteTo.File("Logs/exceptions-.txt", rollingInterval: RollingInterval.Day))
           .WriteTo.Logger(lc => lc
               .Filter.ByExcluding(evt => evt.Level == LogEventLevel.Error || evt.Level == LogEventLevel.Fatal)
               .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day))
           .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GameStore", Version = "v1" });
});
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddSingleton<IBanService, BanService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderGameRepository, OrderGameRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderGameService, OrderGameService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentBankService, PaymentBankService>();
builder.Services.AddScoped<IBoxPaymentService, BoxPaymentService>();
builder.Services.AddScoped<IVisaPaymentService, VisaPaymentService>();
builder.Services.AddHttpClient<IVisaPaymentService, VisaPaymentService>();
builder.Services.AddScoped<IPaymentProcessingService, PaymentProcessingService>();

builder.Services.AddTransient<IPipelineStep, GameSortStep>();
builder.Services.AddTransient<IPipelineStep, GamePaginationStep>();

builder.Services.AddTransient<IPipelineStep, GenreFilterStep>();
builder.Services.AddTransient<IPipelineStep, PlatformFilterStep>();
builder.Services.AddTransient<IPipelineStep, PublisherFilterStep>();
builder.Services.AddTransient<IPipelineStep, PriceFilterStep>();
builder.Services.AddTransient<IPipelineStep, PublishDateFilterStep>();
builder.Services.AddTransient<IPipelineStep, NameFilterStep>();

builder.Services.AddTransient<GamePipeline>();
builder.Services.AddTransient<GameQueryDto>();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.Configure<BankPaymentOptions>(builder.Configuration.GetSection("BankPaymentOptions"));
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("PaymentSettings"));
//builder.Services.Configure<ExternalAuthSettings>(builder.Configuration.GetSection("ExternalAuthService"));
builder.Services.AddMemoryCache();

builder.Services.AddTransient<Seed>();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("GameStore.DataAccessLayer")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionAuthorizationFilter>();
});

builder.Services.AddSingleton<NorthwindDataContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddScoped<IOrderMongoDBRepository, OrderMongoDBRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IShipperService, ShipperService>();
builder.Services.AddScoped<IOrderMongoDBService, OrderMongoDBService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

string mongoConnectionString = builder.Configuration.GetConnectionString("NorthwindDb");
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoConnectionString);
});

builder.Services.AddAutoMapper(typeof(GameMappingProfile));
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));
builder.Services.AddAutoMapper(typeof(OrderDetailMappingProfile));
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

builder.Services.AddAutoMapper(typeof(SupplierMappingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(RoleMappingProfile));
builder.Services.AddScoped<IGenreFacade, GenreFacade>();
builder.Services.AddScoped<IGamesFacade, GamesFacade>();
builder.Services.AddScoped<IOrderFacade, OrderFacade>();
builder.Services.AddScoped<IOrderGameFacade, OrderGameFacade>();
builder.Services.AddScoped<IPublisherFacade, PublisherFacade>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<GameCatalogService>();
builder.Services.AddScoped<SeedGamesService>();

builder.Services.AddIdentity<IdentityUser, UserRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.SignIn.RequireConfirmedEmail = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.Authority = jwtOptions.Audience;
    options.RequireHttpsMetadata = false;
    options.IncludeErrorDetails = true;
    options.Configuration = new OpenIdConnectConfiguration();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
    };
});

builder.Services.AddAuthorization();

builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            Console.WriteLine($"Authentication failed: {context.ToString}");
            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            Console.WriteLine($"Authentication failed: {context.ErrorDescription}");
            return Task.CompletedTask;
        },
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token. Example: `Bearer <token>`",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        },
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
    await productRepository.AddGameKeysToProductsAsync();
}

using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<SeedGamesService>();
    await seedService.SeedGamesAsync();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    await DataInitSeeder.SeedDataAsync(userManager, roleManager, context);
}

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    SeedData(app);
}

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using var scope = scopedFactory.CreateScope();
    var service = scope.ServiceProvider.GetService<Seed>();
    service.SeedDataContext();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore v1");
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseMiddleware<AddTotalGamesHeaderMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
           .WithExposedHeaders("x-total-numbers-of-games");
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionMiddleware();

app.MapControllers();
app.Run();
