using MinimalSCUTAS.Web.Web.Domain.ProductAggregate;

namespace MinimalSCUTAS.Web.Web.ProductFeatures;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
