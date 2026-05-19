using SCUTAS.Web.Core.ContributorAggregate;

namespace SCUTAS.Web.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
