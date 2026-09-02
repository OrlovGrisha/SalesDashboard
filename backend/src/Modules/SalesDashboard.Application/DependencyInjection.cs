using Microsoft.Extensions.DependencyInjection;
using SalesDashboard.Application.Common;
using SalesDashboard.Application.Services;

namespace SalesDashboard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IManagerRankingService, ManagerRankingService>();
        services.AddScoped<PeriodResolver>();
        
        return services;
    }
}