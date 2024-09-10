using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class BrandRepository : GenericRepository<Brand>,IBrandRepository
    {
        private readonly ShoesDbContext context;
        public BrandRepository(ShoesDbContext _context):base(_context) //Ya que cuando instancio el repo de Brands, tambien instancio el generico
        {
            context = _context;
        }

        public void Editar(Brand brand)
        {
            context.Brands.Update(brand);
        }
        public bool EstaRelacionado(Brand brand)
        {
            return context.Shoes.Any(b => b.BrandId == brand.BrandId);
        }
        public bool Existe(Brand? brand)
        {
            if (brand!.BrandId == 0)
            {
                return context.Brands.Any(b => b.BrandName == brand.BrandName);
            }
            return context.Brands.Any(b => b.BrandName == brand.BrandName && b.BrandId != brand.BrandId);
        }
        public int GetCantidad()
        {
            return context.Brands.Count();
        }
    }
}
