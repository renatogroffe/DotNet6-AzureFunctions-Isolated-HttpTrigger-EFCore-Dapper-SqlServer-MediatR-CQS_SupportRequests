using System.Collections.Generic;
using MediatR;

namespace FunctionAppSupport.CQS.Queries
{
    public class LastSupportRequestsQuery :
        IRequest<IEnumerable<LastSupportRequestsQueryResult>>
    {
        public int NumberLastSupportRequests { get; set; }
    }
}