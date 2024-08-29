using be_ip.Checkout.Settings;
using be_ip_common.Keyvault;
using be_ip_common.Keyvault.Interface;
using be_ip_common.Keyvault.Settings;
using be_ip_repository.Blob;
using be_ip_repository.Blob.Interface;
using be_ip_repository.Blob.Settings;
using be_ip_repository.Cosmos;
using be_ip_repository.Cosmos.Interfaces;
using be_ip_repository.Cosmos.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("BlobSettings"));
builder.Services.Configure<KeyVaultSettings>(builder.Configuration.GetSection("KeyVaultSettings"));
builder.Services.Configure<CosmosSettings>(builder.Configuration.GetSection("CosmosSettings"));
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBusSettings"));

builder.Services.AddSingleton<IKeyVaultService, KeyVaultService>();
builder.Services.AddSingleton<ICosmosRepository, CosmosRepository>();
builder.Services.AddSingleton<IBlobRepository, BlobRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://login.microsoftonline.com/102b235b-53e4-4329-ad73-929176d92c47/v2.0";
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://sts.windows.net/102b235b-53e4-4329-ad73-929176d92c47/",
                ValidAudience = "api://9a6868a2-4245-4d7e-9f3c-10886fe66d82",
            };
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
