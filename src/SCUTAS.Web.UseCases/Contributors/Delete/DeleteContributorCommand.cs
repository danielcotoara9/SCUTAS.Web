using SCUTAS.Web.Core.ContributorAggregate;

namespace SCUTAS.Web.UseCases.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
