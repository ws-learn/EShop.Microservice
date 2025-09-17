using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

[Serializable]
public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid Id) : base("Product", Id)
    {
    }
}
