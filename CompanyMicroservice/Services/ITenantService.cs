using SharedMicroservice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyMicroservice.Services
{
    public interface ITenantService
    {
        TenantDTO ReturnByHostName(string hostName);
        TenantDTO ReturnById(int id);
        int Add(TenantDTO tenantDTO);
        void Alter(TenantDTO tenantDTO);
        void Remove(int id);
        IEnumerable<TenantDTO> ReturnAll();
    }
}
