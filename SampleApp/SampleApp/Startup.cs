using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<SampleAppDbContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("SampleAppConnection")));
            services.AddIdentity<IdentityUser,IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();

            var success = CreateUserRoles(services);
        }

        //Create admin role and seed default user if not already configured
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            
            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = RoleManager.RoleExistsAsync("Admin");
            roleCheck.Wait();
            if (!roleCheck.Result)
            {
                var roleTask = RoleManager.CreateAsync(new IdentityRole("Admin"));
                roleTask.Wait();
            //Create the roles and seed to the database
            roleResult = roleTask.Result; 
            }

            var userTask = UserManager.FindByEmailAsync("cristinatcline@gmail.com");
            userTask.Wait();
            //Assign admin role to default admin user

            if(userTask.Result == null)
            {
                var newUserResult = UserManager.CreateAsync(new IdentityUser("cristinatcline@gmail.com"),"Password123!");
                newUserResult.Wait();              
            }

            userTask = UserManager.FindByEmailAsync("cristinatcline@gmail.com");
            userTask.Wait();

            IdentityUser user = userTask.Result; 
            var complete = UserManager.AddToRoleAsync(user, "Admin");
            complete.Wait();
        }
    }
}
