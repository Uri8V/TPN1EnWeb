using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Datos;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Servicios.Interfaces;

namespace TPN1EnWeb.Servicios.Servicios
{
    public class ApplicationUsersService : IApplicationUsersService
    {
        private readonly IApplicationUsersRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ApplicationUsersService(IApplicationUsersRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(ApplicationUser applicationUser)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(applicationUser);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }


        public ApplicationUser? Get(Expression<Func<ApplicationUser, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<ApplicationUser> GetAll(Expression<Func<ApplicationUser, bool>>? filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames)!;
        }



        public void Save(ApplicationUser applicationUser)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (applicationUser.Id == string.Empty)
                {
                    _repository?.Add(applicationUser);
                }
                else
                {
                    _repository?.Update(applicationUser);
                }
                _unitOfWork?.Commit();

            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }
        }

    }
}
