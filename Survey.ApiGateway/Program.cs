using Contracts.Protos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Mapster;

namespace Survey.ApiGateway
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region --- Mapster configuration ---
            TypeAdapterConfig<Timestamp, DateTime>.NewConfig()
                .MapWith(ts => ts.ToDateTime());

            TypeAdapterConfig<DateTime, Timestamp>.NewConfig()
                .MapWith(dt => Timestamp.FromDateTime(dt.ToUniversalTime()));

            TypeAdapterConfig<ByteString, byte[]>.NewConfig()
                .MapWith(bs => bs.ToByteArray());

            TypeAdapterConfig<byte[], ByteString>.NewConfig()
                .MapWith(b => ByteString.CopyFrom(b ?? Array.Empty<byte>()));

            TypeAdapterConfig<DateOnly, Timestamp>.NewConfig()
                .MapWith(d => Timestamp.FromDateTime(d.ToDateTime(TimeOnly.MinValue).ToUniversalTime()));

            TypeAdapterConfig<Timestamp, DateOnly>.NewConfig()
                .MapWith(ts => DateOnly.FromDateTime(ts.ToDateTime().ToLocalTime()));
            #endregion

            // Add user grpc connection.
            builder.Services.AddGrpcClient<User.UserClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:UserServiceUrl"]);
            });

            //Add news grpc connection
            builder.Services.AddGrpcClient<News.NewsClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:NewsServiceUrl"]);
            });

            builder.Services.AddGrpcClient<Contracts.Protos.Survey.SurveyClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:SurveyServiceUrl"]);
            });

            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
