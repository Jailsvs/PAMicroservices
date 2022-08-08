using AutoMapper;
using SharedMicroservice.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SharedMicroservice.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using SharedMicroservice.Constants;
using Newtonsoft.Json;
using AuctionMicroservice.Repository;
using AuctionMicroservice.Models;
using System.Text;

namespace AuctionMicroservice.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private IMapper _mapper;

        public AuctionService(IAuctionRepository auctionRepository, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public IEnumerable<AuctionProductIndexDTO> ReturnAll(int tenantId)
        {
            var auctions = _auctionRepository.Get(a => a.TenantId == tenantId && a.Closed != "T");
            List<AuctionProductIndexDTO> auctionsDTOs = _mapper.Map<List<AuctionProductIndexDTO>>(auctions.ToList());       
            return auctionsDTOs;
        }

        public AuctionProductIndexDTO ReturnById(int id)
        {
            var auction = _auctionRepository.GetById(id);
            return _mapper.Map<AuctionProductIndexDTO>(auction);
        }

        
        public int Add(AuctionProductDTO auctionProductDTO)
        {
            if ((auctionProductDTO.Closed == null) ||
                (auctionProductDTO.Closed.Trim() == ""))
                auctionProductDTO.Closed = "F";

            AuctionProduct auction = _mapper.Map<AuctionProduct>(auctionProductDTO);
            if (auction.Closed.Trim().Equals(""))
                auction.Closed = "F";
            _auctionRepository.Insert(auction);
            if (!_auctionRepository.Commit())
                throw new Exception("Inserting a auction product failed on save.");
            return auction.Id;
        }


        public void Remove(int id)
        {
            _auctionRepository.Delete(id);
            if (!_auctionRepository.Commit())
                throw new Exception("Deleting a auction product failed on save.");
        }

        public void Alter(AuctionProductDTO auctionProductDTO)
        {
            AuctionProduct auctionProduct = _mapper.Map<AuctionProduct>(auctionProductDTO);
            if (auctionProduct.Id != 0)
            {
                _auctionRepository.Update(auctionProduct);
                if (!_auctionRepository.Commit())
                    throw new Exception("Updating a auction product failed on save.");
            }
            
        }

        public async Task<UserIndexDTO> GetUser(int userId)
        {
            UserIndexDTO user = null;

            HttpClient _httpClientUserSrv = new HttpClient();
            _httpClientUserSrv.BaseAddress = new Uri(ServiceConstants.USERSERVICEAPI_URL);
            _httpClientUserSrv.DefaultRequestHeaders.Accept.Clear();
            _httpClientUserSrv.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
          try
            {
                HttpResponseMessage response = await _httpClientUserSrv.GetAsync("api/User/" + userId);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<UserIndexDTO>(data);
                }
                return user;
            }
            catch (Exception)
            {
                return user;
            }
        }

        public async Task DecreaseUserBid(int userId)
        {
            HttpClient _httpClientUserSrvBid = new HttpClient();
            _httpClientUserSrvBid.BaseAddress = new Uri(ServiceConstants.USERSERVICEAPI_URL);
            _httpClientUserSrvBid.DefaultRequestHeaders.Accept.Clear();
            _httpClientUserSrvBid.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            var jsonString = "{\"Id\":" + userId + ",\"AvailableBids\":1}";
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var message = await _httpClientUserSrvBid.PutAsync("api/User/" + userId + "/Bid", httpContent);
            if (!message.IsSuccessStatusCode)
            {
                throw new Exception("Error Bidding");
            }

        }

        public void CloseAuction(AuctionProductClosedDTO auctionProductClosedDTO)
        {
            _auctionRepository.CloseAuction(auctionProductClosedDTO.Id);
        }

        public void BidAuction(AuctionBidDTO auctionBidDTO)
        {
            AuctionBid auctionBid = _mapper.Map<AuctionBid>(auctionBidDTO);
            auctionBid.BidDate = DateTime.Now;
            AuctionProduct auction = _auctionRepository.Find(auctionBidDTO.AuctionProductId);

            if (auction.Closed == "T")
                throw new Exception("Auction already closed!");

            BidStopwatch(new AuctionProductStopwatchBidDTO {
                Id = auctionBidDTO.AuctionProductId,
                StopwatchTime = auction.StopwatchTime,
                OpeningDate = auction.OpeningDate,
                AuctionProductId = auctionBidDTO.AuctionProductId,
                UserId = auctionBidDTO.UserId });
            _auctionRepository.InsertBid(auctionBid);
        }

        public void StartStopwatch(AuctionProductDTO auctionDTO)
        {
            HttpClient _httpClientStopWSrv = new HttpClient();
            _httpClientStopWSrv.BaseAddress = new Uri(ServiceConstants.STOPWATCHSERVICEAPI_URL);
            _httpClientStopWSrv.DefaultRequestHeaders.Accept.Clear();
            _httpClientStopWSrv.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            object data = new
            {
                Id = auctionDTO.Id,
                StopwatchTime = auctionDTO.StopwatchTime,
                OpeningDate = auctionDTO.OpeningDate
            };
           
            var httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var message = _httpClientStopWSrv.PostAsync("api/Stopwatch", httpContent).Result;
            if (!message.IsSuccessStatusCode)
            {
                throw new Exception("Error Start Auction Stopwatch");
            }
        }

        public void BidStopwatch(AuctionProductStopwatchBidDTO auctionDTO)
        {
            HttpClient _httpClientStopWSrv = new HttpClient();
            _httpClientStopWSrv.BaseAddress = new Uri(ServiceConstants.STOPWATCHSERVICEAPI_URL);
            _httpClientStopWSrv.DefaultRequestHeaders.Accept.Clear();
            _httpClientStopWSrv.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            object data = new
            {
                Id = auctionDTO.AuctionProductId,
                StopwatchTime = auctionDTO.StopwatchTime,
                OpeningDate = auctionDTO.OpeningDate,
                AuctionProductId = auctionDTO.AuctionProductId,
                UserId = auctionDTO.UserId
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var message = _httpClientStopWSrv.PostAsync("api/Stopwatch/"+ auctionDTO.AuctionProductId+"/Bid", httpContent).Result;
            if (!message.IsSuccessStatusCode)
            {
                throw new Exception("Error Bidding Auction Stopwatch");
            }
        }
    }
}
