using AutoMapper;
using CompanyMicroservice.Models;
using SharedMicroservice.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyMicroservice.MappingProfiles
{
    public class CompanyMappings: Profile
    {
        public CompanyMappings()
        {
            CreateMap<Company, CompanyDTO>().ReverseMap();
        }
    }
}
