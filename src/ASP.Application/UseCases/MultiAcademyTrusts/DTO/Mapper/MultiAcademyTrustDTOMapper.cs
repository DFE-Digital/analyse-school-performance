namespace ASP.Application.UseCases.MultiAcademyTrusts.DTO.Mapper;

public static class MultiAcademyTrustDTOMapper
{
    public static MultiAcademyTrustDTO MapToMultiAcademyTrustDTO(this ASP.Core.MultiAcademyTrusts.MultiAcademyTrust multiAcademyTrust)
    {
        return new MultiAcademyTrustDTO(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }
}