using AutoMapper;
using UserMicroservice.Models;
using UserMicroservice.Repository;
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

namespace UserMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository userRepository, IMapper mapper, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _mapper = mapper;
           _encrypter = encrypter;
        }

        public IEnumerable<UserIndexDTO> ReturnAll(int tenantId)
        {
            var users = _userRepository.Get(a => a.TenantId == tenantId);
            List<UserIndexDTO> usersDTOs = _mapper.Map<List<UserIndexDTO>>(users.ToList());
            
            return usersDTOs;
        }

        public UserIndexDTO ReturnById(int id)
        {
            var user = _userRepository.GetById(id);
            return _mapper.Map<UserIndexDTO>(user);
        }

        public IEnumerable<UserIndexDTO> ReturnByEmail(string email, int tenantId)
        {
            var users = _userRepository.Get(a => a.Email.ToLower().Trim().Equals(email.ToLower())
                                            && a.TenantId == tenantId);
            List<UserIndexDTO> userDTOs = _mapper.Map<List<UserIndexDTO>>(users.ToList());

            return userDTOs;
        }

        public int Add(UserDTO userDTO)
        {
            var ret = ReturnByEmail(userDTO.Email, userDTO.TenantId);

            List<UserIndexDTO> userDTOs = ret.ToList();
            if (userDTOs.Count > 0)
                throw new Exception("User already exists with email[" + userDTO.Email + "]");

            User user = _mapper.Map<User>(userDTO);
            string salt = _encrypter.GetSalt();
            string password = _encrypter.GetHash(user.Password, salt);
            user.Password = password;

            //using (var scope = new TransactionScope()) //TransactionScopeAsyncFlowOption.Enabled))
            {
                _userRepository.Insert(user);//.ConfigureAwait(false);
                if (!_userRepository.Commit())
                    throw new Exception("Inserting a user failed on save.");
                //scope.Complete();
                return user.Id;
            }
        }


        public void Remove(int id)
        {
            _userRepository.Delete(id);
            if (!_userRepository.Commit())
                throw new Exception("Deleting a user failed on save.");
        }

        public void Alter(UserUpdateDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);
            if (user.Id != 0)
            {
                string salt = _encrypter.GetSalt();
                string password = _encrypter.GetHash(user.Password, salt);
                user.Password = password;

                using (var scope = new TransactionScope()) //TransactionScopeAsyncFlowOption.Enabled))
                {
                    _userRepository.Update(user);//.ConfigureAwait(false);
                    if (!_userRepository.Commit())
                        throw new Exception("Updating a user failed on save.");
                    scope.Complete();
                }
            }
            
        }

        public void IncreaseBids(UserAvailableBidDTO userDTO)
        {
            _userRepository.IncreaseDecreaseBids(userDTO.Id, userDTO.AvailableBids);
        }

        public void DecreaseBids(UserAvailableBidDTO userDTO)
        {
            _userRepository.IncreaseDecreaseBids(userDTO.Id, userDTO.AvailableBids*-1);
        }
    }
}
