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
    public class ShoeSizeService : IShoeSizeService
    {
        private readonly IShoeSizeRepository shoeSizeRepo; private readonly IUnitOfWork _unitOfWork;

        public ShoeSizeService(IShoeSizeRepository _shoeSizeRepo, IUnitOfWork unitOfWork)
        {
            shoeSizeRepo = _shoeSizeRepo;
            _unitOfWork = unitOfWork;
        }

        public ShoeSizes? GetShoeSize(Expression<Func<ShoeSizes, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return shoeSizeRepo.Get(filter,propertiesNames,tracked);
        }

        public IEnumerable<ShoeSizes>? GetShoeSizes(Expression<Func<ShoeSizes, bool>>? filter = null, Func<IQueryable<ShoeSizes>, IOrderedQueryable<ShoeSizes>>? orderBy = null, string? propertiesNames = null)
        {
            return shoeSizeRepo.GetAll(filter, orderBy, propertiesNames);
        }

        public int GetId()
        {
            return shoeSizeRepo.GetId();
        }

        public ShoeSizes GetIdShoeSize(int size, int shoe)
        {
            return shoeSizeRepo.GetIdShoeSize(size, shoe);
        }
        public void Editar(ShoeSizes shoeSizes)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                shoeSizeRepo.Editar(shoeSizes);
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
