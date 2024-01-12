using System.Collections;
using MedinaApi.Helpers;
using MedinaApi.Models;

namespace MedinaApi.DTO
{
    public class ReorderImagesDTO
    {
        public int id { get; set; }
        public int index { get; set; }
    }
    public class UploadProductImageDTO
    {
        public Guid productId { get; set; }
        public List<IFormFile> File { get; set; }
    }

    public class GetProductImagesDTO
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GetProductDto
    {
        public Guid Id { get; set; }
        public LanguageSetDto Name { get; set; }
        public LanguageSetDto Category { get; set; }
        public LanguageSetDto ShortDescription { get; set; }
        public LanguageSetDto LongDescription { get; set; }
        public string MainImage { get; set; }
        public int CategoryId { get; set; }
        public List<string> Images { get; set; }

    }
    public class GetProductDashboardDto
    {
        public Guid Id { get; set; }
        public string EnglishName { get; set; }
        public string KurdishName { get; set; }
        public string ArabicName { get; set; }
        public int CategoryId { get; set; }
        public string EnglishShortDescription { get; set; }
        public string KurdishShortDescription { get; set; }
        public string ArabicShortDescription { get; set; }
        public string EnglishLongDescription { get; set; }
        public string KurdishLongDescription { get; set; }
        public string ArabicLongDescription { get; set; }
        public bool IsActive { get; set; }
        public string MainImage { get; set; }
        public List<ProductImageReturnDto> Images { get; set; }
    }

    public class ProductImageReturnDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }

    public class AddProductDTO
    {
        public string EnglishName { get; set; }
        public string KurdishName { get; set; }
        public string ArabicName { get; set; }
        public int CategoryId { get; set; }
        public string EnglishShortDescription { get; set; }
        public string KurdishShortDescription { get; set; }
        public string ArabicShortDescription { get; set; }
        public string EnglishLongDescription { get; set; }
        public string KurdishLongDescription { get; set; }
        public string ArabicLongDescription { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile>? Images { get; set; } = new();
    }
    public class UpdateProductDTO
    {
        public Guid Id { get; set; }
        public string EnglishName { get; set; }
        public string KurdishName { get; set; }
        public string ArabicName { get; set; }
        public int CategoryId { get; set; }
        public string EnglishShortDescription { get; set; }
        public string KurdishShortDescription { get; set; }
        public string ArabicShortDescription { get; set; }
        public string EnglishLongDescription { get; set; }
        public string KurdishLongDescription { get; set; }
        public string ArabicLongDescription { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? AddedImages { get; set; } = new();
        public List<int>? DeletedImages { get; set; }=new();
    }
}