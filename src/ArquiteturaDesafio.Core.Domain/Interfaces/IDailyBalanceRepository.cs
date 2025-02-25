﻿using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.Interfaces
{
    public interface IDailyBalanceRepository : IBaseRepository<DailyBalance>
    {
        Task<DailyBalance> GetByDateAsync(DateTime date);
    }
}
