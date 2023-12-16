using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;

const string BASEURL = @"https://api.test.datacite.org/dois";
const string BASEPATH = @"D:\repos\DataCiteTestingAPI";

var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", false, true);
var config = builder.AddUserSecrets<Program>().Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(BASEPATH, "logs/Log-.txt"), rollingInterval: RollingInterval.Minute)
    .CreateLogger();

var configuration = builder.Build();

var options = new RestClientOptions(BASEURL)
{
    Authenticator = new HttpBasicAuthenticator(configuration["UserName"]!, configuration["Password"]!)
};

var client = new RestClient(options);
var path = Path.Combine(BASEPATH, "data.json");

var request = new RestRequest(File.ReadAllText(path), Method.Get);
request.AddHeader("Accept", "application/json");


try
{
    var response = await client.PostAsync(request);
    Log.Information(response.StatusCode.ToString());
    Log.Information(messageTemplate: response.Content!);
    Console.WriteLine("{0}", response.Content);
}
catch (HttpRequestException ex)
{
    Console.WriteLine("{0}", ex.Message);
    Log.Error(ex.Message);
    Console.Read();
}
