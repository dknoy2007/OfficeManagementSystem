using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OfficeManagementService.Data.Employee;
using OfficeManagementService.Data.Employee.Interfaces;
using OfficeManagementService.Data.TimeSheets;
using OfficeManagementService.Data.TimeSheets.Interfaces;
using OfficeManagementService.Repositories.Employee;
using OfficeManagementService.Repositories.Employee.Interfaces;
using OfficeManagementService.Repositories.TimeSheet;
using OfficeManagementService.Repositories.TimeSheet.Interfaces;
using OfficeManagementService.Services.Covid19;
using OfficeManagementService.Services.Covid19.Interfaces;
using OfficeManagementService.Services.TimeSheet;
using OfficeManagementService.Services.TimeSheet.Interfaces;

namespace OfficeManagementService
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
            services.AddSingleton<IEmployeeContext, EmployeeContext>();
            services.AddSingleton<ITineSheetContext, TineSheetContext>();

            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<ITimeSheetRepository, TimeSheetRepository>();

            services.AddSingleton<ITimeSheetService, TimeSheetService>();
            services.AddSingleton<ICovid19Service, Covid19Service>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OfficeManagementService", Version = "v1" });
            });

            services.AddControllers();

            services.AddHealthChecks();

            services.AddHealthChecks()
                .AddMongoDb(mongodbConnectionString: Configuration["EmployeesSettings:ConnectionString"], name: "mongodb", failureStatus: HealthStatus.Unhealthy);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OfficeManagementService"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapDefaultControllerRoute();

                endpoints.MapHealthChecks("/health-check");

                endpoints.MapHealthChecks("/health-check-details",
                    new HealthCheckOptions
                    {
                        ResponseWriter = async (context, report) =>
                        {
                            var result = JsonSerializer.Serialize(
                                new
                                {
                                    status = report.Status.ToString(),
                                    monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                                });
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(result);
                        }
                    }
                );
            });
        }
    }
}
