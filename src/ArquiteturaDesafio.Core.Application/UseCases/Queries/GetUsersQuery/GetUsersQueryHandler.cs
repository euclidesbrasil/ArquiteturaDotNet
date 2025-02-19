using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Common;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery
{ 
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQueryRequest, GetUsersQueryResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetUsersQueryResponse> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var sales = await _userRepository.GetUsersPagination(new PaginationQuery()
            {
                Order = request.order,
                Page = request.page,
                Size = request.size,
                Filter = request.filters
            }, cancellationToken);

            List<UserDTO> itensReturn = new List<UserDTO>();
            foreach(var item in sales.Data)
            {
                itensReturn.Add(_mapper.Map<UserDTO>(item));
            }

            return new GetUsersQueryResponse()
            {
                Data = itensReturn,
                CurrentPage = sales.CurrentPage,
                TotalItems = sales.TotalItems,
                TotalPages = sales.TotalPages
            };
        }
    }
}
