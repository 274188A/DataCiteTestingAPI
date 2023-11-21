using Microsoft.Extensions.Configuration;

using RestSharp;
using System.Net;

//https://api.test.datacite.org/dois




var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();

var cred1 = new NetworkCredential()
{
    UserName = "john.barrett@curtin.edu.au",
    Password = configuration["Password"]
};

UriBuilder ub = new UriBuilder("")


var options = new RestClientOptions("https://api.test.datacite.org/dois");
options.Credentials = cred1;


var client = new RestClient(options);

var request = new RestRequest("");
request.AddJsonBody("{\"data\":{\"type\":\"dois\",\"attributes\":{\"types\":{\"resourceTypeGeneral\":\"Audiovisual\"}}}}", false);
var response = await client.PostAsync(request);

Console.WriteLine("{0}", response.Content);
