using System.Collections.Generic;
using System.Threading.Tasks;
using OfferManagerDemo.Models;

namespace OfferManagerDemo.Services
{
    public interface IOfferService
    {
        Task<IReadOnlyList<Offer>> GetOffersAsync(string? search = null);
        Task<Offer> CreateAsync(Offer offer);
        Task<Offer?> UpdateStatusAsync(string id, string status);
    }
}
