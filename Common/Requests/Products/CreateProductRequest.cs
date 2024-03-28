namespace SuperMarket.Common.Requests.Products
{
    public class CreateProductRequest
    {
        public string Barcode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}