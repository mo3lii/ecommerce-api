
using ecommerce.HelperClasses;
using ecommerce.Models;
using ecommerce.Repository;
using ecommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ecommerceContext>(o =>
            {
                o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("mainString"));
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<Mapper>();
            builder.Services.AddScoped<ProductService>();

            builder.Services.AddAuthentication(option => option.DefaultAuthenticateScheme = "myscheme")
                  .AddJwtBearer("myscheme",
                  //validate token
                  op =>{
                      op.TokenValidationParameters = new TokenValidationParameters(){
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"))),
                          ValidateIssuer = false,
                          ValidateAudience = false
                      };
                  });

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.MapControllers();

            app.Run();
        }
    }
}
