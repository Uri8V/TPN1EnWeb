using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IApplicationUsersService
    {
        IEnumerable<ApplicationUser>? GetAll(Expression<Func<ApplicationUser, bool>>? filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orderBy = null,
            string? propertiesNames = null);
        void Save(ApplicationUser applicationUser);
        void Delete(ApplicationUser applicationUser);
        ApplicationUser? Get(Expression<Func<ApplicationUser, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

    }
}
