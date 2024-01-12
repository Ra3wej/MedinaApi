namespace MedinaApi.DTO
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public LanguageSetDto Category { get; set; }
    }
    public class GetCategoryDashboardDto
    {
        public int Id { get; set; }
        public string EnglishCategory { get; set; }
        public string KurdishCategory { get; set; }
        public string ArabicCategory { get; set; }
        public bool IsActive { get; set; }
    }
    public class AddCategoryDto
    {
        public string EnglishCategory { get; set; }
        public string KurdishCategory { get; set; }
        public string ArabicCategory { get; set; }
    }

    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string EnglishCategory { get; set; }
        public string KurdishCategory { get; set; }
        public string ArabicCategory { get; set; }
    }
}
