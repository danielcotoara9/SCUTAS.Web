using MinimalSCUTAS.Web.Domain.CartAggregate;
using MinimalSCUTAS.Web.Domain.GuestUserAggregate;
using MinimalSCUTAS.Web.Domain.OrderAggregate;
using MinimalSCUTAS.Web.Domain.ProductAggregate;
using Vogen;

namespace MinimalSCUTAS.Web.Infrastructure.Data.Config;

[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
[EfCoreConverter<CartItemId>]
[EfCoreConverter<GuestUserId>]
[EfCoreConverter<OrderId>]
[EfCoreConverter<OrderItemId>]
[EfCoreConverter<Quantity>]
[EfCoreConverter<Price>]
internal partial class VogenEfCoreConverters;
