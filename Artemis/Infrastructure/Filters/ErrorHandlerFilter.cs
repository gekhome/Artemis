using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;

namespace Artemis.Infrastructure.Filters
{
    public class ErrorHandlerFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;

        public string ErrorViewName { get; set; } = "CustomError";

        public ErrorHandlerFilter(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                LogError(filterContext);

                if (!_environment.IsDevelopment())
                {
                    // Don't display exception details unless running in Development.
                    return;
                }
                var result = new ViewResult { ViewName = ErrorViewName };
                var modelMetadata = new EmptyModelMetadataProvider();
                result.ViewData = new ViewDataDictionary(modelMetadata, filterContext.ModelState)
                {
                    { "HandleException", filterContext.Exception }
                };
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
        }

        private void LogError(ExceptionContext filterContext)
        {
            string contentRootPath = _environment.ContentRootPath;

            if (!Directory.Exists(Path.Combine(contentRootPath, "ErrorLogs")))
                Directory.CreateDirectory(Path.Combine(contentRootPath, "ErrorLogs"));

            var exceptionMessage = filterContext.Exception.Message;
            var stackTrace = filterContext.Exception.StackTrace;
            var controllerName = filterContext.RouteData?.Values["controller"]?.ToString();
            var actionName = filterContext.RouteData?.Values["action"]?.ToString();

            string Message = "Date :" + DateTime.Now.ToString() + ", Controller: " + controllerName + ", Action: " + actionName + Environment.NewLine +
                 "Error Message : " + exceptionMessage + Environment.NewLine + "Stack Trace :" + Environment.NewLine
                 + stackTrace + Environment.NewLine + Environment.NewLine;

            File.AppendAllText(Path.Combine(contentRootPath, $"ErrorLogs{Path.DirectorySeparatorChar}ErrorLog.txt"), Message);
        }
    }
}
