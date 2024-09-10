using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades.Enums;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface IShoeRepository:IGenericRepository<Shoe>
    {
        void Editar(Shoe? Shoe, List<Size>? sizes = null);
        bool Existe(Shoe? Shoe);
        int GetCantidad(Expression<Func<Shoe, bool>>? filtro);
        List<ShoeListDto>? GetListaDeShoeSinSize();
        void AsignarSizeAlShoe(ShoeSizes nuevaRelacion);
        void AsignarSizeAlShoe(Shoe shoe, List<Size> sizes, List<int> Stock);
        void EliminarRelaciones(Shoe shoe);
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorBrand();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorSport();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorGenre();
        IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorColor();
        IEnumerable<IGrouping<int, ShoeSizes>> GetShoesAgrupadasPorSize();
        List<Size>? GetSizesPorShoes(int shoeId);
        bool ExisteRelacion(Shoe shoe, Size size);

    }
}
