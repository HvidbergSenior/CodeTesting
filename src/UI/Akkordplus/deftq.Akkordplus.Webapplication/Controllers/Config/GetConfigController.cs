using Microsoft.AspNetCore.Mvc;

namespace deftq.Akkordplus.WebApplication.Controllers.Config
{
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _conf;
        private const string AzureAdB2CSection = "AzureAdB2C";
        private const string ClientIdProperty = "ClientId";
        private const string AuthorityProperty = "authority";
        private const string EnvironmentSection = "Environment";
        private const string MaxUploadFileSizeMBProperty = "MaxUploadFileSizeMB";

        public ConfigController(IConfiguration conf) : base()
        {
            _conf = conf;
        }
        
        [HttpGet]
        [Route("/api/config")]
        [ProducesResponseType(typeof(ConfigResponse), StatusCodes.Status200OK)]
        public Task<ActionResult> Configuration()
        {
            var clientId = _conf.GetSection(AzureAdB2CSection)[ClientIdProperty];
            var authority = _conf.GetSection(AzureAdB2CSection)[AuthorityProperty];
            var knownAuthority = new Uri(_conf.GetSection(AzureAdB2CSection)[AuthorityProperty]).Host;
            var maxUploadFileSizeMB = _conf.GetSection(EnvironmentSection).GetValue<int>(MaxUploadFileSizeMBProperty);

            var config = new ConfigResponse(new AzureAdB2C(clientId, authority, knownAuthority), new FeatureFlags(), maxUploadFileSizeMB);
            return Task.FromResult<ActionResult>(base.Ok(config));
        }
    }
    
    public class ConfigResponse
    {
        public AzureAdB2C AzureAdB2C { get; private set; }
        public FeatureFlags FeatureFlags { get; private set; }
        public int MaxUploadFileSizeMB { get; private set; }

        public ConfigResponse(AzureAdB2C azureAdB2C, FeatureFlags featureFlags, int maxUploadFileSizeMB)
        {
            AzureAdB2C = azureAdB2C;
            FeatureFlags = featureFlags;
            MaxUploadFileSizeMB = maxUploadFileSizeMB;
        }
    }

    public class AzureAdB2C
    {
        public string ClientId { get; private set; }
        public string Authority { get; private set; }
        public string KnownAuthority { get; private set; }

        public AzureAdB2C(string clientId, string authority, string knownAuthority)
        {
            ClientId = clientId;
            Authority = authority;
            KnownAuthority = knownAuthority;
        }
    }
        
    public class FeatureFlags
    {
        
    }
}
