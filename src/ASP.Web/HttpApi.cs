using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.DTO.Establishment;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Templating;
using Microsoft.Extensions.Options;
using System.Net;

namespace ASP.Web
{
    public class HttpApi : IAspApi
    {
        private const string ApiError = "API error: ";

        private readonly ApiOptions _options;
        private readonly HttpClient _httpClient;

        public HttpApi(IOptions<ApiOptions> options)
        {
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;

            _httpClient = new HttpClient { BaseAddress = new Uri(_options.EndpointBaseUrl) };
        }

        public async Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request)
        {
            var url = "ViewContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);
            if (request.Revision != null)
            {
                queryString = queryString.Add("revision", request.Revision);
            }

            return await ApiGet(url, queryString)
                .Then(JsonHelper.Deserialize<ContentTemplate>);
        }

        public async Task<Result<UpdateContentTemplateResponse>> UpdateContentTemplate(UpdateContentTemplateRequest request)
        {
            var url = "UpdateContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);
            if (request.Revision != null)
            {
                queryString = queryString.Add("revision", request.Revision);
            }

            return await ApiPost(url, queryString, request.ContentTemplate)
                .Then(JsonHelper.Deserialize<UpdateContentTemplateResponse>);
        }

        public async Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsUseCaseRequest request)
        {
            var url = "GetEstablishmentDetails";
            var queryString = QueryString.Create("urn", request.Urn);
            return await ApiGet(url, queryString)
                .Then(JsonHelper.Deserialize<EstablishmentDetailsDTO>);
        }

        public async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> EstablishmentSearch(EstablishmentSearchUseCaseRequest request)
        {
            var url = "EstablishmentSearch";
            var queryString = QueryString.Create("searchTerm", request.SearchTerm);
            return await ApiGet(url, queryString)
                .Then(JsonHelper.Deserialize<SearchResult<EstablishmentDetailsSearchResultDTO>>);
        }

        private async Task<Result<string>> ApiGet(string url, QueryString queryString)
        {
            try
            {
                var response = await _httpClient.GetAsync(url + queryString);
                return await ToResult(response);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<string>(ex.Message);
            }
        }

        private async Task<Result<string>> ApiPost(string url, QueryString queryString, object body)
        {
            try
            {
                var response = await _httpClient.PostAsync(url + queryString, JsonContent.Create(body));
                return await ToResult(response);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<string>(ex.Message);
            }
        }

        private async Task<Result<string>> ToResult(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                return response.StatusCode switch {
                    HttpStatusCode.OK => Result.Success(content ?? ""),
                    HttpStatusCode.NotFound => Result.NotFound<string>(ApiError + RemoveFromStart("Not found: ", content)),
                    HttpStatusCode.BadRequest => Result.Invalid<string>(ApiError + RemoveFromStart("Invalid: ", content)),
                    HttpStatusCode.Forbidden => Result.NotAllowed<string>(ApiError + RemoveFromStart("Not allowed: ", content)),
                    _ => Result.Unexpected<string>(ApiError + RemoveFromStart("Unexpected: ", content))
                };
            }
            catch (Exception ex)
            {
                return Result.Unexpected<string>(ex.Message);
            }
        }

        private string RemoveFromStart(string stringToRemove, string text)
        {
            if(!text.StartsWith(stringToRemove))
            {
                return text;
            }

            return text.Substring(stringToRemove.Length);
        }
    }
}