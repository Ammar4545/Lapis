using Lapis_DataAcess;
using Lapis_DataAcess.Repository;
using Lapis_DataAcess.Repository.IRepository;
using Lapis_Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lapis
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
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

             /*[services.AddDefaultIdentity<IdentityUser>()]] basic authentication, user registration, and login functionality.
             not contain configuring Roles*/
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders() //Provides Token when forgeting password.
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
            services.AddScoped<IInquiryDetailRepository, InquiryDetailRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            //session configuration
            services.AddHttpContextAccessor();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(10);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();              
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
