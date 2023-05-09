// -------------------------------------------------------------------
//  Copyright (c) Axis Communications AB, SWEDEN. All rights reserved.
// -------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddCors(
	options => options.AddPolicy(name: "LocalhostOrigins",
		policy => policy
		.WithOrigins("http://localhost:8080", "https://localhost:8080")
		.WithMethods("GET", "POST", "OPTIONS")
		.AllowAnyHeader()));


// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("LocalhostOrigins");

app.Run();
