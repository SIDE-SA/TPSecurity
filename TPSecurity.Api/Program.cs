using Serilog;
using TPAuth.Repositories;
using TPAuth.Services;
using TPAuth.Services.Common.Middleware;
using TPSecurity.Api;
using TPSecurity.Api.Filter;
using TPSecurity.Api.Http;
using TPSecurity.Application;
using TPSecurity.Infrastructure;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);
{
    #region Logger

    builder.Host.UseSerilog((ctx, cfg)
    => cfg.ReadFrom.Configuration(ctx.Configuration)
          .Enrich.With(new ThreadIdEnricher()));
    
    #endregion

    #region Auth
    builder.Services
        .AddTPAuthInfrastructure()
        .AddTPAuthApplication();
    #endregion

    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.OperationFilter<UserHeaderFilter>();
    });    

    builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders(HttpContextItemKeys.hashCode)
                .WithExposedHeaders(HttpContextItemKeys.Pagination);
    }));

    builder.Services.AddHealthChecks();
}

var app = builder.Build();
{    
    //app.UseSerilogRequestLogging();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseApiVersioning();
    app.UseMiddleware<BasicAuthMiddleware>();
    app.UseExceptionHandler("/error");
    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseCors("corsapp");
    app.MapControllers();
    app.Run();
}