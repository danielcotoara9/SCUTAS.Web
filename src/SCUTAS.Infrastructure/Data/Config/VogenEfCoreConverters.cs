using SCUTAS.Core.ContributorAggregate;
using Vogen;

namespace SCUTAS.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
