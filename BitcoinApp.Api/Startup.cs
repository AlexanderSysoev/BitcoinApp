using System;
using System.Reflection;
using BitcoinApp.Api.HostedServices;
using BitcoinApp.Domain.InTransactionAggregate;
using BitcoinApp.Domain.OutTransactionAggregate;
using BitcoinApp.Domain.SeedWork;
using BitcoinApp.Domain.WalletAggregate;
using BitcoinApp.Infrastructure;
using BitcoinApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitcoinApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCustomDbContext(Configuration);
            services.AddRepositories();
            services.AddDomainServices();
            services.AddHostedServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<SendService>();
            services.AddScoped<NewTrackService>();
            services.AddScoped<ConfirmationTrackService>();
            services.AddScoped<LastInTransactionsService>();
            services.AddScoped<IBitcoinServiceFactory, BitcoinServiceFactory>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IOutTransactionRepository, OutTransactionRepository>();
            services.AddScoped<IInTransactionRepository, InTransactionRepository>();

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<ConfirmationTrackHostedService>();
            services.AddHostedService<NewTrackHostedService>();

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<BitcoinAppContext>(options =>
                    {
                        options.UseSqlServer(configuration["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 10,
                                    maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
                    }
                );

            return services;
        }
    }
}
