using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Datos.Repositorios;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;

namespace TPN1EnWeb.IOC
{
    public static class DI
    {
        //Esto me permite Inicializar los servicios y repositorios una sola vez y después utilizarlos
        //injecctando, además esta clase va a cerrar sus recursos automaticamente como sucedia cuando usamos los using
        public static void ConfiguracionServicios(IServiceCollection servicios, IConfiguration conn)
        {
                                                    //Para que el escope funcione las clases deben heredar de las interfaces
            servicios.AddScoped<IBrandRepository, BrandRepository>();//Cada vez que nosotros le pidamos un IbranRepo, nos va mandar un BrandRepo, lo mismo con los demás
            servicios.AddScoped<IBrandService, BrandService>();

            servicios.AddScoped<ISportRepository, SportRepository>();
            servicios.AddScoped<ISportService, SportService>();

            servicios.AddScoped<IGenreRepository, GenreRepository>();
            servicios.AddScoped<IGenreService, GenreService>();

            servicios.AddScoped<IColorRepository, ColorRepository>();
            servicios.AddScoped<IColorService, ColorService>();

            servicios.AddScoped<IShoeRepository, ShoeRepository>();
            servicios.AddScoped<IShoeService, ShoeService>();

            servicios.AddScoped<ISizeRepository, SizeRepository>();
            servicios.AddScoped<ISizeService, SizeService>();

            servicios.AddScoped<IShoeSizeRepository, ShoeSizeRepository>();
            servicios.AddScoped<IShoeSizeService, ShoeSizeService>();

            servicios.AddScoped<IUnitOfWork, UnitOfWork>();

            servicios.AddDbContext<ShoesDbContext>(opciones =>
            {
                opciones.UseSqlServer(conn.GetConnectionString("Myconn")); //Ahora mi conexión ya no esta más hardcodeada,
                                                                           //sino que la va buscar al archivo appsettings.Json
            });

        }

    }
}
