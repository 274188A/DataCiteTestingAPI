using Microsoft.Extensions.Configuration;
using RestSharp;
using Serilog;
using System.Net;

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

var networkCreds = new NetworkCredential()
{
    UserName = configuration["UserName"],
    Password = configuration["Password"]
};

var options = new RestClientOptions(BASEURL)
{
    Credentials = networkCreds
};


var client = new RestClient(options);
var request = new RestRequest("");

// get path to json file
var path = Path.Combine(BASEPATH, "data.json");

// read json from file
 request.AddJsonBody(File.ReadAllText(path), false);

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
