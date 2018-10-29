using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EFAPITestAPI.JobModule.v1.Model;
using Newtonsoft.Json;

namespace EFAPITestAPI.JobModule.v1.Client
{
    public class JobModuleAPIClient
    {
        private readonly HttpClient _httpClient;
        private readonly JobModuleAPIClientConfiguration _config;

        public JobModuleAPIClient(HttpClient httpClient, JobModuleAPIClientConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<ClientResult<IEnumerable<Job>>> GetJobs()
        {
            return await Get<IEnumerable<Job>>("jobs_module/api/v1/jobs");
        }

        #region Client Helpers

        private async Task<ClientResult<T>> Get<T>(string url) where T : class
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpClient.BaseAddress + url)
            };

            return await Send<T>(message);
        }

        private async Task<ClientResult<T>> Send<T>(HttpRequestMessage message) where T : class
        {
            if (_config?.ApiKey != null)
            {
                message.Headers.Add("ApiKey", _config.ApiKey.Value.ToString());
            }

            var result = new ClientResult<T>();

            using (var response = await _httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    result.Data = await ReadResponseStream<T>(response);
                }
                else
                {
                    result.Error = await ReadResponseStream<ApiError>(response);
                }
            }

            return result;
        }

        private async Task<T> ReadResponseStream<T>(HttpResponseMessage response) where T : class
        {
            var serializer = new JsonSerializer();
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        #endregion

    }
}
