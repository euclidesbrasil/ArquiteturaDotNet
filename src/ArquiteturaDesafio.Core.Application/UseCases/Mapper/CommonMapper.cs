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
