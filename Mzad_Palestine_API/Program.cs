using System.Text;
using MediatR;
using AutoMapper;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using Mzad_Palestine_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FluentValidation.AspNetCore;
using Mzad_Palestine_API.Middleware;
using Mzad_Palestine_Core.Validation;
using Microsoft.ML;
using System.IdentityModel.Tokens.Jwt;
using Mzad_Palestine_Core.Interfaces.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext and Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 9;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// تسجيل RoleManager بشكل صحيح
builder.Services.AddScoped<IRoleStore<IdentityRole<int>>, RoleStore<IdentityRole<int>, ApplicationDbContext, int>>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll" ,
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true ,
        ValidateAudience = true ,
        ValidateLifetime = true ,
        ValidateIssuerSigningKey = true ,
        ValidateActor = false ,
        ValidateTokenReplay = false ,
        ClockSkew = TimeSpan.Zero ,
        ValidIssuer = jwtSettings["Issuer"] ,
        ValidAudience = jwtSettings["Audience"] ,
        IssuerSigningKey = new SymmetricSecurityKey(key) ,
        NameClaimType = ClaimTypes.Name ,
        RoleClaimType = ClaimTypes.Role
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = context.SecurityToken as JwtSecurityToken;

            if (token != null)
            {
                // التحقق من وجود الدور في التوكن
                var roleClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (roleClaim == null)
                {
                    context.Fail("التوكن لا يحتوي على صلاحيات");
                    return;
                }

                var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                var userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var userRoles = await userManager.GetRolesAsync(user);
                        if (!userRoles.Contains(roleClaim.Value))
                        {
                            context.Fail("الصلاحية غير صالحة");
                            return;
                        }
                    }
                }
            }

            // التحقق من وجود التوكن في القائمة السوداء
            var jti = token?.Id;
            if (!string.IsNullOrEmpty(jti) && Mzad_Palestine_Infrastructure.Repositories.AuthRepository.RevokedTokens.Contains(jti))
            {
                context.Fail("تم إلغاء هذا التوكن");
            }
        }
    };
});

// Register services
builder.Services.AddScoped<ISupportService , SupportService>();

// Register Repositories
builder.Services.AddScoped<IUserRepository , UserRepository>();
builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
builder.Services.AddScoped<IListingRepository , ListingRepository>();
builder.Services.AddScoped<IAuctionRepository , AuctionRepository>();
builder.Services.AddScoped<IBidRepository , BidRepository>();
builder.Services.AddScoped<IPaymentRepository , PaymentRepository>();
builder.Services.AddScoped<IMessageRepository , MessageRepository>();
builder.Services.AddScoped<IReviewRepository , ReviewRepository>();
builder.Services.AddScoped<IReportRepository , ReportRepository>();
builder.Services.AddScoped<INotificationRepository , NotificationRepository>();
builder.Services.AddScoped<IAutoBidRepository , AutoBidRepository>();
builder.Services.AddScoped<IDisputeRepository , DisputeRepository>();
builder.Services.AddScoped<ITagRepository , TagRepository>();
builder.Services.AddScoped<IWatchlistRepository , WatchlistRepository>();
builder.Services.AddScoped<ISubscriptionRepository , SubscriptionRepository>();
builder.Services.AddScoped<ICustomerSupportTicketRepository , CustomerSupportTicketRepository>();
builder.Services.AddScoped<ISupportRepository , SupportRepository>();
builder.Services.AddScoped<IAuthRepository , AuthRepository>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();

// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register Services
builder.Services.AddScoped<IUserService , UserService>();
builder.Services.AddScoped<ICategoryService , CategoryService>();
builder.Services.AddScoped<IListingService , ListingService>();
builder.Services.AddScoped<IAuctionService , AuctionService>();
builder.Services.AddScoped<IBidService , BidService>();
builder.Services.AddScoped<IPaymentService , PaymentService>();
builder.Services.AddScoped<IMessageService , MessageService>();
builder.Services.AddScoped<IReviewService , ReviewService>();
builder.Services.AddScoped<IReportService , ReportService>();
builder.Services.AddScoped<INotificationService , NotificationService>();
builder.Services.AddScoped<IAutoBidService , AutoBidService>();
builder.Services.AddScoped<IDisputeService , DisputeService>();
builder.Services.AddScoped<ITagService , TagService>();
builder.Services.AddScoped<IWatchlistService , WatchlistService>();
builder.Services.AddScoped<ISubscriptionService , SubscriptionService>();
builder.Services.AddScoped<ISupportService , SupportService>();
builder.Services.AddScoped<IAuthService , AuthService>();
builder.Services.AddScoped<ILaptopPredictionService , LaptopPredictionService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(config =>
        config.RegisterValidatorsFromAssemblyContaining<CreateAuctionDtoValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

// Important: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ Seed Roles
using (var scope = app.Services.CreateScope())
{
    try
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        string[] roleNames = { "User" , "Admin" , "Moderator" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}: {string.Join(", " , result.Errors)}");
                }
            }
        }

        // التحقق من المستخدمين الحاليين وتعيين دور User لهم إذا لم يكن لديهم أي دور
        var users = userManager.Users.ToList();
        foreach (var user in users)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                await userManager.AddToRoleAsync(user , "User");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex , "An error occurred while seeding roles");
    }
}

app.Run();
