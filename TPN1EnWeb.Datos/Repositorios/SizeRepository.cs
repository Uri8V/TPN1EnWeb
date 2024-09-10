using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Datos.Interfaces;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class SizeRepository:GenericRepository<Size>,ISizeRepository
    {
        private readonly ShoesDbContext context;
        public SizeRepository(ShoesDbContext _context):base(_context)
        {
            context = _context;
        }

        public void AgregarSizeShoe(ShoeSizes nuevarelacion)
        {
            context.Set<ShoeSizes>().Add(nuevarelacion);
        }
        public Size? GetSizePorId(int id, bool incluyeShoe = false)
        {   //El id que esta como parametro es de Size
            var query = context.Sizes;
            if (incluyeShoe)//Indica si debemos incluir los Shoes asociados a este Size
            {
                return query.Include(s => s.ShoeSize)
                    .ThenInclude(ss => ss.Shoe)//Con este permito que me traiga los datos de los Shoes
                    .FirstOrDefault(s => s.SizeId == id);//Este me trae el Size y los Shoes que tiene relacionados
            }
            return query.FirstOrDefault(s => s.SizeId == id);//Este me trae el Size que y le pido
        }
        public List<ShoeListDto> GetShoePoSize(Size size)
        {
            return context.Shoes //Le indico que me traiga una lista de tipo SHOELISTDTO, la cual va a incluir los datos de las tablas
              .Include(b => b.Brands)//simples y me va a traer todos los Shoes los cuales no tengan relacionado algún Size
              .Include(s => s.Sports)
              .Include(c => c.Color)
              .Include(g => g.Genres)
              .Include(ss => ss.ShoeSize)
              .Where(sz => sz.ShoeSize.Any(s => s.Size.SizeId == size.SizeId))
              .Select(sh => new ShoeListDto
              {
                  shoeId = sh.ShoeId,
                  brand = sh.Brands!.BrandName,
                  sport = sh.Sports!.SportName,
                  color = sh.Color!.ColorName,
                  genre = sh.Genres!.GenreName,
                  descripcion = sh.Descripcion,
                  price = sh.Price,
                  model = sh.Model,
              }).ToList();
        }
    }

}

