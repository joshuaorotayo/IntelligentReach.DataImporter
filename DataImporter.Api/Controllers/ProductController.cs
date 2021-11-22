using DataImporter.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataImporter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public ProductController(ILogger<ProductController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("Company_id={company_id}/Feed_id={feed_id}", Name = "GetProducts")]
        public IActionResult Get(int company_id, int feed_id)
        {
            SetUpMethod();

            var dataImporterService = new DataImporterService(configuration);
            _ = new ProductService(configuration);

            dataImporterService.AddCompanys();
            dataImporterService.AddFeeds();
            
            return Content(dataImporterService.ReadData(company_id,feed_id));
 
        }

        private void SetUpMethod()
        {
            var serviceCollection = new ServiceCollection();

            IConfiguration configuration;
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<DataImporterService>();
            serviceCollection.AddSingleton<ProductService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _ = serviceProvider.GetService<DataImporterService>();
            _ = serviceProvider.GetService<ProductService>();

        }

        private readonly ILogger<ProductController> _logger;
    }
}
