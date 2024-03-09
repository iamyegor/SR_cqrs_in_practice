using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Api.ActionConstraints;

[AttributeUsage(AttributeTargets.Method)]
public class RequestHeaderValidMediaType : Attribute, IActionConstraint
{
    private readonly string _headerName;
    private readonly MediaTypeCollection _allowedMediaTypes;

    public RequestHeaderValidMediaType(string headerName, params string[] allowedMediaTypes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headerName);
        _headerName = headerName;

        if (allowedMediaTypes.Length == 0)
        {
            throw new ArgumentException("At least one media type has to be provided");
        }

        _allowedMediaTypes = new MediaTypeCollection();
        foreach (var allowedMediaType in allowedMediaTypes)
        {
            _allowedMediaTypes.Add(allowedMediaType);
        }
    }

    public bool Accept(ActionConstraintContext context)
    {
        IHeaderDictionary requestHeaders = context.RouteContext.HttpContext.Request.Headers;
        if (!requestHeaders.ContainsKey(_headerName))
        {
            return false;
        }

        MediaType parsedRequestMediaType = new MediaType(requestHeaders[_headerName]!);
        foreach (var allowedMediaType in _allowedMediaTypes)
        {
            MediaType parsedAllowedMediaType = new MediaType(allowedMediaType);
            if (parsedAllowedMediaType.Equals(parsedRequestMediaType))
            {
                return true;
            }
        }

        return false;
    }

    public int Order => 0;
}
