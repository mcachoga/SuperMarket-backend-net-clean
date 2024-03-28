namespace SuperMarket.Common.Requests.Products
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}