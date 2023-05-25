using IPR_BE.Services;
using IPR_BE.DataAccess;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<SMTPService>();
builder.Services.AddScoped<InterviewBotRepo>((ctx) => new InterviewBotRepo(builder.Configuration.GetConnectionString("InterviewBotDB")));
builder.Services.AddDbContext<TestReportDbContext>((options) => options.UseSqlServer(builder.Configuration.GetConnectionString("ReportsDB")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Leave Swagger on for prods too, for testing purposes
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
