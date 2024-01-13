namespace MedinaApi.Helpers
{
    public static class StaticDirectories
    {
        public static readonly string AppImages = "AppImages";
        public static readonly string CarouselImages = "CarouselImages";
    }

    public static class StaticAppLanguages
    {
        public static readonly byte English = 1;
        public static readonly byte Kurdish = 2;
        public static readonly byte Arabic = 3;
    }

    public static class StaticCarouselType
    {
        public static readonly short Image = 0;
        public static readonly short Video = 1;
    }

    public static class StaticTokenRole
    {
        public static readonly string Admin = "admin";
        public static readonly string User = "user";

    }

    public static class StaticGenders
    {
        public static readonly short Male = 0;
        public static readonly short Female = 1;
        public static readonly short PreferNotToSay = 2;
    }
}
