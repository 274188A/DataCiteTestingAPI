using Microsoft.Extensions.Configuration;
using RestSharp;
using Serilog;
using System.Net;
using static System.Net.WebRequestMethods;


var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", false, true);
var config = builder.AddUserSecrets<Program>().Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/Log-.txt"), rollingInterval: RollingInterval.Minute)
    .CreateLogger();
var configuration = builder.Build();

const string BASEURL = @"https://api.test.datacite.org/dois";


var cred1 = new NetworkCredential()
{
    UserName = configuration["UserName"],
    Password = configuration["Password"]
};


var options = new RestClientOptions(BASEURL)
{
    Credentials = cred1
};


var client = new RestClient(options);

var request = new RestRequest("");

// get path to json file
var path = Path.Combine(@"D:\repos\DataCiteTestingAPI", "data.json");

// readd json from file
 request.AddJsonBody(System.IO.File.ReadAllText(path), false);

try
{
    var response = await client.PostAsync(request);
    Log.Logger.Information(response.StatusCode.ToString());
    Log.Logger.Information(messageTemplate: response.Content!);
    Console.WriteLine("{0}", response.Content);
}
catch (HttpRequestException ex)
{
    Console.WriteLine("{0}", ex.Message);
    Console.Read();
}
