using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery
{
    public sealed record GetUsersQueryResponse
    {
        public List<UserDTO> Data { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
