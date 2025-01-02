using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Core.DataDownloads
{
    public abstract class DataDownloadsScope
    {
        public DataDownloadsScopeType ScopeType { get; }
        public string Identifier { get; }
        public Optional<int> Year { get; }
        public abstract string ConfigScope { get; }
        public abstract string IdentifierPlaceholderName { get; }
        public abstract string IdentifierPlaceholderPattern { get; }
        public abstract string BuildPathPrefix(Optional<int> year);

        public DataDownloadsScope(DataDownloadsScopeType scopeType, string identifier, Optional<int> year)
        {
            ScopeType = scopeType;
            Identifier = identifier;
            Year = year;
        }

        public class Validator: IDataDownloadsScopeValidator
        {
            private ILocalAuthorityRepository _localAuthorityRepository;
            private readonly IEstablishmentRepository _establishmentRepository;

            public Validator(ILocalAuthorityRepository localAuthorityRepository, IEstablishmentRepository establishmentRepository)
            {
                _localAuthorityRepository = localAuthorityRepository;
                _establishmentRepository = establishmentRepository;
            }

            public Task<Result<DataDownloadsScope>> ValidateScope(DataDownloadsScopeType scopeType, string scopeIdentifier, Optional<int> year)
            {
                return scopeType switch {
                    DataDownloadsScopeType.LA => _localAuthorityRepository.GetLocalAuthority(scopeIdentifier)
                        .Map(la => (DataDownloadsScope)new DataDownloadsLocalAuthorityScope(scopeType, scopeIdentifier, year)),
                    _ => _establishmentRepository.GetEstablishmentDetails(scopeIdentifier)
                        .Map(school => (DataDownloadsScope)new DataDownloadsSchoolScope(scopeType, scopeIdentifier, year))
                };
            }
        }

        private class DataDownloadsLocalAuthorityScope : DataDownloadsScope
        {
            public override string ConfigScope => "LocalAuthority";
            public override string IdentifierPlaceholderName => "code";
            public override string IdentifierPlaceholderPattern => @"\d+";

            public override string BuildPathPrefix(Optional<int> year) => year.Match(y => $"LA/{Identifier}/{y}", () => $"LA/{Identifier}");

            public DataDownloadsLocalAuthorityScope(DataDownloadsScopeType scopeType, string identifier, Optional<int> year)
                : base(scopeType, identifier, year)
            {
            }

            public override string ToString()
                => $"Local Authority \"{Identifier}\"";
        }

        private class DataDownloadsSchoolScope : DataDownloadsScope
        {
            public override string ConfigScope => "School";
            public override string IdentifierPlaceholderName => "urn";
            public override string IdentifierPlaceholderPattern => @"\d+";
            public override string BuildPathPrefix(Optional<int> year) => year.Match(y => $"School/{Identifier}/{y}", () => $"School/{Identifier}");

            public DataDownloadsSchoolScope(DataDownloadsScopeType scopeType, string identifier, Optional<int> year)
                : base(scopeType, identifier, year)
            {
            }

            public override string ToString()
                => $"School \"{Identifier}\"";
        }
    }
}
