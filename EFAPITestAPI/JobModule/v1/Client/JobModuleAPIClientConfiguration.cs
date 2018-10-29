using System;
namespace EFAPITestAPI.JobModule.v1.Client
{
    public class JobModuleAPIClientConfiguration
    {
        public JobModuleAPIClientConfiguration(Guid? apiKey)
        {
            ApiKey = apiKey;
        }

        public Guid? ApiKey { get; private set; }
    }
}
