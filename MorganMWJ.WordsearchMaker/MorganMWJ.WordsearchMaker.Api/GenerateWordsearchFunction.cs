using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MorganMWJ.WordsearchMaker.Api;

public class GenerateWordSearchFunction
{
    private readonly ILogger _logger;

    public GenerateWordSearchFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GenerateWordSearchFunction>();
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
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
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
            var ws = new Wordsearch(
                    request.WordList,
                    (uint)request.GridSize,
                    request.GenerationSeed);

            ws.Regenerate();
            var wsGrid = ws.GetGrid();
            var wsGridJagged = ToJagged(wsGrid);
            var wsString = ws.ToStringWordsearch();
            var wsSolutionString = ws.ToStringSolution();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new WordSearchResponse
            {
                GridSize = request.GridSize,
                Grid = wsGridJagged,
                Words = ws.Words,
                Seed = ws.Seed,
                WordsearchString = wsString,
                SolutionString = wsSolutionString
            });

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

    /// <summary>
    /// Move this into nuget pkg later
    /// </summary>
    public char[][] ToJagged(char[,] source)
    {
        int rows = source.GetLength(0);
        int cols = source.GetLength(1);

        var result = new char[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new char[cols];
            for (int j = 0; j < cols; j++)
            {
                result[i][j] = source[i, j];
            }
        }

        return result;
    }
}
