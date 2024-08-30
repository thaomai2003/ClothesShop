using MyFinalExam.Data;

namespace MyFinalExam.ViewModels
{
    public class ProductVM
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int? CategoryID { get; set; }
        public decimal? Price { get; set; }
        public string? ImageURL { get; set; }
        public string? CategoryName { get; set; }
        public int? DetailSale {  get; set; }
    }
    public class ProductDetailVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal? Price { get; set; }
        public string? ImageURL { get; set; }
        public string CategoryDescription { get; set; }
        public int? StockQuantity { get; set; }
        public int? CategoryId { get; set; }
        public int? Sale { get; set; }
        public int NumberRate { get; set; }
        public List<ProductVM> SimilarProducts { get; set; }

    }
}
