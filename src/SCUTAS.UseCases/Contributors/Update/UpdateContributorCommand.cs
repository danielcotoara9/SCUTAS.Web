using SCUTAS.Core.ContributorAggregate;

namespace SCUTAS.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
