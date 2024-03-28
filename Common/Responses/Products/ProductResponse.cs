﻿namespace SuperMarket.Common.Responses.Products
{
    public class ProductResponse
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}