using Duende.IdentityServer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddTestUsers(TestUsers.Users);

        builder.Services.AddAuthentication()
            .AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = "111";
                options.ClientSecret = "111";
            });
            // .AddOpenIdConnect("oidc","demo IdentityServer", options =>
            // {
            //     options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //     options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            //     options.SaveTokens = true;
            //
            //     options.Authority = "https://demo.duendesoftware.com";
            //     options.ClientId = "interactive.confidential";
            //     options.ClientSecret = "secret";
            //     options.ResponseType = "code";
            //
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         NameClaimType = "name",
            //         RoleClaimType = "role"
            //     };
            // });
        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
