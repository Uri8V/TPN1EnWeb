using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class OrderHeadersRepository : GenericRepository<OrderHeader>, IOrderHeadersRepository
    {
        private readonly ShoesDbContext _db;

        public OrderHeadersRepository(ShoesDbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(OrderHeader orderHeader)
        {

            _db.OrderHeaders.Update(orderHeader);

        }
    }
}
