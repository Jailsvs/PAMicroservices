using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AuctionMicroservice.Models;
using AuctionMicroservice.DBContexts;

namespace AuctionMicroservice.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionContext _dbContext;

        public AuctionRepository(AuctionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Commit()
        {
            int changeCount = _dbContext.SaveChanges();
            return changeCount >= 0;
        }

        public int Count()
        {
            return _dbContext.Tb_AuctionProduct.Count();
        }

        public void Delete(Func<AuctionProduct, bool> predicate)
        {
            _dbContext.Set<AuctionProduct>()
            .Where(predicate).ToList()
            .ForEach(del => _dbContext.Set<AuctionProduct>().Remove(del));
        }

        public void Delete(int id)
        {
            var auction = _dbContext.Tb_AuctionProduct.Find(id);
            if (auction == null)
                throw new Exception("Auction Product not found");
            _dbContext.Tb_AuctionProduct.Remove(auction);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        public AuctionProduct Find(params object[] key)
        {
            return _dbContext.Set<AuctionProduct>().Find(key);
        }

        public AuctionProduct First(Expression<Func<AuctionProduct, bool>> predicate)
        {
            return _dbContext.Set<AuctionProduct>().Where(predicate).FirstOrDefault();
        }

        public IQueryable<AuctionProduct> Get(Expression<Func<AuctionProduct, bool>> predicate)
        {
            return _dbContext.Set<AuctionProduct>().Where(predicate);
        }

        public IQueryable<AuctionProduct> GetAll()
        {
            return _dbContext.Set<AuctionProduct>();
        }

        public IEnumerable<AuctionProduct> GetAllList()
        {
            return _dbContext.Tb_AuctionProduct.ToList();
        }

        public AuctionProduct GetById(int id)
        {
            return _dbContext.Tb_AuctionProduct.Find(id);
        }

        public void Insert(AuctionProduct entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(AuctionProduct entity)
        {
            var auction = _dbContext.Tb_AuctionProduct.Find(entity.Id);
            if (auction == null)
                throw new Exception("Aucion Product not found");

            if ((entity.BidAmount != 0) &&
                (entity.BidValue != 0) &&
               (entity.Closed != null) &&
               (entity.CompanyId != 0) &&
               (entity.CurrentValue != 0) &&
               (entity.Description != null) &&
               (entity.LastBidDate != null) &&
               (entity.MinValue != 0) &&
               (entity.OpeningDate != null) &&
               (entity.StopwatchTime != 0) &&
               (entity.URLDescExt != null) &&
               (entity.URLImg != null) &&
               (entity.WinnerAuctionUserId != 0))
                _dbContext.Entry(entity).State = EntityState.Modified;
            else
            {
                if (entity.BidAmount != 0)
                    _dbContext.Entry(entity).Property("BidAmount").IsModified = true;

                if (entity.Closed != null)
                    _dbContext.Entry(entity).Property("Closed").IsModified = true;

                if (entity.CompanyId != 0)
                    _dbContext.Entry(entity).Property("CompanyId").IsModified = true;

                if (entity.CurrentValue != 0)
                    _dbContext.Entry(entity).Property("CurrentValue").IsModified = true;

                if (entity.Description != null)
                    _dbContext.Entry(entity).Property("Description").IsModified = true;

                if (entity.LastBidDate != null)
                    _dbContext.Entry(entity).Property("LastBidDate").IsModified = true;

                if (entity.MinValue != 0)
                    _dbContext.Entry(entity).Property("MinValue").IsModified = true;

                if (entity.OpeningDate != null)
                    _dbContext.Entry(entity).Property("OpeningDate").IsModified = true;

                if (entity.StopwatchTime != 0)
                    _dbContext.Entry(entity).Property("StopwatchTime").IsModified = true;

                if (entity.URLDescExt != null)
                    _dbContext.Entry(entity).Property("URLDescExt").IsModified = true;

                if (entity.URLImg != null)
                    _dbContext.Entry(entity).Property("URLImg").IsModified = true;

                if (entity.WinnerAuctionUserId != 0)
                    _dbContext.Entry(entity).Property("WinnerAuctionUserId").IsModified = true;

                if (entity.BidValue != 0)
                    _dbContext.Entry(entity).Property("BidValue").IsModified = true;
            }
        }

        public void CloseAuction(int auctionId)
        {
            //var commandText = "UPDATE TB_AUCTIONPRODUCT SET CLOSED = 'T', CURRENTVALUE = COALESCE(BIDVALUE, 0) * COALESCE(BIDAMOUNT, 0) WHERE ID = " + auctionId;
            //_dbContext.Database.ExecuteSqlCommand(commandText);
            AuctionProduct auction = _dbContext.Tb_AuctionProduct.Find(auctionId);
            if (auction != null)
            {
                auction.Closed = "T";
                auction.CurrentValue = auction.BidAmount * (auction.BidValue != 0 ? auction.BidValue : 1);
                _dbContext.Entry(auction).Property("Closed").IsModified = true;
                _dbContext.Entry(auction).Property("BidAmount").IsModified = true;
            }
            Commit();
        }

        public void InsertBid(AuctionBid entity)
        {
            _dbContext.Tb_AuctionBid.Add(entity);

            AuctionProduct auction = _dbContext.Tb_AuctionProduct.Find(entity.AuctionProductId);
            if (auction != null)
            {
                auction.BidAmount += 1;
                auction.CurrentValue = auction.BidAmount * (auction.BidValue != 0 ? auction.BidValue:1);
                auction.WinnerAuctionUserId = entity.UserId;
                auction.LastBidDate = entity.BidDate;
                
                _dbContext.Entry(auction).Property("BidAmount").IsModified = true;
                _dbContext.Entry(auction).Property("CurrentValue").IsModified = true;
                _dbContext.Entry(auction).Property("WinnerAuctionUserId").IsModified = true;
                _dbContext.Entry(auction).Property("LastBidDate").IsModified = true;

            }

            //var commandText = "UPDATE TB_AUCTIONPRODUCT SET CLOSED = 'T' WHERE ID = " + entity.AuctionProductId;
            //_dbContext.Database.ExecuteSqlCommand(commandText);
            Commit();
        }
    }

}
