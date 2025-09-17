using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

[Serializable]
public class ProductNotFoundException : NotFoundExceptions
{
    public ProductNotFoundException(Guid Id) : base("Product", Id)
    {
    }
}
