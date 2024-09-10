using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Entidades;
using System.Linq.Expressions;

namespace TPN1EnWeb.Servicios.Servicios
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SizeService(ISizeRepository sizeRepository, IUnitOfWork unitOfWork)
        {
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
        }

        public List<ShoeListDto> GetShoePorSize(Size size)
        {
            return _sizeRepository.GetShoePoSize(size);
        }
        public Size? GetSize(Expression<Func<Size, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true)
        {
            return _sizeRepository.Get(filter, propertiesNames, tracked);
        }

        public Size? GetSizePorId(int id, bool incluyeShoe = false)
        {
            return _sizeRepository.GetSizePorId(id, incluyeShoe);
        }

        public IEnumerable<Size> GetSizes(Expression<Func<Size, bool>>? filter = null,
           Func<IQueryable<Size>, IOrderedQueryable<Size>>? orderBy = null,
           string? propertiesNames = null)
        {
            return _sizeRepository?.GetAll(filter,orderBy,propertiesNames)!;
        }

    }

}
