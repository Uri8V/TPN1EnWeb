using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class ApplicationUsersRepository : GenericRepository<ApplicationUser>, IApplicationUsersRepository
    {
        private readonly ShoesDbContext _db;

        public ApplicationUsersRepository(ShoesDbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Update(ApplicationUser applicationUser)
        {

            _db.ApplicationUsers.Update(applicationUser);

        }
    }
}
