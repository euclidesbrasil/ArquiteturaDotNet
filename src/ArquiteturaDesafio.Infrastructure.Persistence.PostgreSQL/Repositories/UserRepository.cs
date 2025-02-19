using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Extensions;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }
        public async Task<User?> GetByLoginAsync(string login)
        {
            var user = await _context.Users.Where(x => x.Username == login).FirstOrDefaultAsync();
            return user;
        }

        public async Task<PaginatedResult<User>> GetUsersPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken)
        {
            var query = _context.Users.Where(x => true);
            query = query.ApplyFilters(paginationQuery.Filter);
            paginationQuery.Order = paginationQuery.Order ?? "id asc";
            query = query.OrderBy(paginationQuery.Order);

            var totalCount = await query.CountAsync(cancellationToken); // Total de itens sem paginação
            var items = await query
                .Skip(paginationQuery.Skip)
                .Take(paginationQuery.Size)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<User>
            {
                Data = items,
                TotalItems = totalCount,
                CurrentPage = paginationQuery.Page
            };
        }

    }
}
