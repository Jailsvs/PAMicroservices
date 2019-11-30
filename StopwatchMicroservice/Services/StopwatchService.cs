using SharedMicroservice.DTO;
using StopwatchMicroservice.Tasks;
using System.Collections.Concurrent;

namespace StopwatchMicroservice.Services
{
    public class StopwatchService : IStopwatchService
    {
        private readonly ConcurrentDictionary<int, StopwatchAuction> _stopwatchs;
        
        public StopwatchService(ConcurrentDictionary<int, StopwatchAuction> stopwatchs)
        {
            _stopwatchs = stopwatchs;
        }

        public void Add(AuctionProductStopwatchDTO auctionDTO)
        {
            if (!_stopwatchs.ContainsKey(auctionDTO.Id))
            {
                InternalAdd(auctionDTO);
            }
        }

        public void Bid(AuctionProductStopwatchBidDTO auctionBidDTO)
        {
            
            if (!_stopwatchs.ContainsKey(auctionBidDTO.AuctionProductId))
            {
                InternalAdd(new AuctionProductStopwatchDTO { Id = auctionBidDTO.AuctionProductId,
                    StopwatchTime = auctionBidDTO.StopwatchTime,
                    OpeningDate = auctionBidDTO.OpeningDate});
            }
            InternalBid(auctionBidDTO);
            
        }

        private void InternalAdd(AuctionProductStopwatchDTO auctionDTO)
        {
            StopwatchAuction s = new StopwatchAuction(auctionDTO.Id, auctionDTO.StopwatchTime, auctionDTO.OpeningDate, _stopwatchs);
            _stopwatchs.TryAdd(auctionDTO.Id, s);
        }

        private void InternalBid(AuctionProductStopwatchBidDTO auctionBidDTO)
        {
            StopwatchAuction s;
            _stopwatchs.TryGetValue(auctionBidDTO.AuctionProductId, out s);
            if (s != null)
            {
                s.Stop();
                s.Restart();
                s.Start();
            }
        }
    }
}
