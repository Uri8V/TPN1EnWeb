using TPN1EnWeb.IOC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Utilities;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;

namespace TPN1EnWeb.Web
{
    public class Program
    {
        public static async Task Main(string[] args) //PREGUNTAR QUÉ ES ASYNC? POR QUÉ? PARA QUÉ SIRVE? LO MISMO TASK
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ShoesDbContext>().AddDefaultTokenProviders();//Para que le designe un token por defecto al usuario que se quiere registrar
            builder.Services.ConfigureApplicationCookie(options =>                                                //Esto serviria si yo quisiese enviar un email de confirmacion
            {
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
            });

            builder.Services.AddRazorPages();//Con esto puedo trabajar con las páginas Razor las cuales son las de ingreso y registro a la página
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            DI.ConfiguracionServicios(builder.Services, builder.Configuration);//Ac� injecto todos los servicios y la conexi�n con la DB

            builder.Services.AddAutoMapper(typeof(Program).Assembly);//Injecto el AutoMapper

            var app = builder.Build();

            using (var scope = app.Services.CreateScope()) //Para utilizar el método de abajo utilizo este Scope
            {
                var services = scope.ServiceProvider; //Ya que solicita los servicios y se los injecta al método de abajo
                await SeedRolesAndAdminUser(services);
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();//Indico que utilizo archivos estaticos (estos estan en WWWROOT)

            app.UseRouting();

            app.UseAuthentication();//Se fijo que usuario es y el tipo

            app.UseAuthorization();
            app.MapRazorPages();//Para el mapeo de las Páginas Razor
            app.MapControllerRoute(//Indico como voy a mapear las rutas que le paso en la barra de navegaci�n
                name: "default",
                //Puede tener recibir un controlador, sino por defecto es Home, puede recibir una acci�n, sino por defecto es Index
                //y podr�a llegar a recibir un parametro (este es opcional)
                pattern: "{area=Customer}/{controller=Home}/{action=Hero}/{id?}"); //Ahora como tenemos un area se la debemos agregar al patr�n, adem�s debo pasarle
                                                                                   //el _viewimport y el _viewstart tanto a las vistas del area admin como a customer

            app.Run();
        }
        private static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //Traigo el manejador de roles
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); //Traigo el manejador de usuarios

            if (!await roleManager.RoleExistsAsync(WC.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Admin)); //Si no existe el rol Admin me lo crea
            }
            if (!await roleManager.RoleExistsAsync(WC.Role_Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Customer));//Si no existe el rol Customer me lo crea
            }

            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com"); //Se fija si existe un usuario de tipo administrador el cual va a trabajar con este correo electronico
            if (adminUser == null) //Si no lo encuentra lo crea 
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123."); //Me lo agrega con esta contraseña

                // Asignar el rol de Admin al usuario
                await userManager.AddToRoleAsync(adminUser, WC.Role_Admin); //Y por último lo agrega al rol de Admin
            }

        }
    }
}
