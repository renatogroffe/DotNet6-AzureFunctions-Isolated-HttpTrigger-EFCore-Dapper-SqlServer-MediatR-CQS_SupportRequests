using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MediatR;
using FunctionAppSupport.CQS.Commands;

namespace FunctionAppSupport;

public class CreateSupportRequest
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public CreateSupportRequest(ILoggerFactory loggerFactory,
        IMediator mediator)
    {
        _logger = loggerFactory.CreateLogger<CreateSupportRequest>();
        _mediator = mediator;
    }

    [Function(nameof(PostSupportRequest))]
    [OpenApiOperation(operationId: "SupportRequests", tags: new[] { "SupportRequests" }, Summary = "SupportRequests", Description = "Incluir Chamados de Suporte", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateSupportRequestCommand), Required = true, Description = "Objeto contendo os dados de um Chamado de Suporte")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateSupportRequestCommandResult), Summary = "Resultado da inclusao de um Chamado de Suporte", Description = "Resultado da inclusao de um Chamado de Suporte")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(CreateSupportRequestCommandResult), Summary = "Falha na inclusao de um Chamado de Suporte", Description = "Falha na inclusao de um Chamado de Suporte")]
    public async Task<HttpResponseData> PostSupportRequest([HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequestData req)
    {
        _logger.LogInformation("Iniciando a inclusao do chamado...");

        var requestCommand = await req.ReadFromJsonAsync<CreateSupportRequestCommand>();
        _logger.LogInformation(
            $"Dados recebidos: {JsonSerializer.Serialize(requestCommand)}");
        
        var result = await _mediator.Send(requestCommand);

        var response = req.CreateResponse();
        await response.WriteAsJsonAsync(result);
        if (result.Success)
            _logger.LogInformation(result.Message);
        else
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            _logger.LogError(result.Message);
        }
        return response;
    }
}