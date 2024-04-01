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
            app.UseCustomDependencies();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.UseCustomCors(Configuration);
            app.UseCustomSwagger(Configuration);
            app.UseCustomSecurity(Configuration);
            app.UseCustomApi(Configuration);
        }
    }
}