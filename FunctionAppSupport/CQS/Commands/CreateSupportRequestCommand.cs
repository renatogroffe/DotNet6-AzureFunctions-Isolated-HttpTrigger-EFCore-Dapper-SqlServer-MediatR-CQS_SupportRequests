using MediatR;

namespace FunctionAppSupport.CQS.Commands;

public class CreateSupportRequestCommand : IRequest<CreateSupportRequestCommandResult>
{
    public string Email { get; set; }
    public string Problem { get; set; }
}