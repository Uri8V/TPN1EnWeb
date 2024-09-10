using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos
{
    public class ShoesDbContext:DbContext
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
        }
        //Estos DbSet se los podría considerar tablas que se van a crear en la Base de Datos
        //estos son muy importantes ya que de estos la migración puede darse una idea de como crear la entidad en la BD
        //las propiedades de las entidades son las que le dan forma a las tablas, le indican cuantas columnas y sus respectivos tipos de datos 
        public DbSet<Brand> Brands { get; set; } //Acordarse de poner los DbSet como publicos para poder usarlas en otras capas
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Colour> Colors { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ShoeSizes> ShoeSizes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoeSizes>(entity =>
            {
                entity.HasKey(ss => new { ss.ShoeId, ss.SizeId }); //Indico que la key va a ser la combinación de estos ID's
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
            var sizeLISt = new List<Size>()
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

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(p => p.SizeNumber).HasColumnType("decimal (3,1)");
                entity.HasData(sizeLISt);
            });
        }

    }
}
