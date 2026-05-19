using SCUTAS.Core.ContributorAggregate;

namespace SCUTAS.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
