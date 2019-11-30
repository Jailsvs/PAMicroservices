using SharedMicroservice.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserMicroservice.Services
{
    public interface IUserService
    {
        UserIndexDTO ReturnById(int id);
        IEnumerable<UserIndexDTO> ReturnByEmail(string email, int tenandId);
        int Add(UserDTO userDTO);
        void IncreaseBids(UserAvailableBidDTO userDTO);
        void DecreaseBids(UserAvailableBidDTO userDTO);

        void Alter(UserUpdateDTO userDTO);
        void Remove(int id);
        IEnumerable<UserIndexDTO> ReturnAll(int TenantId);
        //Task<TenantDTO> GetTenant(string hostName);
    }
}
