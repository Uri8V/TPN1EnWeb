﻿using System;
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
    public class CitiesService : ICitiesService
    {
        private readonly ICitiesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CitiesService(ICitiesRepository? repository, IUnitOfWork? unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public bool Exist(City city)
        {
            if (_repository is null)
            {
                throw new ApplicationException("Dependency not set");
            }
            return _repository.Exist(city);
        }

        public IEnumerable<City> GetAll(Expression<Func<City, bool>>? filter = null,
            Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null, string? propertiesNames = null)
        {
            if (_repository == null)
            {
                throw new ApplicationException("Dependency not set");
            }
            return _repository.GetAll(filter, orderBy, propertiesNames)!;
        }

        public City? Get(Expression<Func<City, bool>> filter, string? propertiesNames = null, bool tracked = true)
        {
            if (_repository == null)
            {
                throw new ApplicationException("Dependency not set");
            }

            return _repository.Get(filter, propertiesNames, tracked);
        }

        public bool ItsRelated(int cityId)
        {
            return _repository!.ItsRelated(cityId);
        }

        public void Remove(City city)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                _repository?.Delete(city);
                _unitOfWork?.Commit();

            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }

        }

        public void Save(City city)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (city.CityId == 0)
                {
                    _repository?.Add(city);
                }
                else
                {
                    _repository?.Update(city);
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
