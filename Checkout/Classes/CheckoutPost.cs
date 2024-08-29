using be_ip.Product.Classes;

namespace be_ip.Checkout.Classes
{
    public class CheckoutPost
    {
        public IList<BaseProduct> Items { get; set; }
    }
}
