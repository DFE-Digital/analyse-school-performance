namespace ASP.Domain.MultiAcademyTrusts.UseCases.DTO.Mapper;

public static class MultiAcademyTrustDTOMapper
{
    public static MultiAcademyTrustDTO MapToMultiAcademyTrustDTO(this ASP.Domain.MultiAcademyTrusts.MultiAcademyTrust multiAcademyTrust)
    {
        return new MultiAcademyTrustDTO(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }
}