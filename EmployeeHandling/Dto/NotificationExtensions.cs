using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EmployeeHandling.Dto
{
    public static class NotificationExtensions
    {
        public static void Success(this ITempDataDictionary tempData, string message)
            => tempData["ToastMessage"] = $"success|{message}";

        public static void Error(this ITempDataDictionary tempData, string message)
            => tempData["ToastMessage"] = $"error|{message}";
    }

}
