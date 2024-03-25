using System.Net.Http.Json;
using deftq.Akkordplus.WebApplication.Controllers.Config;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Tests.End2End.Config
{
    [Collection("End2End")]
    public class ConfigTest
    {
        private readonly WebAppFixture fixture;

        public ConfigTest(WebAppFixture webAppFixture, ITestOutputHelper output)
        {
            fixture = webAppFixture;
        }

        [Fact]
        public async Task GivenConfiguratino_GetApiConfig_ReturnsConfiguration()
        {
            // GIVEN
            // WHEN
            var configResponse = await fixture.Client.GetFromJsonAsync<ConfigResponse>($"/api/config", fixture.JsonSerializerOptions());

            // THEN
            configResponse.Should().NotBeNull();
            configResponse!.MaxUploadFileSizeMB.Should().BeGreaterThan(0);
        }
    }}
