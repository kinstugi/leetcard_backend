using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpCardAPI.Data;
using SharpCardAPI.Repository;
using SharpCardAPI.Tasks;
using SharpCardAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(); // adding db context

builder.Services.AddTransient<IAuthRepository,AuthRepository>();
builder.Services.AddTransient<IUserRepository,UserRepository>();
builder.Services.AddTransient<IQuestionRepository,QuestionRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<LoadDataTask>(); // to load questions from csv files

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder => {
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); //adding auth middleware
app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy"); // remove this
app.Run();
