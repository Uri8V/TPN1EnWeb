using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class GenreRepository:GenericRepository<Genre>,IGenreRepository
    {
        private readonly ShoesDbContext context;
        public GenreRepository(ShoesDbContext _context) : base(_context) //Ya que cuando instancio el repo de Genres, tambien instancio el generico
        {
            context = _context;
        }
        public void Editar(Genre genre)
        {
            context.Genres.Update(genre);
        }
        public bool EstaRelacionado(Genre? Genre)
        {
            return context.Shoes.Any(g => g.GenreId == Genre.GenreId);
        }
        public bool Existe(Genre? Genre)
        {
            if (Genre.GenreId == 0)
            {
                return context.Genres.Any(b => b.GenreName == Genre.GenreName);
            }
            return context.Genres.Any(b => b.GenreName == Genre.GenreName && b.GenreId != Genre.GenreId);
        }
        public int GetCantidad()
        {
            return context.Genres.Count();
        }

    }
}
