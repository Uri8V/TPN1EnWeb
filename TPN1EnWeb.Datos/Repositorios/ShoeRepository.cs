 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades.Enums;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class ShoeRepository:GenericRepository<Shoe>, IShoeRepository
    {
        private readonly ShoesDbContext _context;
        private readonly IShoeSizeRepository _shoeSizeRepository;//Lo necesito para obtener el Id para crear la nueva relacion
                                                                 // (No se me ocurrió otra forma)
        public ShoeRepository(ShoesDbContext context, IShoeSizeRepository shoeSizeRepository):base(context)
        {
            _context = context;
            _shoeSizeRepository = shoeSizeRepository;
        }

        public void Agregar(Shoe? shoe)
        {
            // Verificar si la Brand asociado
            // al Shoe ya existe en la base de datos
            var brandExistente = _context.Brands
                .FirstOrDefault(t => t.BrandId == shoe!.BrandId);

            // Si la Brand ya existe,
            // adjuntarlo al contexto en lugar de agregarlo nuevamente
            if (brandExistente != null)
            {
                _context.Attach(brandExistente);
                shoe!.Brands = brandExistente;
            }
            // Verificar si el Sport asociado
            // al Shoe ya existe en la base de datos

            var sportExistente = _context
                .Sports.FirstOrDefault(t => t.SportId == shoe!.SportId);

            // Si el Sport ya existe,
            // adjuntarlo al contexto en lugar de agregarlo nuevamente

            if (sportExistente != null)
            {
                _context.Attach(sportExistente);
                shoe!.Sports = sportExistente;
            }

            var colorExistente = _context
               .Colors.FirstOrDefault(t => t.ColourId == shoe!.ColourId);
            if (colorExistente != null)
            {
                _context.Attach(colorExistente);
                shoe!.Color = colorExistente;
            }

            var genreExistente = _context
             .Genres.FirstOrDefault(t => t.GenreId == shoe!.GenreId);
            if (genreExistente != null)
            {
                _context.Attach(genreExistente);
                shoe!.Genres = genreExistente;
            }

            // Agregar la planta al contexto de la base de datos
            _context.Shoes.Add(shoe!);
        }

        public void Editar(Shoe? shoe, List<Size>? sizes = null)
        {
            // Verificar si la Brand asociado
            // al Shoe ya existe en la base de datos
            var brandExistente = _context.Brands
                .FirstOrDefault(t => t.BrandId == shoe!.BrandId);

            // Si la Brand ya existe,
            // adjuntarlo al contexto en lugar de agregarlo nuevamente
            if (brandExistente != null)
            {
                _context.Attach(brandExistente);
                shoe!.Brands = brandExistente;
            }
            // Verificar si el Sport asociado
            // al Shoe ya existe en la base de datos

            var sportExistente = _context
                .Sports.FirstOrDefault(t => t.SportId == shoe!.SportId);

            // Si el Sport ya existe,
            // adjuntarlo al contexto en lugar de agregarlo nuevamente

            if (sportExistente != null)
            {
                _context.Attach(sportExistente);
                shoe!.Sports = sportExistente;
            }

            var colorExistente = _context
               .Colors.FirstOrDefault(t => t.ColourId == shoe!.ColourId);
            if (colorExistente != null)
            {
                _context.Attach(colorExistente);
                shoe!.Color = colorExistente;
            }

            var genreExistente = _context
             .Genres.FirstOrDefault(t => t.GenreId == shoe!.GenreId);
            if (genreExistente != null)
            {
                _context.Attach(genreExistente);
                shoe!.Genres = genreExistente;
            }

            // Agregar la planta al contexto de la base de datos

            _context.Shoes.Update(shoe!);
        }

        public bool Existe(Shoe? Shoe)
        {
            if (Shoe?.ShoeId == 0)
            {
                return _context.Shoes.Any(s => s.SportId == Shoe.SportId && s.BrandId == Shoe.BrandId && s.GenreId == Shoe.GenreId && s.ColourId == Shoe.ColourId);
            }
            return _context.Shoes.Any(s => s.SportId == Shoe!.SportId && s.BrandId == Shoe.BrandId && s.GenreId == Shoe.GenreId && s.ColourId == Shoe.ColourId && s.ShoeId != Shoe.ShoeId);
        }

        public int GetCantidad(Expression<Func<Shoe, bool>>? filtro)//xpresión lambda de tipo Expression<Func<Shoe, bool>>?,
                                                                    //que se utiliza como filtro para contar solo los registros que cumplan con cierta condición.
        {

            if (filtro != null)
            {
                return _context.Shoes.AsQueryable().Where(filtro).Count();
                //AsQueryable() convierte la tabla Shoes en una consulta IQueryable,
                //lo cual es necesario para aplicar la expresión lambda.
                //Where(filtro) aplica el filtro especificado por la expresión lambda filtro.
            }
            else
            {
                return _context.Shoes.Count();
            }

        }

        public List<ShoeListDto>? GetListaDeShoeSinSize()
        {
            return _context.Shoes //Le indico que me traiga una lista de tipo SHOELISTDTO, la cual va a incluir los datos de las tablas
                .Include(b => b.Brands)//simples y me va a traer todos los Shoes los cuales no tengan relacionado algún Size
                .Include(s => s.Sports)
                .Include(c => c.Color)
                .Include(g => g.Genres)
                .Where(sz => !sz.ShoeSize.Any())
                .Select(sh => new ShoeListDto
                {
                    shoeId = sh.ShoeId,
                    brand = sh.Brands!.BrandName,
                    sport = sh.Sports!.SportName,
                    color = sh.Color!.ColorName,
                    genre = sh.Genres!.GenreName,
                    descripcion = sh.Descripcion,
                    price = sh.Price,
                    model = sh.Model
                }).ToList();
        }

        public void AsignarSizeAlShoe(ShoeSizes nuevaRelacion)
        {
            _context.Set<ShoeSizes>().Add(nuevaRelacion);//Indico que en la propiedad ShoeSizes me agregue una nueva relación

        }

        public void AsignarSizeAlShoe(Shoe shoe, List<Size> sizes, List<int> stocks)
        {
            var id = _shoeSizeRepository.GetId();
            for (int i = 0; i < sizes.Count; i++)
            {
                var size = sizes[i];
                var stock = stocks[i];
                var sizeExistente = _context.Sizes
                    .FirstOrDefault(p => p.SizeId == size.SizeId);

                if (sizeExistente == null)
                {
                    _context.Sizes.Add(size); // Agregar nuevo size, en este caso nunca va a ser nuevo 
                    //a menos que lo configure para que pueda agregar uno nuevo
                    sizeExistente = size; // Establecer sizeExistente como el nuevo size
                }
                else
                {
                    _context.Sizes.Attach(sizeExistente); // Attach si el size ya existe y está detached
                }

                if (!ExisteRelacion(shoe, sizeExistente))
                {
                    _context.ShoeSizes.Add(new ShoeSizes
                    {
                        ShoeSizeId = id,
                        ShoeId = shoe.ShoeId,
                        SizeId = sizeExistente.SizeId,
                        QuantityInStock = stock
                    });
                }
                id++;
            }

        }

        private bool ExisteRelacion(Shoe shoe, Size sizeExistente)
        {
            if (shoe == null || sizeExistente == null) return false;

            return _context.ShoeSizes
                .Any(pp => pp.ShoeId == shoe.ShoeId
                && pp.SizeId == sizeExistente.SizeId);
        }

        public void EliminarRelaciones(Shoe shoe)
        {
            var relacionesPasadas = _context.ShoeSizes
               .Where(pp => pp.ShoeId == shoe.ShoeId)
               .ToList();

            _context.ShoeSizes
                .RemoveRange(relacionesPasadas);
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorBrand()
        {
            return _context.Shoes.GroupBy(pp => pp.BrandId).ToList();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorSport()
        {
            return _context.Shoes.GroupBy(pp => pp.SportId).ToList();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorGenre()
        {
            return _context.Shoes.GroupBy(_ => _.GenreId).ToList();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorColor()
        {
            return _context.Shoes.GroupBy(ss => ss.ColourId).ToList();
        }

        public IEnumerable<IGrouping<int, ShoeSizes>> GetShoesAgrupadasPorSize()
        {
            return _context.ShoeSizes.GroupBy(ss => ss.SizeId).ToList();
        }

        public List<Size>? GetSizesPorShoes(int shoeId)
        {
            return _context.ShoeSizes.Include(_ => _.Size)
                .Where(s => s.ShoeId == shoeId)
                .Select(s => s.Size)
                .ToList();
        }

        bool IShoeRepository.ExisteRelacion(Shoe shoe, Size size)
        {
            if (shoe is null || size is null) return false;
            var existerelación = _context.Shoes.Include(ss => ss.ShoeSize)
                .ThenInclude(ss => ss.Size)
                .Any(ss => ss.ShoeId == shoe.ShoeId &&
                ss.ShoeSize.Any(ss => ss.SizeId == size.SizeId));
            return existerelación;
        }
    }
}
