using MinimalSCUTAS.Web.Web.Domain.CartAggregate;
using MinimalSCUTAS.Web.Web.Domain.GuestUserAggregate;
using MinimalSCUTAS.Web.Web.Domain.OrderAggregate;
using MinimalSCUTAS.Web.Web.Domain.ProductAggregate;
using Vogen;

namespace MinimalSCUTAS.Web.Web.Infrastructure.Data.Config;

[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
[EfCoreConverter<CartItemId>]
[EfCoreConverter<GuestUserId>]
[EfCoreConverter<OrderId>]
[EfCoreConverter<OrderItemId>]
[EfCoreConverter<Quantity>]
[EfCoreConverter<Price>]
internal partial class VogenEfCoreConverters;
