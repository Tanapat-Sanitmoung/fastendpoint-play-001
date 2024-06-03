
using FastEndpoints;
using FluentValidation;

namespace SimpleApi.WeatherForcast;

public sealed class GetRandomWeatherForecast
    : Endpoint<WeatherForecastRequest, WeatherForecastView>
{
    public override void Configure()
    {
        Get("/weatherforecast");
        AllowAnonymous();
    }

    public override Task<WeatherForecastView> ExecuteAsync(WeatherForecastRequest request, CancellationToken ct)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast =  new WeatherForecastView(
            Enumerable.Range(1, 5).Select(index =>
                new WeatherForecastView.Data
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray());

        return Task.FromResult(forecast);
    }
}

public record WeatherForecastRequest
{
    [QueryParam, BindFrom("days")]
    public int Days { get; init; }
}

public class WeatherForecastRequestValidator: Validator<WeatherForecastRequest>
{
    public WeatherForecastRequestValidator()
    {
        RuleFor(e => e.Days)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Days must greater than zero")
            .LessThanOrEqualTo(14)
            .WithMessage("Days msut less than or equal to 14");
        
    }
}

public record WeatherForecastView(WeatherForecastView.Data[] Forecasts)
{
    public record Data(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}