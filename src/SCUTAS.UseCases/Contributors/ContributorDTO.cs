using SCUTAS.Core.ContributorAggregate;

namespace SCUTAS.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
