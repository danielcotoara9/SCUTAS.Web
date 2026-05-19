using MinimalSCUTAS.Web.Domain.ProductAggregate;

namespace MinimalSCUTAS.Web.ProductFeatures;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
