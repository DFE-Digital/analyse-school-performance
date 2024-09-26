using ASP.Core.Optionality;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public record GetAvailableSchoolDownloadsRequest(string Urn, Optional<int> Year);
}
