using Basket.API.Model;
using BuildingService.CQRS;
using Marten;

namespace Basket.API.Basket.GetBasket;
public record GetBasketQueryRequest(string UserName) : IQuery<GetBasketCommandResponse>;
public record GetBasketCommandResponse(IEnumerable<ShoppingCart> ShoppingCart);
public class GetBasketQueryHandler(IDocumentSession Session) : IQueryHandler<GetBasketQueryRequest, GetBasketCommandResponse>
{
    public async Task<GetBasketCommandResponse> Handle(GetBasketQueryRequest userName, CancellationToken cancellationToken)
    {
        try
        {
            var cartItems = await Session
                     .Query<ShoppingCart>()
                     .Where(x => x.UserName == userName.UserName)
                     .ToListAsync(cancellationToken);

            return new GetBasketCommandResponse(cartItems);
        }
        catch (Exception ex)
        {

            throw;
        }

    }
}

