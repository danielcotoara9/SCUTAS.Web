using SCUTAS.Web.Core.ContributorAggregate;
using Vogen;

namespace SCUTAS.Web.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
