using Basket.API.Model;
using BuildingService.CQRS;
using FluentValidation;
using Marten;

namespace Basket.API.Basket.StoredBasket;

public record CreateBasketCommand(ShoppingCart ShoppingCart) : ICommand<CreateBasketCommandResponse>;
public record CreateBasketCommandResponse(string userName);

public class createBasketCommandValidation : AbstractValidator<CreateBasketCommand>
{
    public createBasketCommandValidation()
    {
        RuleFor(x => x.ShoppingCart.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");
        RuleFor(x => x.ShoppingCart.Items)
            .NotEmpty()
            .WithMessage("Items are required");
        RuleFor(x => x.ShoppingCart.TotalPrice)
            .GreaterThan(0)
            .WithMessage("TotalPrice must be greater than 0");
    }
}
public class CreateBasketCommandHandler(IDocumentSession session) : ICommandHandler<CreateBasketCommand, CreateBasketCommandResponse>
{
    public async Task<CreateBasketCommandResponse> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {

        try
        {
            session.Store(request.ShoppingCart);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateBasketCommandResponse(request.ShoppingCart.UserName);
        }
        catch (Exception ex)
        {

            throw;
        }

    }
}


