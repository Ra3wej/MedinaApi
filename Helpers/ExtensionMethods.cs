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

    }
}
