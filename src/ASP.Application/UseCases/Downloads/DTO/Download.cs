//namespace ASP.Application.UseCases.Downloads.DTO
//{
//    public class Download
//    {
//        public required string Id { get; set; }
//        public required string Label { get; set; }
//        public required Source Source { get; set; }
//        public required int Year { get; set; }
//        public required DatasetType DatasetType { get; set; }
//        public ReleaseVersion? Version { get; set; }

//        public string BaseId
//        {
//            get
//            {
//                int lastDashIndex = Id.LastIndexOf('-');
//                return lastDashIndex >= 0 ? Id.Substring(0, lastDashIndex) : Id;
//            }
//        }
//    }
//}
