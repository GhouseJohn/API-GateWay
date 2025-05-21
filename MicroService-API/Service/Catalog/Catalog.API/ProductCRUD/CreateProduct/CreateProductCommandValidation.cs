using BuildingService.CQRS;
using Catalog.API.Model;
using FluentValidation;
using Marten;


namespace Catalog.API.ProductCRUD.CreateProduct;
public record CreateProductCommand(string Name, List<string> Category,
        string Description, string ImageFile, decimal Price) : ICommand<CreateProductCommandResponse>;

public record CreateProductCommandResponse(Guid Id);

public class productValidation : AbstractValidator<CreateProductCommand>
{
    public productValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
public class CreateProductCommandValidation(IDocumentSession session)
            : ICommandHandler<CreateProductCommand, CreateProductCommandResponse>
{
    public async Task<CreateProductCommandResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var _product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        session.Store(_product);
        await session.SaveChangesAsync(cancellationToken);
        return new CreateProductCommandResponse(_product.Id);
    }
}

