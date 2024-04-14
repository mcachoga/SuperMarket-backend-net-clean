namespace SuperMarket.WebApi.Configuration
{
    public class Startup
    {
        protected IConfiguration Configuration { get; }

        protected IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            => (Configuration, Environment) = (configuration, environment);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomConfigurationApi(Configuration);
            services.AddCustomConfigurationDependencies(Configuration);
            services.AddCustomConfigurationSecurity(Configuration);
            services.AddCustomConfigurationCors(Configuration);
            services.AddCustomConfigurationSwagger(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCustomDependencies(Configuration);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCustomCors(Configuration);
            app.UseCustomSwagger(Configuration);
            app.UseCustomApi(Configuration);
        }
    }
}