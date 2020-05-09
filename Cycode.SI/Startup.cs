using Cycode.BL;
using Cycode.Common.Interfaces.BusinessLogic;
using Cycode.Common.Interfaces.Repositories;
using Cycode.DAL;
using Cycode.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cycode.SI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IGradesRepository, GradesRepository>();
            services.AddScoped<IStudentRepository, StudentsRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IStudentInCourseRepository, StudentInCourseRepository>();
            services.AddScoped<ICycodeBusinessLogic, CycodeBusinessLogic>();
            
            services.AddDbContext<CycodeContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("cycodeConnection")));
            
            services.AddControllers();
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            // app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}