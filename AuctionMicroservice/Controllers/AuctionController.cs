using Microsoft.AspNetCore.Mvc;
using SharedMicroservice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AuctionMicroservice.Services;

namespace AuctionMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        
        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int tenantId)
        {
   
            var auctions = _auctionService.ReturnAll(tenantId);
            return new OkObjectResult(auctions);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var auction = _auctionService.ReturnById(id);
            if (auction == null)
                return new NotFoundResult();

            return new OkObjectResult(auction);
        }


        [HttpPost]
        public IActionResult Post([FromBody] AuctionProductDTO auctionProductDTO, [FromQuery] int tenantId)
        {

            auctionProductDTO.TenantId = tenantId;
            int dto_id = _auctionService.Add(auctionProductDTO);
            auctionProductDTO.Id = dto_id;
            _auctionService.StartStopwatch(auctionProductDTO);
            return CreatedAtAction(nameof(Get), new { id = dto_id });
        }

        [Route("{id}/Close")]
        [HttpPost]
        public IActionResult PostClose(int id, [FromBody] AuctionProductClosedDTO auctionProductDTO)
        {
            auctionProductDTO.Id = id;
            _auctionService.CloseAuction(auctionProductDTO);
            return new OkResult();
        }

        [Route("{id}/Bid")]
        [HttpPost]
        public async Task<IActionResult> PostBid(int id, [FromBody] AuctionBidDTO auctionBidDTO, [FromQuery] int tenantId)
        {
            AuctionProductIndexDTO auction = _auctionService.ReturnById(id);
            UserIndexDTO user = await _auctionService.GetUser(auctionBidDTO.UserId);

            if ((user == null) || (user.AvailableBids <= 0))
                throw new Exception("User wihthout bids available");

            //await _auctionService.DecreaseUserBid(auctionBidDTO.UserId);
            _ = _auctionService.DecreaseUserBid(auctionBidDTO.UserId);

            auctionBidDTO.TenantId = tenantId != 0 ? tenantId : user.TenantId;;
            auctionBidDTO.AuctionProductId = id;
            _auctionService.BidAuction(auctionBidDTO);
            return new OkResult();
        }


        [HttpPut]
        public IActionResult Put([FromBody] AuctionProductDTO auctionProductDTO)
        {
            if (auctionProductDTO != null)
            {
                _auctionService.Alter(auctionProductDTO);
                return new OkResult();
            }
            return new NoContentResult();
        }

      

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _auctionService.Remove(id);
            return new OkResult();
        }
    }
}
