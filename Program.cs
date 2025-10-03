using EbayChat.Entities;
using EbayChat.Hubs;
using EbayChat.Services.ServicesImpl;
using Microsoft.EntityFrameworkCore;

namespace EbayChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<CloneEbayDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Denpendency injection for services
            builder.Services.AddScoped<Services.IUserServices, UserServices>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            // Add session support
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();



            app.MapStaticAssets();
            app.MapHub<ChatHub>("/chatHub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();


            app.Run();
        }
    }
}
