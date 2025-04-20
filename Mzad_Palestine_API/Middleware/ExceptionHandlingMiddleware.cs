using System.Net;
using System.Text.Json;

namespace Mzad_Palestine_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // استدعاء الـ Middleware التالي في التسلسل
                await _next(context);
            }
            catch (Exception ex)
            {
                // تسجيل الاستثناء
                _logger.LogError(ex, "حدث خطأ غير متوقع.");
                // التعامل مع الاستثناء وإرجاع استجابة موحدة
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            // يمكن تغيير الكود حسب نوع الاستثناء إذا كان لديك استثناءات معرفة مسبقاً
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "حدث خطأ في الخادم. الرجاء المحاولة مرة أخرى.",
                // في البيئات الإنتاجية يُفضّل إخفاء التفاصيل الدقيقة لتجنب كشف معلومات حساسة
                Detailed = exception.Message //يمكن الاستغناء عنها في البيئات الإنتاجية
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
