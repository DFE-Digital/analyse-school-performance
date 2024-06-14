using ASP.Core.Helpers;

namespace ASP.Core.DTO.Establishment;

public class AddressDTO
{
    public string Street { get; set; }
    public string Town { get; set; }
    public string PostCode { get; set; }
    
    public override string ToString()
    {
        return StringHelper.ConcatNonEmpties(", ", Street, Town, PostCode);
    }
}