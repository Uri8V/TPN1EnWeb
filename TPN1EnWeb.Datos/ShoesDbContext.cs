using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos
{
    public class ShoesDbContext:IdentityDbContext<IdentityUser> // Al heredar de IdentityDbContext<IdentityUser>, estás integrando las tablas y configuraciones necesarias para usar ASP.NET Identity, un sistema que gestiona usuarios, roles y autenticación.
                                                                //Como ahora mi proyecto va a utilizar Identity mi contexto de una DBContext pasa a ser un IdentityDBContext
                                                                //Para eso debo instalar el paquete Microsoft.AspNetCore.Identity.EntityFrameworkCore
                                                                //PARA PODER INSTALAR EL PAQUETE TUVO QUE ACTUALIZAR MI PROYECTO A .NET 9
    {
        //EN ESTA OCASIÓN NO DEBO HACER NINGUNA MIGRACIÓN DEBIDO A QUE LA BASE DE DATOS YA ESTA CREADA
        // CONFIGURADA, Y PARA AHORRAR TIEMPO NO VAMOS A CREAR OTRA
        public ShoesDbContext()
        {

        }
        public ShoesDbContext(DbContextOptions<ShoesDbContext> options) : base(options)
        {

        }
        //Creo la conexión para la base de datos que voy a crear y postereormente usar
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.; Initial Catalog=ShoesEFCore; Trusted_Connection=True;
                        TrustServerCertificate=True");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        }
        //Estos DbSet se los podría considerar tablas que se van a crear en la Base de Datos
        //estos son muy importantes ya que de estos la migración puede darse una idea de como crear la entidad en la BD
        //las propiedades de las entidades son las que le dan forma a las tablas, le indican cuantas columnas y sus respectivos tipos de datos 
        public DbSet<Brand> Brands { get; set; } //Acordarse de poner los DbSet como publicos para poder usarlas en otras capas
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Colour> Colors { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<TPN1EnWeb.Entidades.Size> Sizes { get; set; }
        public DbSet<ShoeSizes> ShoeSizes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                        .HasMany(au => au.OrderHeaders)
                        .WithOne(oh => oh.ApplicationUser)
                        .HasForeignKey(oh => oh.ApplicationUserId);

            modelBuilder.Entity<OrderDetail>()
                         .HasOne(od => od.ShoeSizes)
                         .WithMany()
                         .HasForeignKey(od => od.ShoeSizeId)
                         .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                // Configura la relación con ShoeSize
                entity.HasOne(sc => sc.ShoeSize)
                      .WithMany() // Sin relación inversa, ya que ShoeSize no necesita conocer ShoppingCart
                      .HasForeignKey(sc => sc.ShoeSizeId)
                      .OnDelete(DeleteBehavior.Restrict); // O el comportamiento de tu preferencia

                // Configura la relación con ApplicationUser
                entity.HasOne(sc => sc.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(sc => sc.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Cascade); // Borra el carrito si el usuario es eliminado
            });
            modelBuilder.Entity<ShoeSizes>(entity =>
            {
                entity.HasKey(ss => ss.ShoeSizeId); //Indico que la key va a ser la combinación de estos ID's
                entity.HasOne(ss => ss.Shoe).WithMany(s => s.ShoeSize).HasForeignKey(ss => ss.ShoeId); //Indico que un Shoe va a tener muchos ShoeSize con la clave foranea ShoeId
                entity.HasOne(ss => ss.Size).WithMany(s => s.ShoeSize).HasForeignKey(ss => ss.SizeId);//Indico que un Size va a tener muchos ShoeSize con la clave foranea SizeId
                entity.Property(ss => ss.QuantityInStock).IsRequired();// Indico que esta propiedad es requerida
                entity.HasIndex(ss => new { ss.SizeId, ss.ShoeId }).IsUnique();//Indico que la combinacion entre estos ID's sea único
                entity.HasIndex(ss => ss.ShoeSizeId).IsUnique();//Indico que el Id sea único
            });
            var brandList = new List<Brand>()
            {
                new ()
                {
                    BrandId = 1,
                    BrandName="Nike"
                },
                new ()
                {
                    BrandId=2,
                    BrandName="Adidas"
                },
                new ()
                {
                    BrandId=3,
                    BrandName="Puma"
                }
            };
            var colorList = new List<Colour>()
            {
                new ()
                {
                    ColourId=1,
                    ColorName="Lila"
                },
                new ()
                {
                    ColourId=2,
                    ColorName="Violeta"
                },
                new ()
                {
                    ColourId=3,
                    ColorName="Purpura"
                }
            };
            var genreList = new List<Genre>()
            {
                new()
                {
                    GenreId=1,
                    GenreName="Masculino"
                },
                new()
                {
                    GenreId=2,
                    GenreName="Femenino"
                },
                new()
                {
                    GenreId=3,
                    GenreName="Unisex"
                }
            };
            var sportList = new List<Sport>()
            {
                new()
                {
                    SportId=1,
                    SportName="Futbol"
                },
                new()
                {
                    SportId=2,
                    SportName="Voley"
                },
                new()
                {
                    SportId=3,
                    SportName="Hokey"
                }
            };

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brands");//Le decimos en que tabla trabajar
                entity.HasIndex(p => p.BrandName).IsUnique();//Acá hicimos que la propiedad sea única
                entity.Property(p => p.BrandName).HasMaxLength(50);//Le decimos que esta propiedad tenga un máximo de longitud de 50
                entity.HasData(brandList);//Agregamos una lista de datos a la tabla
                entity.Property(p => p.Active).HasDefaultValue(true);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genres");
                entity.HasIndex(p => p.GenreName).IsUnique();
                entity.Property(p => p.GenreName).HasMaxLength(10);
                entity.HasData(genreList);
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.ToTable("Sports");
                entity.HasIndex(p => p.SportName).IsUnique();
                entity.Property(p => p.SportName).HasMaxLength(20);
                entity.HasData(sportList);
                entity.Property(p => p.Active).HasDefaultValue(true);
            });
            modelBuilder.Entity<Colour>(entity =>
            {
                entity.ToTable("Colors");
                entity.HasIndex(p => p.ColorName).IsUnique();
                entity.Property(p => p.ColorName).HasMaxLength(50);
                entity.HasData(colorList);
                entity.Property(p => p.Active).HasDefaultValue(true);
            });

            //Acá modificamos las propiedades que deseamos
            modelBuilder.Entity<Shoe>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal (10,2)");
                entity.Property(p => p.Model).HasMaxLength(150);
                entity.Property(p => p.Active).HasDefaultValue(true);
            });
            var sizeLISt = new List<TPN1EnWeb.Entidades.Size>()
            {
                new(){SizeId=1,SizeNumber=28},
                new(){SizeId=2, SizeNumber=28.5m},
                new(){SizeId=3, SizeNumber=29},
                new(){SizeId=4, SizeNumber=29.5m},
                new(){SizeId=5, SizeNumber=30},
                new(){SizeId=6, SizeNumber=30.5m},
                new(){SizeId=7, SizeNumber=31},
                new(){SizeId=8, SizeNumber=31.5m},
                new(){SizeId=9, SizeNumber=32},
                new(){SizeId=10, SizeNumber=32.5m},
                new(){SizeId=11, SizeNumber=33},
                new(){SizeId=12, SizeNumber=33.5m},
                new(){SizeId=13, SizeNumber=34},
                new(){SizeId=14, SizeNumber=34.5m},
                new(){SizeId=15, SizeNumber=35},
                new(){SizeId=16, SizeNumber=35.5m},
                new(){SizeId=17, SizeNumber=36},
                new(){SizeId=18, SizeNumber=36.5m},
                new(){SizeId=19, SizeNumber=37},
                new(){SizeId=20, SizeNumber=37.5m},
                new(){SizeId=21, SizeNumber=38},
                new(){SizeId=22, SizeNumber=38.5m},
                new(){SizeId=23, SizeNumber=39},
                new(){SizeId=24, SizeNumber=39.5m},
                new(){SizeId=25, SizeNumber=40},
                new(){SizeId=26, SizeNumber=40.5m},
                new(){SizeId=27, SizeNumber=41},
                new(){SizeId=28, SizeNumber=41.5m},
                new(){SizeId=29, SizeNumber=42},
                new(){SizeId=30, SizeNumber=42.5m},
                new(){SizeId=31, SizeNumber=43},
                new(){SizeId=32, SizeNumber=43.5m},
                new(){SizeId=33, SizeNumber=44},
                new(){SizeId=34, SizeNumber=44.5m},
                new(){SizeId=35, SizeNumber=45},
                new(){SizeId=36, SizeNumber=45.5m},
                new(){SizeId=37, SizeNumber=46},
                new(){SizeId=38, SizeNumber=46.5m},
                new(){SizeId=39, SizeNumber=47},
                new(){SizeId=40, SizeNumber=47.5m},
                new(){SizeId=41, SizeNumber=48},
                new(){SizeId=42, SizeNumber=48.5m},
                new(){SizeId=43, SizeNumber=49},
                new(){SizeId=44, SizeNumber=49.5m},
                new(){SizeId=45, SizeNumber=50}
            };

            modelBuilder.Entity<TPN1EnWeb.Entidades.Size>(entity =>
            {
                entity.Property(p => p.SizeNumber).HasColumnType("decimal (3,1)");
                entity.HasData(sizeLISt);//Utilizas HasData para precargar la base de datos con valores predeterminados al realizar migraciones. Esto es útil para garantizar que ciertas tablas (como Brands o Genres) tengan datos desde el inicio
            });

            // Configurar la clave primaria para IdentityUserLogin<string> (Este IdentityUserLogin es de tipo string para que los IDs de usuario se verán como GUIDs (ejemplo: "3fcb5a07-dc3d-4f58-8bc4-25b3bdfd2561"))
            modelBuilder.Entity<IdentityUserLogin<string>>()
                   .HasKey(login => new { login.LoginProvider, login.ProviderKey });
            //LoginProvider: Es un identificador del proveedor externo de inicio de sesión (por ejemplo, "Google", "Facebook", o "Microsoft")
            //ProviderKey: Es el identificador único que el proveedor externo utiliza para identificar al usuario. Por ejemplo:
            //En el caso de Google, esto sería el ID único del usuario en Google.
            //En el caso de Facebook, sería el ID único del usuario en Facebook.

            //La combinación de estas dos propiedades es única por cada inicio de sesión externo. Al usarlas como clave primaria compuesta:
            //Garantizamos unicidad: Evitamos duplicados en la tabla AspNetUserLogins. Por ejemplo, el mismo usuario no debería tener dos entradas con el mismo LoginProvider y ProviderKey.
            //Facilitamos consultas rápidas: ASP.NET Identity necesita buscar rápidamente si un usuario ya tiene un inicio de sesión externo registrado.
            //Es una convención estándar: Esta estructura ya está definida por diseño en ASP.NET Identity y es consistente con las mejores prácticas.




            // Configurar la clave primaria compuesta de IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId }); // Clave primaria compuesta
            });
            //Usando las propiedades UserId y RoleId. Esto asegura que:
            //Un mismo usuario no pueda estar en el mismo rol más de una vez.
            //El sistema pueda buscar y eliminar relaciones de usuario - rol de manera eficiente


            //IdentityUserToken<string> Sirve para crear tokens personalizados
            //Un token es un valor que puede representar información sobre un usuario o proporcionar acceso temporal a ciertos recursos.
            //Ejemplos comunes:
            //Tokens para autenticación persistente(como cookies o JWT).
            //Tokens para reiniciar contraseñas o realizar verificaciones de correo electrónico.
            //Tokens de integración con otros servicios(como OAuth o API externas).
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name }); // Clave primaria compuesta
            });
            //UserId: Un usuario puede tener varios tokens, pero cada token pertenece a un usuario específico.
            //LoginProvider: Permite distinguir tokens emitidos por diferentes proveedores(como Google, Facebook, etc.).
            //Name: Permite que un mismo proveedor emita diferentes tipos de tokens para el mismo usuario(como AccessToken y RefreshToken).
            //Con esta combinación, el sistema asegura que:
            //No existan tokens duplicados para un mismo usuario, proveedor y nombre.
            //Se pueda identificar y eliminar un token específico de forma eficiente.

            modelBuilder.Entity("TPN1EnWeb.Entidades.Country", b =>
            {
                b.Property<int>("CountryId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                b.Property<string>("CountryName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("CountryId");

                b.ToTable("Countries");
            });

            modelBuilder.Entity("TPN1EnWeb.Entidades.State", b =>
            {
                b.Property<int>("StateId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateId"));

                b.Property<int>("CountryId")
                    .HasColumnType("int");

                b.Property<string>("StateName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("StateId");

                b.HasIndex("CountryId");

                b.ToTable("States");
            });

            modelBuilder.Entity("TPN1EnWeb.Entidades.City", b =>
            {
                b.Property<int>("CityId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                b.Property<string>("CityName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("CountryId")
                    .HasColumnType("int");

                b.Property<int>("StateId")
                    .HasColumnType("int");

                b.HasKey("CityId");

                b.HasIndex("CountryId");

                b.HasIndex("StateId");

                b.ToTable("Cities");
            });

            modelBuilder.Entity("TPN1EnWeb.Entidades.State", b =>
            {
                b.HasOne("TPN1EnWeb.Entidades.Country", "Country")
                    .WithMany()
                    .HasForeignKey("CountryId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Country");
            });

            modelBuilder.Entity("TPN1EnWeb.Entidades.City", b =>
            {
                b.HasOne("TPN1EnWeb.Entidades.Country", "Country")
                    .WithMany()
                    .HasForeignKey("CountryId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("TPN1EnWeb.Entidades.State", "State")
                    .WithMany()
                    .HasForeignKey("StateId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Country");

                b.Navigation("State");
            });
         
        }

    }
}
