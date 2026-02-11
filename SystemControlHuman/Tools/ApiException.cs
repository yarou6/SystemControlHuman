using System;
using System.Net;

namespace SystemControlHuman.Tools;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApiException(HttpStatusCode code, string message) : base(message)
    {
        StatusCode = code;
    }
}