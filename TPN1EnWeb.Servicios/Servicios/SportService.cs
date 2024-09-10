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
    public class SportService : ISportService
    {
        private readonly ISportRepository _sportRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SportService(ISportRepository sportRepository, IUnitOfWork unitOfWork)
        {
            _sportRepository = sportRepository;
            _unitOfWork = unitOfWork;
        }

        public void Borrar(Sport sport)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _sportRepository.Delete(sport);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }
        }

        public bool EstaRelacionado(Sport sport)
        {
            return _sportRepository.EstaRelacionado(sport);
        }

        public bool Existe(Sport? sport)
        {
            return _sportRepository.Existe(sport);
        }

        public int GetCantidad()
        {
            return _sportRepository.GetCantidad();
        }

        public Sport? GetSport(Expression<Func<Sport, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true)
        {
            return _sportRepository.Get(filter,propertiesNames,tracked);
        }
        public IEnumerable<Sport>? GetSports(Expression<Func<Sport, bool>>? filter = null,
           Func<IQueryable<Sport>, IOrderedQueryable<Sport>>? orderBy = null,
           string? propertiesNames = null)
        {
            return _sportRepository.GetAll(filter,orderBy,propertiesNames);
        }

        public void Guardar(Sport sport)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (sport.SportId == 0)
                {
                    _sportRepository.Add(sport);
                }
                else
                {
                    _sportRepository.Editar(sport);
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
