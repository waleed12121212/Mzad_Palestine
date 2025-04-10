﻿using System.Text;
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

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext and Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register services
builder.Services.AddScoped<ISupportService , SupportService>();
// Register other services...

// 3. إعداد JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true ,
        ValidateAudience = true ,
        ValidateLifetime = true ,
        ValidateIssuerSigningKey = true ,
        ValidIssuer = jwtSettings["Issuer"] ,
        ValidAudience = jwtSettings["Audience"] ,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 4. تسجيل الـ Repositories
builder.Services.AddScoped<IUserRepository , UserRepository>();
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

// 5. تسجيل UnitOfWork
builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();

// 6. تسجيل Services
builder.Services.AddScoped<IUserService , UserService>();
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

// 7. تسجيل AutoMapper (مع MappingProfile)
builder.Services.AddAutoMapper(typeof(Program));

// 8. تسجيل MediatR (لنمط CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// 9. تسجيل FluentValidation (في حال كنت تستخدمها)
builder.Services.AddControllers()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(typeof(Program).Assembly));

// 10. إضافة Controllers و Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 11. تسجيل الـ Middleware الخاص بإدارة الاستثناءات (Global Exception Handling)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// استخدام Authentication و Authorization
app.UseAuthentication();
app.UseAuthorization();

// تسجيل Controllers
app.MapControllers();

app.Run();