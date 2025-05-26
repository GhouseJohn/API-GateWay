using Basket.API.Model;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.GetBasket;
public record GetShopingDaat(string UserName);
public record GetShopingResponse(IEnumerable<ShoppingCart> ShoppingCart);

public class GetBasketQueryHandlerEndPoint(ILogger<GetBasketQueryHandlerEndPoint> _logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("GetShopingData", async ([AsParameters] GetShopingDaat request, ISender sender) =>
        {
            _logger.LogInformation("Hello Im From Basket GetData");
            var command = request.Adapt<GetBasketQueryRequest>();
            var response = await sender.Send(command);
            var result = response.Adapt<GetShopingResponse>();
            return Results.Ok(result);
        })
             .WithName("GetData");
    }
}

