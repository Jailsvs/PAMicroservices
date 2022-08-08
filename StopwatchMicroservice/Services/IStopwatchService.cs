using SharedMicroservice.DTO;

namespace StopwatchMicroservice.Services
{
    public interface IStopwatchService
    {
        void Add(AuctionProductStopwatchDTO auctionDTO);
        void Bid(AuctionProductStopwatchBidDTO auctionBidDTO);
    }
}
