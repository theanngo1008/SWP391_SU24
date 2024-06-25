﻿namespace BE.Models
{
    public class UpdateJewelryRequest
    {
        public string? JewelryName { get; set; }

        public decimal? Cost { get; set; }

        public int? Quantity { get; set; }

        public bool? Status { get; set; }

        public int? ChargeId { get; set; }

        public int? WarehouseId { get; set; }    

        public string? SubCateId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
