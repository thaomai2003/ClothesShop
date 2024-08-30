namespace MyFinalExam.ViewModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductSale {  get; set; }
        public string ProductImageURL { get; set; }

        public decimal PriceTotal => ProductSale * Quantity;
    }
}
