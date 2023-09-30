namespace Voxo.ViewModels
{
    public class OrderViewModel
    {
        public OrderCreateViewModel Order { get; set; }

        public List<CheckoutViewModel> CheckoutItems { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
