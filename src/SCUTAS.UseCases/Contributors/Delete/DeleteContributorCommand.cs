using SCUTAS.Core.ContributorAggregate;

namespace SCUTAS.UseCases.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
