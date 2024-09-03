namespace be_ip.Product.Classes
{
    public class BaseProduct
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public double Price { get; set; }

        public string PictureUrl { get; set; } = default!;
    }
}
