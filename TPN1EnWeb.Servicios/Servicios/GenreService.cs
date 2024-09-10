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
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GenreService(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
        {
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
        }

        public void Borrar(Genre genre)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _genreRepository?.Delete(genre);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }
        }

        public bool EstaRelacionado(Genre? genre)
        {
            return _genreRepository.EstaRelacionado(genre);
        }

        public bool Existe(Genre? genre)
        {
            return _genreRepository.Existe(genre);
        }

        public int GetCantidad()
        {
            return _genreRepository.GetCantidad();
        }

        public Genre? GetGenre(Expression<Func<Genre, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true)
        {
            return _genreRepository.Get(filter,propertiesNames,tracked);
        }

        public IEnumerable<Genre>? GetGenres(Expression<Func<Genre, bool>>? filter = null,
           Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy = null,
           string? propertiesNames = null)
        {
            return _genreRepository.GetAll(filter,orderBy,propertiesNames);
        }

        public void Guardar(Genre genre)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (genre.GenreId == 0)
                {
                    _genreRepository.Add(genre);
                }
                else
                {
                    _genreRepository.Editar(genre);
                    //CUANDO EDITO UN DATO ME DICE QUE SE TRUNCAN LOS VALORES
                    //Valor truncado: "<p>aaaaaaa".
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
