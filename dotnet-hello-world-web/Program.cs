using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace RestApi
{
    public class Program
    {
        private static IMetricsRoot Metrics { get; set; }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            Metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();
            
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureMetrics(Metrics)
                .UseMetrics(
                    options =>
                    {
                        options.EndpointOptions = endpointsOptions =>
                        {
                            endpointsOptions.MetricsTextEndpointOutputFormatter =
                                Metrics.OutputMetricsFormatters.GetType<MetricsPrometheusTextOutputFormatter>();
                            endpointsOptions.MetricsEndpointOutputFormatter = 
                                Metrics.OutputMetricsFormatters.GetType<MetricsPrometheusProtobufOutputFormatter>();
                        };
                    })
                .UseStartup<Startup>()
                .Build();            
        }
    }
}