namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name, List<string> Category,
    string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}


internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) :
    ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(command, cancellationToken);
        var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
        if (errors.Any())
        {
            throw new ValidationException(errors.FirstOrDefault());
        }

        // create Product Entity from command object
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // save to database
        session.Store(product);

        await session.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateProductResult(product.Id);

    }
}



