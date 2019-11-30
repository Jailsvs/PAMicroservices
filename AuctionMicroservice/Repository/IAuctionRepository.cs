using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharedMicroservice.Repository;
using AuctionMicroservice.Models;

namespace AuctionMicroservice.Repository
{
    public interface IAuctionRepository: IRepository<AuctionProduct>
    {
        void CloseAuction(int auctionId);
        void InsertBid(AuctionBid entity);

    }
}
