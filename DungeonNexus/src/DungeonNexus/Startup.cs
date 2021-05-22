using Blazored.LocalStorage;
using DungeonNexus.Infrastructure.DependencyContainer;
using DungeonNexus.Model;
using DungeonNexus.Model.Users;
using DungeonNexus.ViewModel;
using DungeonNexus.ViewModel.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;

namespace DungeonNexus
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContextFactory();
            services.AddBlazoredLocalStorage();
            services.AddHttpClient();

            RegisterAnnotatedTypesFromAssemblyContaining<DungeonNexusDB>(services);
            RegisterAnnotatedTypesFromAssemblyContaining<ViewModelBase>(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private static void RegisterAnnotatedTypesFromAssemblyContaining<TRepresentant>(IServiceCollection serviceCollection)
        {
            var services = typeof(TRepresentant).Assembly
                .GetTypes()
                .Select(type => (type, attribute: type.GetCustomAttribute<ServiceAttribute>()));

            foreach (var (type, attribute) in services)
            {
                if (attribute is not null)
                    attribute.Register(type, serviceCollection);
            }
        }
    }
}
