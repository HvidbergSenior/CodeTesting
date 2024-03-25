using Baseline;
using Microsoft.Extensions.Configuration;

namespace deftq.BuildingBlocks.Configuration
{
    public interface IEnvironment
    {
        string EnvironmentName { get; }
        bool IsDevelopmentEnvironment();
        bool IsDemoEnvironment();
        bool IsProductionEnvironment();
    }
    
    public class Environment : IEnvironment
    {
        private readonly IConfiguration _configuration;
        
        private readonly string _EnvironmentNameLocalDevelopment = "Development";
        
        private readonly string _EnvironmentNameDev = "DEV";
        private readonly string _EnvironmentNameDemo = "DEMO";
        private readonly string _EnvironmentNameProd = "PROD";
        
        private readonly string _environmentSection = "Environment";
        private readonly string _environmentNameKey = "Name";

        public Environment(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string EnvironmentName
        {
            get
            {
                return _configuration.GetSection(_environmentSection).GetValue<string>(_environmentNameKey);
            }
        }

        public bool IsDevelopmentEnvironment()
        {
            return EnvironmentName.EqualsIgnoreCase(_EnvironmentNameDev) || EnvironmentName.EqualsIgnoreCase(_EnvironmentNameLocalDevelopment);   
        }
        
        public bool IsDemoEnvironment()
        {
            return EnvironmentName.EqualsIgnoreCase(_EnvironmentNameDemo);   
        }
        
        public bool IsProductionEnvironment()
        {
            return EnvironmentName.EqualsIgnoreCase(_EnvironmentNameProd);
        }
    }
}
