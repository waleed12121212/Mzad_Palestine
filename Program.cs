using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Python.Runtime;
using System.IO;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// إضافة الخدمات
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Phone Price Prediction API",
        Version = "v1",
        Description = "API للتنبؤ بأسعار الهواتف المحمولة بناءً على مواصفاتها",
        Contact = new OpenApiContact
        {
            Name = "Mzad Palestine",
            Email = "support@mzad-palestine.com"
        }
    });
});

// إعداد Python
var pythonPath = Environment.GetEnvironmentVariable("PYTHON_PATH") ?? @"C:\Python39";
Runtime.PythonDLL = Path.Combine(pythonPath, "python39.dll");

// إضافة مسار Python إلى متغيرات البيئة
Environment.SetEnvironmentVariable("PYTHONPATH", 
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "python"));

// تهيئة Python
PythonEngine.Initialize();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); 