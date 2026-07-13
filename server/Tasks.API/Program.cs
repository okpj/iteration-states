using Tasks.API.Model;
using Tasks.API.Services;

const string cors = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllers();

services.Configure<TfsApiOptions>(builder.Configuration.GetSection(TfsApiOptions.SectionName));
services.AddScoped<TfsServiceFactory>();

services.AddCors(options =>
{
  options.AddPolicy(cors,
      x => x
          .SetIsOriginAllowed(origin => true)
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
  });
}

app.UseCors(cors);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
