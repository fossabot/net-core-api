using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace RestApi
{
    public class Program
    {
        private static IMetricsRoot Metrics { get; set; }

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }            
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
                .UseSerilog()
                .Build();            
        }
    }
}