using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);
        Task<PaginatedResult<User>> GetUsersPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    }
}
