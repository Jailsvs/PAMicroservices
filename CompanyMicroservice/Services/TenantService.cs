using AutoMapper;
using CompanyMicroservice.Models;
using CompanyMicroservice.Repository;
using SharedMicroservice.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace CompanyMicroservice.Services
{
    public class TenantService: ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private IMapper _mapper;

        public TenantService(ITenantRepository tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public TenantDTO ReturnById(int id)
        {
            var tenant =  _tenantRepository.GetById(id);
            return _mapper.Map<TenantDTO>(tenant);
        }

        public TenantDTO ReturnByHostName(string hostName)
        {
            var tenants = _tenantRepository.Get(a => a.Host.ToLower().Trim().Equals(hostName.ToLower()));
            List<TenantDTO> tenantDTOs = _mapper.Map<List<TenantDTO>>(tenants.ToList());

            if (tenantDTOs.Count == 0)
                return null;
            else
                return tenantDTOs[0];
        }

        public int Add(TenantDTO tenantDTO)
        {
            var ret = ReturnByHostName(tenantDTO.Host);

            if (ret != null)
                throw new Exception("Tenant already exists with host name["+ tenantDTO.Host + "]");

            Tenant tenant = _mapper.Map<Tenant>(tenantDTO);

            //using (var scope = new TransactionScope()) //TransactionScopeAsyncFlowOption.Enabled))
            {
                _tenantRepository.Insert(tenant);//.ConfigureAwait(false);
                if (!_tenantRepository.Commit())
                    throw new Exception("Inserting a tenant failed on save.");
                //scope.Complete();
                return tenant.Id;
            }
        }


        public void Remove(int id)
        {
            _tenantRepository.Delete(id);
            if (!_tenantRepository.Commit())
                throw new Exception("Deleting a tenant failed on save.");
        }

        public void Alter(TenantDTO tenantDTO)
        {
            Tenant tenant = _mapper.Map<Tenant>(tenantDTO);
            if (tenant.Id != 0)
            {
                //using (var scope = new TransactionScope()) // TransactionScopeAsyncFlowOption.Enabled))
                {
                    _tenantRepository.Update(tenant);//.ConfigureAwait(false);
                    if (!_tenantRepository.Commit())
                        throw new Exception("Updating a tenant failed on save.");
                    //scope.Complete();
                }
            }
        }

        public IEnumerable<TenantDTO> ReturnAll()
        {
            var tenants = _tenantRepository.GetAllList();
            List<TenantDTO> tenantDTOs = _mapper.Map<List<TenantDTO>>(tenants.ToList());

            return tenantDTOs;
        }

    }
}
