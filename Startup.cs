using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestoAppAPI.Service;
using RestoAppAPI.Repository;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using RestoAppAPI.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using RestoAppAPI.Middlewares;

namespace RestoAppAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestoAppAPI", Version = "v1" });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
            });
            services.AddHttpContextAccessor();
            services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
            services.AddScoped<IImageRepository,ImageRepository>();
            services.AddScoped<IMenuCategoryService,MenuCategoryService>();
            services.AddScoped<IImageService,ImageService>();
            services.AddScoped<IMenuCategoryRepository,MenuCategoryRepository>();
            services.AddScoped<ITokenRepository,TokenRepository>();
            services.AddScoped<IMenuRepository,MenuRepository>();
            services.AddScoped<IMenuSerive,MenuService>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderService,OrderSerive>();
            services.AddScoped<ITableService,TableService>();
            services.AddScoped<ITableRepository,TableRepository>();
            services.AddScoped<IDashBoardRepository,DashBoardRepository>();
            services.AddScoped<IDashboardService,DashboardSerive>();
            
           
           

            ////////////////////////////////////////////////////
    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5001/",
                ValidAudience = "https://localhost:5001/",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bbuosyJSSGPOosflUSJ75JJHst6yjjjST5rt65SY77uhSYSko098HHhgst"))
            };
    });


            /////////////////////////////////////////////////
        

            services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            // .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

    
            services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestoAppAPI v1"));
            }
            
            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddlewareADO>();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Logs")),
                RequestPath = "/Logs"
            });
           
             app.UseCors("AllowAll");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context)=>{
               await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            });
        }
    }
}
