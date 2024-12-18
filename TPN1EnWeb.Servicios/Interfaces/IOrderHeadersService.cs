using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IOrderHeadersService
    {
        IEnumerable<OrderHeader>? GetAll(Expression<Func<OrderHeader, bool>>? filter = null,
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
            string? propertiesNames = null);
        void Save(OrderHeader OrderHeader);
        void Delete(OrderHeader OrderHeader);
        OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
    }
}
