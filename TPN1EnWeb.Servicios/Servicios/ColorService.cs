using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Servicios.Interfaces;
using System.Linq.Expressions;

namespace TPN1EnWeb.Servicios.Servicios
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ColorService(IColorRepository colorRepository, IUnitOfWork unitOfWork)
        {
            _colorRepository = colorRepository;
            _unitOfWork = unitOfWork;
        }

        public void Borrar(Colour Colour)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _colorRepository.Delete(Colour);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }
        }

        public bool EstaRelacionado(Colour Colour)
        {
            return _colorRepository.EstaRelacionado(Colour);
        }

        public bool Existe(Colour? Colour)
        {
            return _colorRepository.Existe(Colour);
        }

        public int GetCantidad()
        {
            return _colorRepository.GetCantidad();
        }

        public Colour? GetColour(Expression<Func<Colour, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true)
        {
            return _colorRepository.Get(filter,propertiesNames,tracked);
        }
        public IEnumerable<Colour>? GetColours(Expression<Func<Colour, bool>>? filter = null,
           Func<IQueryable<Colour>, IOrderedQueryable<Colour>>? orderBy = null,
           string? propertiesNames = null)
        {
            return _colorRepository.GetAll(filter,orderBy,propertiesNames);
        }

        public void Guardar(Colour Colour)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (Colour.ColourId == 0)
                {
                    _colorRepository.Add(Colour);
                }
                else
                {
                    _colorRepository.Editar(Colour);
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
