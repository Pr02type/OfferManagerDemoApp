/// <summary>
/// Einfacher Mock für Angebotsdaten.
/// Zweck: UI-Demo & schnelle Tests ohne DB/Repository.
/// TODO: Für Produktion durch ein persistentes Repository ersetzen (EF Core/REST o.ä.).
/// </summary>

using OfferManagerDemo.Models;

namespace OfferManagerDemo.Services
{
    // Demo-Daten: bewusst klein gehalten, damit UI-Tests schnell bleiben.
    public class MockOfferService : IOfferService
    {
        private readonly List<Offer> _offers = new()
        {
            new Offer { Number = "OFF-2025-001", Customer = "Muster AG", Amount = 12450m, Status = "Open", CreatedAt = DateTime.Today.AddDays(-10) },
            new Offer { Number = "OFF-2025-002", Customer = "Globex GmbH", Amount = 8600m, Status = "Won", CreatedAt = DateTime.Today.AddDays(-7) },
            new Offer { Number = "OFF-2025-003", Customer = "ACME Corp", Amount = 4390m, Status = "Lost", CreatedAt = DateTime.Today.AddDays(-3) },
        };

        public Task<IReadOnlyList<Offer>> GetOffersAsync(string? search = null)
        {
            IEnumerable<Offer> q = _offers;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLowerInvariant();
                q = q.Where(o => o.Number.ToLower().Contains(s)
                              || o.Customer.ToLower().Contains(s)
                              || o.Status.ToLower().Contains(s));
            }
            return Task.FromResult<IReadOnlyList<Offer>>(q.OrderByDescending(o => o.CreatedAt).ToList());
        }

        public Task<Offer> CreateAsync(Offer offer)
        {
            offer.Id = Guid.NewGuid().ToString("N");
            offer.Number = offer.Number is { Length: > 0 } ? offer.Number : $"OFF-{DateTime.Now:yyyy}-{_offers.Count+1:000}";
            offer.CreatedAt = DateTime.Now;
            _offers.Add(offer);
            return Task.FromResult(offer);
        }

        public Task<Offer?> UpdateStatusAsync(string id, string status)
        {
            var o = _offers.FirstOrDefault(x => x.Id == id);
            if (o is null) return Task.FromResult<Offer?>(null);
            o.Status = status;
            return Task.FromResult<Offer?>(o);
        }
    }
}
