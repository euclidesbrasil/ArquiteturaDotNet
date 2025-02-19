using AutoMapper;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;
using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery;

using Microsoft.Extensions.Configuration;
namespace ArquiteturaDesafio.Core.Application.UseCases.Mapper
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            //Adress
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<GeolocationDto, Geolocation>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Lat))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Long));

            CreateMap<Geolocation, GeolocationDto>()
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Longitude));

            //User
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, CreateUserResponse>();
            CreateMap<DeleteUserRequest, User>();
            CreateMap<User, DeleteUserRequest>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UpdateUserRequest>();
            CreateMap<User, GetUsersQueryResponse>();
            CreateMap<User, UserDTO>();
            CreateMap<UpdateUserRequest, UpdateUserResponse>();
        }
    }
}
