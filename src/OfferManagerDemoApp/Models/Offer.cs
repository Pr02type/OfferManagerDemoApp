using System;

namespace OfferManagerDemo.Models
{
    public class Offer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Number { get; set; } = "";
        public string Customer { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Draft";
    }
}
