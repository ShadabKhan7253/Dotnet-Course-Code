// this will actually going to build an api which is going to be a actual server that going
// to run. server will be listening to a request and will reponse to a request when it will recieve


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // it will endpoint to the controllers 

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


/// it will try to map out all the endpoint in the application so that there are some method of 
// discovering a different route that are avaiable
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); // it allow to run the swagger UI to explore the api for testing purposes

builder.Services.AddCors((options) => 
    {
        options.AddPolicy("DevCors", (corsBuilder) => 
            {
                corsBuilder.WithOrigins("http://localhost:4200","http://localhost:3000","http://localhost:8000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            options.AddPolicy("ProdCors", (corsBuilder) => 
            {
                corsBuilder.WithOrigins("http://myProdcutionSite.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });


// builder actual build the application 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection(); // in the production mode it will check the route for
}


// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.MapControllers(); // it will have access to all the controller in our application and it will able to set the router for us 

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
