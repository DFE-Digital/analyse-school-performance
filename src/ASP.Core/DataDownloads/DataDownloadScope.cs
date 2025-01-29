using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Core.DataDownloads
{
    public abstract class DataDownloadScope
    {
        public DataDownloadScopeType ScopeType { get; }
        public string Identifier { get; }
        public Optional<int> Year { get; }
        public abstract string ConfigScope { get; }
        public abstract string IdentifierPlaceholderName { get; }
        public abstract string IdentifierPlaceholderPattern { get; }
        public abstract string BuildPathPrefix(Optional<int> year);

        public DataDownloadScope(DataDownloadScopeType scopeType, string identifier, Optional<int> year)
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

            public Result<string> ValidateScopeIdentifier(string scopeIdentifier)
            {
                return Result.Success(scopeIdentifier)
                    .ErrorIf(
                        id => !Constants.ScopeIdentifierRegex.Match(id).Success,
                        Error.Invalid($@"""{scopeIdentifier}"" is not a valid scopeIdentifier.")
                    );
            }

            public Task<Result<DataDownloadScope>> ValidateScope(DataDownloadScopeType scopeType, string scopeIdentifier, Optional<int> year)
            {
                return scopeType switch {
                    DataDownloadScopeType.LA => _localAuthorityRepository.GetLocalAuthority(scopeIdentifier)
                        .Map(la => (DataDownloadScope)new DataDownloadsLocalAuthorityScope(scopeType, scopeIdentifier, year)),
                    _ => _establishmentRepository.GetEstablishmentDetails(scopeIdentifier)
                        .Map(school => (DataDownloadScope)new DataDownloadsSchoolScope(scopeType, scopeIdentifier, year))
                };
            }
        }

        private class DataDownloadsLocalAuthorityScope : DataDownloadScope
        {
            public override string ConfigScope => "LocalAuthority";
            public override string IdentifierPlaceholderName => "code";
            public override string IdentifierPlaceholderPattern => @"\d+";

            public override string BuildPathPrefix(Optional<int> year) => year.Match(y => $"LA/{Identifier}/{y}", () => $"LA/{Identifier}");

            public DataDownloadsLocalAuthorityScope(DataDownloadScopeType scopeType, string identifier, Optional<int> year)
                : base(scopeType, identifier, year)
            {
            }

            public override string ToString()
                => $"Local Authority \"{Identifier}\"";
        }

        private class DataDownloadsSchoolScope : DataDownloadScope
        {
            public override string ConfigScope => "School";
            public override string IdentifierPlaceholderName => "urn";
            public override string IdentifierPlaceholderPattern => @"\d+";
            public override string BuildPathPrefix(Optional<int> year) => year.Match(y => $"School/{Identifier}/{y}", () => $"School/{Identifier}");

            public DataDownloadsSchoolScope(DataDownloadScopeType scopeType, string identifier, Optional<int> year)
                : base(scopeType, identifier, year)
            {
            }

            public override string ToString()
                => $"School \"{Identifier}\"";
        }
    }
}
