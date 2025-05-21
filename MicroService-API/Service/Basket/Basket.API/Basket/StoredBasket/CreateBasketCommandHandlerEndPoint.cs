using Basket.API.Model;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.StoredBasket;

public record CreateBasket(ShoppingCart ShoppingCart);
public record CreateBasketResponse(string UserName);
public class CreateBasketCommandHandlerEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("StoredBasket123", async (CreateBasket request, ISender sender) =>
        {
            var command = request.Adapt<CreateBasketCommand>();
            var response = await sender.Send(command);
            var result = response.Adapt<CreateBasketResponse>();
            return Results.Created($"/products/{result.UserName}", result);
        })
             .WithName("CreateProduct")
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}


public class ABC
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public record RequestWrapper(ABC request);
public class test : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("testingSide", async (RequestWrapper request, ISender sender) =>
        {


        })
             .WithName("CreateProductABC");
    }
}

