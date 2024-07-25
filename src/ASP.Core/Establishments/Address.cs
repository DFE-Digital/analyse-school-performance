using ASP.Core.Helpers;

namespace ASP.Core.Establishments
{
    public class Address
    {
        public string Street { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        
        public override string ToString()
        {
            return StringHelper.ConcatNonEmpties(" ", StringHelper.ConcatNonEmpties(", ", Street, Town), PostCode);
        }
    }
}
