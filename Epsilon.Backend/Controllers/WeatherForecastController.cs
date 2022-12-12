using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Backend.Controllers;

public class Droid
{
    public string Id { get; set; }
    public string Name { get; set; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class DroidType : ObjectGraphType<Droid>
{
    public DroidType()
    {
        Field(x => x.Id).Description("The Id of the Droid.");
        Field(x => x.Name).Description("The name of the Droid.");
    }
}

public sealed class StarWarsQuery : ObjectGraphType
{
    public StarWarsQuery()
    {
        Field<DroidType>("hero")
            .Resolve(context => new Droid { Id = "1", Name = "R2-D2" });
    }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get()
    {
        var schema = new Schema { Query = new StarWarsQuery() };
        
        var json = await schema.ExecuteAsync(_ =>
        {
           _.Query = "{ hero { id name } }";
        });

        Console.WriteLine(json);
        return json;
    }
}