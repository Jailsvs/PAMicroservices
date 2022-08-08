using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedMicroservice.DTO;
using StopwatchMicroservice.Services;
using System.Threading.Tasks;

namespace StopwatchMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class StopwatchController : ControllerBase
    {
        private readonly IStopwatchService _stopwatchService;
        private readonly IHubContext<StopwatchHub> _hubContext;

        public StopwatchController(IStopwatchService stopwatchService, IHubContext<StopwatchHub> hubContext)
        {
            _stopwatchService = stopwatchService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new OkResult();
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuctionProductStopwatchDTO auctionDTO)
        {
            _stopwatchService.Add(auctionDTO);
            return new OkResult();
        }

        [Route("{auctionId}/Bid")]
        [HttpPost]
        public IActionResult PostBid(int auctionId, [FromBody] AuctionProductStopwatchBidDTO auctionBidDTO)
        {
            _stopwatchService.Bid(auctionBidDTO);
            return new OkResult();
        }

        [Route("{auctionId}/SendMessage")]
        [HttpPost]
        public async Task<IActionResult> PostMessage(int auctionId, [FromBody] AuctionProductStopwatchTimeDTO auctionTimeDTO)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTime", auctionTimeDTO.AuctionProductId, auctionTimeDTO.StopwatchTimeCounter);
            return new OkResult();
        }
    }
}
