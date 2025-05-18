using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// إضافة الخدمات
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// إضافة CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// إعداد Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Phone Price Prediction API",
        Version = "v1",
        Description = "API للتنبؤ بأسعار الهواتف المحمولة بناءً على مواصفاتها"
    });
});

var app = builder.Build();

// تكوين خط أنابيب الطلب
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Phone Price Prediction API V1");
        c.RoutePrefix = string.Empty; // لجعل Swagger الصفحة الرئيسية
    });
}

// استخدام CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// تعيين المنفذ
app.Urls.Clear();
app.Urls.Add("http://localhost:5252");
app.Urls.Add("https://localhost:7252");

app.Run(); 