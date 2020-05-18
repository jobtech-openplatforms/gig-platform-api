using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
  public class PlatformHttpClient : IPlatformHttpClient
  {
    private readonly HttpClient _client;
    private readonly ILogger<PlatformHttpClient> _logger;

    public PlatformHttpClient(HttpClient client, ILogger<PlatformHttpClient> logger)
    {
      _client = client;
      _logger = logger;
    }

    public async Task<PlatformDataUserUpdateResult> GetUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri)
    {
      var response = await GetUserDataFromPlatformResponseAsync(request, exportDataUri);

      if (!response.IsSuccessStatusCode)
      {
        switch (response.StatusCode)
        {
          case HttpStatusCode.NotFound:
            throw new UserNotFoundForPlatformException(request.Username, "Platform reported non existant user");
          default:
            throw new PlatformCommunicationException($"Got unhandled http status code {response.StatusCode} from platform");
        }
      }

      var stringResult = await response.Content.ReadAsStringAsync();

      PlatformDataUserUpdateResult result;
      try
      {
        result = JsonConvert.DeserializeObject<PlatformDataUserUpdateResult>(stringResult);
        result.RawData = stringResult;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Could not parse data provided by the platform.");
        throw new MalformedPlatformDataException(stringResult, "Could not parse data");
      }

      return result;
    }

    public async Task<PlatformDataUserTestResult> TestUserDataFromPlatformAsync(UserDataRequest request, string exportDataUri)
    {
      var response = await GetUserDataFromPlatformResponseAsync(request, exportDataUri);

      var stringResult = await response.Content.ReadAsStringAsync();

      PlatformDataUserUpdateResult result = null;
      if (response.IsSuccessStatusCode)
      {
        try
        {
          result = JsonConvert.DeserializeObject<PlatformDataUserUpdateResult>(stringResult);
        }
        catch (Exception)
        {
          _logger.LogInformation("Could not deserialize repsonse from platform.");
        }
      }

      return new PlatformDataUserTestResult(result,
          new TestRequest(null, request
              )
          , new TestResponse(
              response.Headers.Select(item => string.Format("{0} : {1}", item.Key, string.Join(", ", item.Value))).ToArray()
              , response.StatusCode.ToString()
              , stringResult
          ));
    }

    private async Task<HttpResponseMessage> GetUserDataFromPlatformResponseAsync(UserDataRequest request, string exportDataUri)
    {
      _client.BaseAddress = new Uri(exportDataUri);

      // For GETting the user data from the external platform
      //_client.DefaultRequestHeaders.Add("platformToken", request.PlatformToken);
      //_client.DefaultRequestHeaders.Add("username", request.Username);
      //_client.DefaultRequestHeaders.Add("requestId", request.RequestId);
      //var response = await _client.GetAsync("");

      var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
      HttpResponseMessage response;
      try
      {
        response = await _client.PostAsync(exportDataUri, content);
      }
      catch (Exception e)
      {
        _logger.LogError(e, "Could not communicate with the platform. Will throw.");
        throw new PlatformCommunicationException("Could not communicate with platform.");
      }

      return response;
    }
  }

  public class UserNotFoundForPlatformException : Exception
  {
    public UserNotFoundForPlatformException(string username, string message = null) : base(message)
    {
      Username = username;
    }
    public string Username { get; }
  }

  public class PlatformCommunicationException : Exception
  {
    public PlatformCommunicationException(string message = null) : base(message)
    {

    }
  }

  public class MalformedPlatformDataException : Exception
  {
    public MalformedPlatformDataException(string rawData, string message = null) : base(message)
    {
      RawData = rawData;
    }

    public string RawData { get; }
  }
}