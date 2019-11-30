using AuctionMicroservice.Models;
using AutoMapper;
using SharedMicroservice.DTO;

namespace UserMicroservice.MappingProfiles
{
    public class AuctionProductMappings : Profile
    {
        public AuctionProductMappings()
        {
            CreateMap<AuctionProduct, AuctionProductDTO>().ReverseMap();
            CreateMap<AuctionProduct, AuctionProductIndexDTO>().ReverseMap();
            CreateMap<AuctionProduct, AuctionProductClosedDTO>().ReverseMap();
            CreateMap<AuctionBid, AuctionBidDTO>().ReverseMap();
        }
    }
}
