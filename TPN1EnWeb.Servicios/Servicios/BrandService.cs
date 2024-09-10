using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Servicios.Interfaces;

namespace TPN1EnWeb.Servicios.Servicios
{
    public class BrandService:IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BrandService(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public void Borrar(Brand brand)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _brandRepository.Delete(brand);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public bool EstaRelacionado(Brand brand)
        {
            return _brandRepository!.EstaRelacionado(brand);
        }

        public bool Existe(Brand? brand)
        {
            return _brandRepository.Existe(brand);
        }

        public Brand? GetBrand(Expression<Func<Brand, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _brandRepository!.Get(filter, propertiesNames, tracked);
        }

        public IEnumerable<Brand>? GetBrands(Expression<Func<Brand, bool>>? filter = null, Func<IQueryable<Brand>, IOrderedQueryable<Brand>>? orderBy = null, string? propertiesNames = null)
        {
            return _brandRepository.GetAll(filter, orderBy, propertiesNames);
        }

        public int GetCantidad()
        {
            return _brandRepository.GetCantidad();
        }

        public void Guardar(Brand brand)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (brand.BrandId == 0)
                {
                    _brandRepository.Add(brand);
                }
                else
                {
                    _brandRepository.Editar(brand);
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
