using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using ArquiteturaDesafio.Core.Domain.Enum;
using System.Threading;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetDailyReportQuery
{
    public class GetDailyReportQueryHandler : IRequestHandler<GetDailyReportQueryRequest, GetDailyReportQueryResponse>
    {

        private readonly IDailyBalanceReportRepository _repositoryNotRelational;
        private readonly IDailyBalanceRepository _repositoryRelational;
        private readonly IMapper _mapper;

        public GetDailyReportQueryHandler(IDailyBalanceReportRepository repositoryNotRelational, IDailyBalanceRepository repositoryRelational, IMapper mapper)
        {
            _repositoryNotRelational = repositoryNotRelational;
            _repositoryRelational = repositoryRelational;
            _mapper = mapper;
        }


        public async Task<GetDailyReportQueryResponse> Handle(GetDailyReportQueryRequest request, CancellationToken cancellationToken)
        {
            if(request.Type == DatabaseType.Mongo)
            {
                return (await getDataFromNotRelationalDB(request, cancellationToken));
            }

            if (request.Type == DatabaseType.Postgree)
            {
                return (await getDataFromRelationalDB(request, cancellationToken));
            }

            throw new InvalidOperationException($"Tipo inválido para Type de banco de dados. Value: {request.Type}");
        }

        private async Task< GetDailyReportQueryResponse> getDataFromRelationalDB(GetDailyReportQueryRequest request, CancellationToken cancellationToken)
        {
            var filterDailysBalance = await _repositoryRelational.Filter(x => x.Date == request.Date, cancellationToken);

            List<TransactionQueryBaseDTO> itensReturn = new List<TransactionQueryBaseDTO>();
            var dailyBalance = filterDailysBalance.FirstOrDefault();

            // Caso não exista, instancia o objeto para inserir
            dailyBalance = dailyBalance ?? new Core.Domain.Entities.DailyBalance(request.Date.Value, new Money(0));
            return _mapper.Map<GetDailyReportQueryResponse>(dailyBalance);
        }

        private async Task<GetDailyReportQueryResponse> getDataFromNotRelationalDB(GetDailyReportQueryRequest request, CancellationToken cancellationToken)
        {
            var filterDailysBalance = await _repositoryNotRelational.Filter(x => x.Date == request.Date, cancellationToken);

            List<TransactionQueryBaseDTO> itensReturn = new List<TransactionQueryBaseDTO>();
            var dailyBalance = filterDailysBalance.FirstOrDefault();

            // Caso não exista, instancia o objeto para inserir
            dailyBalance = dailyBalance ?? new Core.Domain.Entities.DailyBalanceReport(request.Date.Value, new Money(0));
            return _mapper.Map<GetDailyReportQueryResponse>(dailyBalance);
        }

    }
}
