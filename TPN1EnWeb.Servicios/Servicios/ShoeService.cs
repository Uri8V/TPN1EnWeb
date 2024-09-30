using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades.Enums;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Servicios.Interfaces;
using System.Drawing.Drawing2D;
using TPN1EnWeb.Datos.Repositorios;

namespace TPN1EnWeb.Servicios.Servicios
{
    public class ShoeService : IShoeService
    {
        private readonly IShoeRepository _shoeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISizeRepository _sizeRepository;
        private readonly IShoeSizeRepository _shoeSizeRepository;
        public ShoeService(IShoeRepository shoeRepository, IUnitOfWork unitOfWork, ISizeRepository sizeRepository, IShoeSizeRepository shoeSizeRepository)
        {
            _shoeRepository = shoeRepository;
            _unitOfWork = unitOfWork;
            _sizeRepository = sizeRepository;
            _shoeSizeRepository = shoeSizeRepository;
        }

        public void AsignarSizealShoe(Shoe? shoeSinSize, Size? sizeSeleccionado, int Stock)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Crear una nueva relación entre el Shoe y el Size
                ShoeSizes nuevaRelacion = new ShoeSizes()
                {
                    ShoeSizeId = _shoeSizeRepository.GetId(),
                    Shoe = shoeSinSize!,
                    Size = sizeSeleccionado!,
                    QuantityInStock = Stock
                };

                _shoeRepository.AsignarSizeAlShoe(nuevaRelacion);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Borrar(Shoe shoe)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (shoe == null)
                {
                    throw new Exception("El shoe especificado no existe.");
                }

                // Eliminar relaciones con Sizes
                if (shoe.ShoeSize != null)
                {
                    foreach (var item in shoe.ShoeSize.ToList())
                    {
                        shoe.ShoeSize.Remove(item); //Borro las relaciones
                    }
                    _shoeRepository.Delete(shoe);//Cuando termino borro la planta
                }


                _unitOfWork.Commit(); // Confirmar los cambios
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Editar(Shoe shoe, int stock, int? value = null)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Editar la shoe
                _shoeRepository.Editar(shoe);

                if (value.HasValue)
                {
                    // Buscar el size
                    var size = _sizeRepository
                        .GetSizePorId(value.Value);
                    if (value != null)
                    {
                        // Crear la nueva relación si no existe
                        if (!shoe.ShoeSize
                            .Any(pp => pp.SizeId == size!.SizeId))
                        {
                            var nuevaRelacion = new ShoeSizes
                            {
                                ShoeSizeId = _shoeSizeRepository.GetId(),
                                ShoeId = shoe.ShoeId,
                                SizeId = size!.SizeId,
                                QuantityInStock = stock
                            };
                            _sizeRepository.AgregarSizeShoe(nuevaRelacion);

                        }
                    }
                    else
                    {
                        throw new Exception("Size no encontrado.");
                    }
                }

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        public bool Existe(Shoe Shoe)
        {
            return _shoeRepository.Existe(Shoe);
        }

        public bool ExisteRelacion(Shoe shoe, Size size)
        {
            return _shoeRepository.ExisteRelacion(shoe, size);
        }

        public int GetCantidad(Expression<Func<Shoe, bool>>? filtro)
        {
            return _shoeRepository.GetCantidad(filtro);
        }

        public List<ShoeListDto>? GetListaDeShoeSinSize()
        {
            return _shoeRepository.GetListaDeShoeSinSize();
        }

        public Shoe? GetShoe(Expression<Func<Shoe, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _shoeRepository.Get(filter, propertiesNames, tracked);
        }

        public IEnumerable<Shoe>? GetShoes(Expression<Func<Shoe, bool>>? filter = null, Func<IQueryable<Shoe>, IOrderedQueryable<Shoe>>? orderBy = null, string? propertiesNames = null)
        {
            try
            {
                return _shoeRepository.GetAll(filter, orderBy, propertiesNames);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorBrand()
        {
            return _shoeRepository.GetShoesAgrupadasPorBrand();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorColor()
        {
            return _shoeRepository.GetShoesAgrupadasPorColor();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorGenre()
        {
            return _shoeRepository.GetShoesAgrupadasPorGenre();
        }

        public IEnumerable<IGrouping<int, ShoeSizes>> GetShoesAgrupadasPorSize()
        {
            return _shoeRepository.GetShoesAgrupadasPorSize();
        }

        public IEnumerable<IGrouping<int, Shoe>> GetShoesAgrupadasPorSport()
        {
            return _shoeRepository.GetShoesAgrupadasPorSport();
        }

        public List<Size>? GetSizesPorShoes(int shoeId)
        {
            return _shoeRepository.GetSizesPorShoes(shoeId);
        }

        public void Guardar(Shoe Shoe, List<int> stock, List<Size>? sizes = null)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (Shoe.ShoeId == 0)
                {
                    _shoeRepository.Add(Shoe);
                    _unitOfWork.SaveChanges(); // Guardar cambios para obtener el id del Shoe agregado

                    if (sizes != null && sizes.Any())
                    {
                        _shoeRepository.AsignarSizeAlShoe(Shoe, sizes, stock);
                    }
                }
                else
                {
                    _shoeRepository.Editar(Shoe);
                    _unitOfWork.SaveChanges(); // Guardar cambios del shoe antes de manejar relaciones
                }
                _unitOfWork.SaveChanges(); // Guardar todos los cambios al final
                _unitOfWork.Commit(); // Confirmar los cambios
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Guardar(Shoe shoe)
        {

            try
            {
                _unitOfWork.BeginTransaction();
                if (shoe.ShoeId == 0)
                {
                    _shoeRepository.Add(shoe);
                }
                else
                {
                    _shoeRepository.Editar(shoe);
                }
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }

}
