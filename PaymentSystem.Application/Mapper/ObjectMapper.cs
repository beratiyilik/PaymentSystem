using AutoMapper;
using PaymentSystem.Application.Models;
using PaymentSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Mapper
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ProjectNameDTOMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }

    public class ProjectNameDTOMapper : Profile
    {
        public ProjectNameDTOMapper()
        {
            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<AccrualLoan, AccrualLoanModel>().ReverseMap();

            CreateMap<CollectionRecord, CollectionRecordModel>().ReverseMap();
        }
    }
}
