using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Dapper;

namespace FunctionAppSupport.CQS.Queries
{
    public class LastSupportRequestsQueryHandler :
        IRequestHandler<LastSupportRequestsQuery, IEnumerable<LastSupportRequestsQueryResult>>
    {
        public async Task<IEnumerable<LastSupportRequestsQueryResult>> Handle(LastSupportRequestsQuery request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                Environment.GetEnvironmentVariable("DBSupportDapper"));
            
            connection.Open();
            var result = await connection.QueryAsync<LastSupportRequestsQueryResult>(
                "SELECT TOP (@NumberLastSupportRequests) * " +
                "FROM dbo.SupportRequests " +
                "ORDER BY Id DESC",
                new { NumberLastSupportRequests = request.NumberLastSupportRequests });
            connection.Close();

            return result;
        }
    }
}