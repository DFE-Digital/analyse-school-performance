using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;

namespace ASP.Api.Client.InProcess
{
    public class FunctionRoute
    {
        private readonly string _functionName;
        private readonly string[] _httpMethods;
        private readonly string? _route;
        private readonly Func<HttpRequest, Task<ActionResult>> _func;
        private readonly RoutePattern? _routePattern;
        private readonly TemplateMatcher? _templateMatcher;

        public FunctionRoute(string functionName, string? route, string[]? httpMethods, Func<HttpRequest, Task<ActionResult>> func)
        {
            _functionName = functionName;
            _httpMethods = httpMethods ?? [];
            _route = route;
            _func = func;

            if (!string.IsNullOrEmpty(_route))
            {
                _routePattern = RoutePatternFactory.Parse("/api/" + _route);
                _templateMatcher = new TemplateMatcher(new RouteTemplate(_routePattern), new RouteValueDictionary());
            }
        }

        public bool IsMatch(HttpRequest request)
        {
            if(!_httpMethods.Contains(request.Method, StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }

            if (_routePattern != null && _templateMatcher != null)
            {
                var routeValues = new RouteValueDictionary();
                if (!_templateMatcher.TryMatch(request.Path, routeValues))
                {
                    return false;
                }

                var match = _routePattern.ParameterPolicies.All(p => IsMatchForPolicies(p.Value, p.Key, routeValues));

                return match;
            }
            else
            {
                return request.Path == "/api/" + _functionName;
            }
        }

        private bool IsMatchForPolicies(IReadOnlyList<RoutePatternParameterPolicyReference> policies, string routeParameter, RouteValueDictionary routeValues)
        {
            var routeValue = routeValues.TryGetValue(routeParameter, out var value) ? value?.ToString() ?? "" : "";

            var isMatch = policies.All(pol => pol.Content switch
            {
                "int" => int.TryParse(routeValue, out _),
                _ => true
            });


            return isMatch;
        }

        public Task<ActionResult> Run(HttpRequest request)
        {
            if (_templateMatcher != null)
            {
                var values = new RouteValueDictionary();
                if (_templateMatcher.TryMatch(request.Path, values))
                {
                    request.RouteValues = values;
                }

            }
            return _func(request);
        }
    }

}
