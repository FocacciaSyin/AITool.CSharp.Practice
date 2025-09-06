namespace AITool.CSharp.Practice.Models.Helpers;

/// <summary>
/// 是否要紀錄觸發 AI Http Request/Response 的 Helper
/// </summary>
public class HttpLoggerHelper : DelegatingHandler
{
    public static HttpClient GetHttpClient(bool log = false)
    {
        return log
            ? new HttpClient(new HttpLoggerHelper(new HttpClientHandler()))
            : new HttpClient();
    }

    public HttpLoggerHelper(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Request Body:");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        //Console.WriteLine(request.ToString());
        //Console.WriteLine();
        if (request.Content != null)
        {
            Console.WriteLine(await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
        }

        Console.WriteLine();
        Console.ResetColor();


        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Response Body:");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        //Console.WriteLine(response.ToString());
        //Console.WriteLine();
        if (response.Content != null)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
        }

        Console.WriteLine();
        Console.ResetColor();


        return response;
    }
}
