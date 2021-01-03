using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using McuServerApi;
using McuServerApi.core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;

namespace McuServerTest
{
    [TestFixture]
    public class EndToEndTest
    {
        private IHost apiHost;
        private Task apiTask;
        private IConfiguration apiConfiguration;

        [SetUp]
        public void Setup()
        {
            apiHost = McuApiProgram.CreateHostBuilder(new string[] { }).Build();
            apiConfiguration = apiHost.Services.GetService<IConfiguration>();
            apiTask = Task.Run(action: () => apiHost.Run());
        }

        [TearDown]
        public async Task TearDown()
        {
            await apiHost.StopAsync();
            await apiTask;
        }

        [Test]
        public async Task WhenImageReportedApiShouldSaveImage()
        {
            var httpClient = new HttpClient( /* new LoggingHandler(new HttpClientHandler()) */);
            var image = new ImageInfo
            {
                Base64Data = Convert.ToBase64String(new byte[] {1, 2, 3}),
                ClientName = "Test",
                DateTime = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(image), Encoding.UTF8, "application/json");
            httpClient.BaseAddress = new Uri(apiConfiguration["urls"]);
            var response = await httpClient.PostAsync("image/report", content);

            var uploadDirectory = apiConfiguration["ImageUploadDirectory"];
            var imageDateTimeFormat = apiConfiguration["ImageDateTimeFormat"];

            using (new AssertionScope())
            {
                Directory.EnumerateFiles(uploadDirectory).Should().Contain(f =>
                    f.Contains(image.ClientName) && f.Contains(image.DateTime.ToString(imageDateTimeFormat)));

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}