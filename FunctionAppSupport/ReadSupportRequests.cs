using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MediatR;
using FunctionAppSupport.CQS.Queries;

namespace FunctionAppSupport
{
    public class ReadSupportRequests
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ReadSupportRequests(ILoggerFactory loggerFactory,
            IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<ReadSupportRequests>();
            _mediator = mediator;
        }

        [Function(nameof(GetSupportRequests))]
        [OpenApiOperation(operationId: "SupportRequests", tags: new[] { "SupportRequests" }, Summary = "SupportRequests", Description = "Incluir Chamados de Suporte", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LastSupportRequestsQueryResult), Summary = "Ultimos Chamados de Suporte", Description = "Ultimos Chamados de Suporte")]
        public async Task<HttpResponseData> GetSupportRequests([HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequestData req)
        {
            _logger.LogInformation("Iniciando a consulta aos ultimos chamados...");

            var query = new LastSupportRequestsQuery()
            {
                NumberLastSupportRequests = Convert.ToInt32(
                    Environment.GetEnvironmentVariable("NumberLastSupportRequests"))
            };

            var result = await _mediator.Send(query);

            _logger.LogInformation(
                $"No. de registros encontrados: {result.Count()} | " +
                $"Qtde. maxima retornada por pesquisa: {query.NumberLastSupportRequests}");

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}