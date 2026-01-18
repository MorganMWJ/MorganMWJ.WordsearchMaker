using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MorganMWJ.WordsearchMaker.Api;

public class GenerateWordSearchFunction
{
    private readonly ILogger _logger;
    private readonly IWordsearchFactory _wordsearchFactory;

    public GenerateWordSearchFunction(ILoggerFactory loggerFactory, IWordsearchFactory wordsearchFactory)
    {
        _logger = loggerFactory.CreateLogger<GenerateWordSearchFunction>();
        _wordsearchFactory = wordsearchFactory;
    }

    [Function("GenerateWordSearch")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req)
    {
        WordSearchRequest? request;

        try
        {
            request = await JsonSerializer.DeserializeAsync<WordSearchRequest>(
                req.Body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
                }
            );
        }
        catch
        {
            return CreateError(req, HttpStatusCode.BadRequest, "Invalid JSON body.");
        }

        if (request is null ||
            request.GridSize <= 0 ||
            request.WordList is null ||
            request.WordList.Count == 0)
        {
            return CreateError(req, HttpStatusCode.BadRequest, "Invalid request data.");
        }

        try
        {
            var ws = _wordsearchFactory.Create(request);

            ws.Regenerate();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(ws.ToResponse());

            return response;
        }
        catch (Exception e)
        {
            return CreateError(req, HttpStatusCode.BadRequest, e.Message);
        }

    }

    private static HttpResponseData CreateError(
        HttpRequestData req,
        HttpStatusCode status,
        string message)
    {
        var response = req.CreateResponse(status);
        response.WriteAsJsonAsync(new { error = message });
        return response;
    }


}
