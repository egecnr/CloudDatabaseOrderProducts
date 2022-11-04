using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExtraFunction.DAL;
using ExtraFunction.Repository.Interface;
using ExtraFunction.Service;
using ExtraFunction.Repository;

namespace ExtraFunction
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                     .ConfigureFunctionsWorkerDefaults()
                     .ConfigureAppConfiguration(config =>
                          config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: false))
                     .ConfigureOpenApi()
                     .ConfigureServices(services =>
                     {
                         services.AddControllers();
                         services.AddDbContext<DatabaseContext>(options =>

                                   options.UseCosmos("https://sawa-db.documents.azure.com:443/",
                         "gggcb28Z24nJAmpz4SRwQRNT9Xyd0wn1riSKAUkvVyaBf4WRALsyx4kgl6POPmi8Ka7JHZfTx06uWD3DHzoqTw==",
                         "sawa-db"));
                         services.AddTransient<IProductService, ProductService>();
                         services.AddTransient<IReviewService, ReviewServices>();
                         services.AddTransient<IProductRepository, ProductRepository>();
                         services.AddTransient<IReviewRepository, ReviewRepository>();                     
                     })
                     .Build();


            host.Run();


        }
    }
}
