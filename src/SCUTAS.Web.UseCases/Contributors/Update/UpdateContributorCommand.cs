using SCUTAS.Web.Core.ContributorAggregate;

namespace SCUTAS.Web.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
