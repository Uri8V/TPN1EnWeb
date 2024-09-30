using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades.Enums;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IShoeService
    {
        void Borrar(Shoe shoe);
        void Guardar(Shoe shoe);
        void Guardar(Shoe Shoe, List<int> stock, List<Size>? sizes = null);
        bool Existe(Shoe Shoe);
        int GetCantidad(Expression<Func<Shoe, bool>>? filtro);
        List<ShoeListDto>? GetListaDeShoeSinSize();
        void AsignarSizealShoe(Shoe shoeSinSize, Size? sizeSeleccionado, int Stock);
        void Editar(Shoe shoe, int stock, int? value = null);
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorBrand();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorSport();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorGenre();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorColor();
        IEnumerable<IGrouping<int, ShoeSizes>> GetShoesAgrupadasPorSize();
        List<Size>? GetSizesPorShoes(int shoeId);
        bool ExisteRelacion(Shoe shoe, Size size);
        IEnumerable<Shoe>? GetShoes(Expression<Func<Shoe, bool>>? filter = null,
          Func<IQueryable<Shoe>, IOrderedQueryable<Shoe>>? orderBy = null,
          string? propertiesNames = null);
        Shoe? GetShoe(Expression<Func<Shoe, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true);
    }
}
