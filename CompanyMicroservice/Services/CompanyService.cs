using AutoMapper;
using CompanyMicroservice.Models;
using CompanyMicroservice.Repository;
using SharedMicroservice.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CompanyMicroservice.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public IEnumerable<CompanyDTO> ReturnAll(int tenantId)
        {
            var companies = _companyRepository.Get(a => a.TenantId == tenantId);
            List<CompanyDTO> companyDTOs = _mapper.Map<List<CompanyDTO>>(companies.ToList());
            
            return companyDTOs;
        }

        public CompanyDTO ReturnById(int id)
        {
            var company =  _companyRepository.GetById(id);
            return _mapper.Map<CompanyDTO>(company);
        }

        public IEnumerable<CompanyDTO> ReturnByCNPJ(string cnpj, int tenantId)
        {
            var companies = _companyRepository.Get(a => a.CNPJ.ToLower().Trim().Equals(cnpj.ToLower().Trim()) &&
                                                 a.TenantId == tenantId);
            List<CompanyDTO> companiesDTOs = _mapper.Map<List<CompanyDTO>>(companies.ToList());

            return companiesDTOs;
        }

        public int Add(CompanyDTO companyDTO)
        {
            
            var ret = ReturnByCNPJ(companyDTO.CNPJ, companyDTO.TenantId);

            List<CompanyDTO> companiesDTOs = ret.ToList();
            if (companiesDTOs.Count > 0)
                throw new Exception("Company already exists with CNPJ [" + companyDTO.CNPJ + "]");

            Company company = _mapper.Map<Company>(companyDTO);

            //using (var scope = new TransactionScope())// TransactionScopeAsyncFlowOption.Enabled))
            {
                _companyRepository.Insert(company);//.ConfigureAwait(false);
                if (!_companyRepository.Commit())
                    throw new Exception("Inserting a company failed on save.");
                //scope.Complete();
                return company.Id;
            }
        }


        public void Remove(int id)
        {
            _companyRepository.Delete(id);
            if (!_companyRepository.Commit())
                throw new Exception("Deleting a company failed on save.");
        }

        public void Alter(CompanyDTO companyDTO)
        {
            Company company = _mapper.Map<Company>(companyDTO);
            if (company.Id != 0)
            {
                //using (var scope = new TransactionScope())// TransactionScopeAsyncFlowOption.Enabled))
                {
                    _companyRepository.Update(company);//.ConfigureAwait(false);
                    if (!_companyRepository.Commit())
                        throw new Exception("Updating a company failed on save.");
                    //scope.Complete();
                }
            }
            
        }

    }
}
