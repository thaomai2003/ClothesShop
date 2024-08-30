namespace MyFinalExam.Areas.Admin.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int? CategoryID { get; set; }
        public decimal? Price { get; set; }
        public string? ImageURL { get; set; }
        public string? CategoryName { get; set; }
        public int? DetailSale { get; set; }
    }
}
