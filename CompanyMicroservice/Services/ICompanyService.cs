using SharedMicroservice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyMicroservice.Services
{
    public interface ICompanyService
    {
        CompanyDTO ReturnById(int id);
        IEnumerable<CompanyDTO> ReturnByCNPJ(string cnpj, int tenantId);
        int Add(CompanyDTO companyDTO);
        void Alter(CompanyDTO companyDTO);
        void Remove(int id);
        IEnumerable<CompanyDTO> ReturnAll(int TenantId);
    }
}
