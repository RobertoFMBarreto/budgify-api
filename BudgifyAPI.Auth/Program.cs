using BudgifyAPI.Auth.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls("http://+:5072").UseKestrel();
builder.Services.AddModels();
builder.Services.AddServices();
builder.Services.AddQueries();
builder.Services.AddHelpers();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();