using SCUTAS.Web.Core.ContributorAggregate;

namespace SCUTAS.Web.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
