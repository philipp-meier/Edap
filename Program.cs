using Edap.Models;
using Edap.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeFolder("/");
        options.Conventions.AllowAnonymousToPage("/Index");
        options.Conventions.AllowAnonymousToPage("/Polls/Participate");
    });

services.AddDbContext<PollContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //options.Cookie.SameSite = SameSiteMode.Strict;

    options.Cookie.Name = builder.Configuration["IdentityProvider:CookieName"];
})
.AddOpenIdConnect(options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    options.Authority = builder.Configuration["IdentityProvider:Authority"];
    options.ClientId = builder.Configuration["IdentityProvider:ClientId"];
    options.ClientSecret = builder.Configuration["IdentityProvider:ClientSecret"];
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.ResponseMode = OpenIdConnectResponseMode.Query;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.RequireHttpsMetadata = true;

    options.Scope.Add("openid");
    options.Scope.Add("email");
    options.Scope.Add("profile");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = options.Authority,
        ValidAudience = options.ClientId,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromSeconds(5),
        IssuerSigningKeyResolver = (_, _, kid, _) =>
        {
            var jsonAsync = new HttpClient().GetStringAsync(builder.Configuration["IdentityProvider:JwksUri"]);
            jsonAsync.Wait();

            return JsonWebKeySet.Create(jsonAsync.Result)
                .GetSigningKeys()
                .Where(x => x.KeyId == kid)
                .ToArray();
        }
    };

    options.SaveTokens = true;
});

services.AddScoped<PollService>();

// Required for the NGINX proxy.
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
