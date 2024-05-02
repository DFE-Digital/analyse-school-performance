namespace ASP.Web;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAppConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseAuthorization()
            .UseRouting();
            //.UseEndpoints(r => r.MapControllers());

        if (!environment.IsDevelopment())
        {
            app
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                .UseHsts();
        }

        return app;
    }
}