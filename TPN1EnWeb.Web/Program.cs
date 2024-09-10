using TPN1EnWeb.IOC;

namespace TPN1EnWeb.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
           
            DI.ConfiguracionServicios(builder.Services, builder.Configuration);//Ac� injecto todos los servicios y la conexi�n con la DB

            builder.Services.AddAutoMapper(typeof(Program).Assembly);//Injecto el AutoMapper

            var app = builder.Build();

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

            app.UseAuthorization();

            app.MapControllerRoute(//Indico como voy a mapear las rutas que le paso en la barra de navegaci�n
                name: "default",
                //Puede tener recibir un controlador, sino por defecto es Home, puede recibir una acci�n, sino por defecto es Index
                //y podr�a llegar a recibir un parametro (este es opcional)
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
