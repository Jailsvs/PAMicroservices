using SharedMicroservice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionMicroservice.Services
{
    public interface IAuctionService
    {
        AuctionProductIndexDTO ReturnById(int id);
        int Add(AuctionProductDTO userDTO);
        void Alter(AuctionProductDTO userDTO);
        void Remove(int id);
        IEnumerable<AuctionProductIndexDTO> ReturnAll(int TenantId);
        Task<UserIndexDTO> GetUser(int userId);
        Task DecreaseUserBid(int userId);
        void CloseAuction(AuctionProductClosedDTO auctionProductClosedDTO);
        void BidAuction(AuctionBidDTO auctionBidDTO);
        void StartStopwatch(AuctionProductDTO auctionDTO);
        void BidStopwatch(AuctionProductStopwatchBidDTO auctionDTO);
    }
}
