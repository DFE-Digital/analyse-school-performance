using ASP.Core.DTO;

namespace ASP.Application.Utilities;

public static class EducationPhase
{
    public static string GetPhaseOfEducation(IEducationPhase educationDetails)
    {
        var phaseOfEducation = "";
        if (educationDetails.IsPrimary ?? false) phaseOfEducation = "Primary";
        if (educationDetails.IsSecondary ?? false) phaseOfEducation = "Secondary";
        if (educationDetails.IsPost16 ?? false) phaseOfEducation = "16 to 18";
        return phaseOfEducation;
    }

}