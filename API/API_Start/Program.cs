// this will actually going to build an api which is going to be a actual server that going
// to run. server will be listening to a request and will reponse to a request when it will recieve


using System.Text;
using API_Start.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

// t will help to use the method of IUserRepository inside the controller
builder.Services.AddScoped<IUserRepository,UserRepository>();

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(
            tokenKeyString != null ? tokenKeyString : ""
        )
    );

TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuerSigningKey = true,
    ValidateIssuer = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
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
    app.UseCors("DevCors");
    app.UseHttpsRedirection(); // in the production mode it will check the route for
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers(); // it will have access to all the controller in our application and it will able to set the router for us 

app.Run();

