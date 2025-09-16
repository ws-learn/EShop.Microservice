using System;

namespace Catalog.API.Exceptions;

[Serializable]
public class ProductNotFoundException : Exception
{
    public ProductNotFoundException() : base("Product Not Found!")
    {
    }

    public ProductNotFoundException(string? message) : base(message)
    {
    }

    public ProductNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}