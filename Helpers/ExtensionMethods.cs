using MedinaApi.DTO;
using MedinaApi.Models;

namespace MedinaApi.Helpers
{
    public static class ExtensionMethods
    {
        public static LanguageSetDto GetLanguageObject(this LanguageSet set)
        {
            return new LanguageSetDto
            {
                English = set.English,
                Arabic = set.Arabic,
                Kurdish = set.Kurdish
            };
        }

        public static string GetFileUrl(this HttpContext httpContext, string folderPath, string? fileName)
        {
            if (fileName?.Trim() == "" || fileName == null)
            {
                return "";
            }
            var Url = "" + httpContext.Request.Scheme.ToString() + "://" + httpContext.Request.Host.ToString() + "/" + "api" + "/" + "Files" + "/" + folderPath + "/" + fileName;
            return Url;
        }


     
    }
}
