﻿using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions.Api.Dtos;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Ardalis.HttpClientTestExtensions.Tests;

public class HttpClientPostExtensionMethodsTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;
  private readonly ITestOutputHelper _outputHelper;

  public HttpClientPostExtensionMethodsTests(CustomWebApplicationFactory factory, ITestOutputHelper outputHelper)
  {
    _client = factory.CreateClient();
    _outputHelper = outputHelper;
  }

  [Fact]
  public async Task PostAndDeserializeTestAsync()
  {
    var expectedId = "CAN";
    var expectedName = "Canada";
    var dto = new CountryDto { Id = expectedId, Name = expectedName };
    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

    var response = await _client.PostAndDeserializeAsync<CountryDto>("/countries", content, _outputHelper);

    response.Id.ShouldBe(expectedId);
    response.Name.ShouldBe(expectedName);
  }

  [Fact]
  public async Task PostAndEnsureNotFoundTestAsync()
  {
    var dto = new CountryDto();
    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
    _ = await _client.PostAndEnsureNotFoundAsync("/wrongendpoint", content, _outputHelper);
  }
}
