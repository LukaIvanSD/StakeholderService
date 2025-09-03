using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stakeholders.Infrastructure;
using Stakeholders.Startup;
using Stakeholders.GrpcsServices;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(8888, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
    options.ListenLocalhost(8080, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1);
});

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

builder.Services.RegisterModules();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
    db.Database.Migrate(); //
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<AuthenticationGrpcService>();
app.MapGrpcService<PersonGrpcService>();
app.MapGrpcService<UserGrpcService>();

app.MapControllers();

app.Run();
