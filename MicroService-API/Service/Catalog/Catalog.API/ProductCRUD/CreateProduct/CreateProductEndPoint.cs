using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.ProductCRUD.CreateProduct;
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);

public class CreateProductEndPoint(ILogger<CreateProductEndPoint> logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Createproducts", async (CreateProductRequest request, ISender sender) =>
      {
          logger.LogInformation("Im Logging....");
          logger.LogError("Im FRom  Basket.. Testing");
          var command = request.Adapt<CreateProductCommand>();
          var result = await sender.Send(command);
          var response = result.Adapt<CreateProductResponse>();
          return Results.Created($"/products/{response.Id}", response);
      })
              .WithName("GenerateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}

