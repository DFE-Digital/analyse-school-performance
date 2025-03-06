using ASP.Domain.LocalAuthorities;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.Schools;

namespace ASP.Domain.DataDownloads
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
            private readonly ISchoolRepository _schoolRepository;

            public Validator(ILocalAuthorityRepository localAuthorityRepository, ISchoolRepository schoolRepository)
            {
                _localAuthorityRepository = localAuthorityRepository;
                _schoolRepository = schoolRepository;
            }

            public Result<string> ValidateScopeIdentifier(DataDownloadsScopeType scopeType, string scopeIdentifier)
            {
                var identifierDescription = scopeType == DataDownloadsScopeType.LA
                    ? "LA code"
                    : "school URN";

                return Result.Success(scopeIdentifier)
                    .ErrorIf(
                        id => !Constants.ScopeIdentifierRegex.Match(id).Success,
                        Error.Invalid($@"""{scopeIdentifier}"" is not a valid {identifierDescription}.")
                    );
            }

            public Task<Result<DataDownloadsScope>> ValidateScope(DataDownloadsScopeType scopeType, string scopeIdentifier, Optional<int> year)
            {
                return scopeType switch {
                    DataDownloadsScopeType.LA => 
                        from laCode in LACode.Parse(scopeIdentifier)
                        from la in _localAuthorityRepository.Get(laCode)
                        select (DataDownloadsScope)new DataDownloadsLocalAuthorityScope(scopeType, scopeIdentifier, year),

                    DataDownloadsScopeType.School => 
                        from schoolUrn in SchoolUrn.Parse(scopeIdentifier)
                        from school in _schoolRepository.Get(schoolUrn)
                        select (DataDownloadsScope)new DataDownloadsSchoolScope(scopeType, scopeIdentifier, year),

                    _ => throw new ArgumentOutOfRangeException($@"Data Downloads Scope type ""{scopeType}"" is not supported.")
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
