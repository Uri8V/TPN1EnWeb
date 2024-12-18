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
    public class OrderHeadersService : IOrderHeadersService
    {
        private readonly IOrderHeadersRepository? _repository;
        private readonly IShoeSizeRepository? _shoeSizeRepository;
        private readonly IShoppingCartsRepository? _shoppingCartsRepository;
        private readonly IUnitOfWork? _unitOfWork;

        public OrderHeadersService(IOrderHeadersRepository? repository,
            IShoeSizeRepository shoeSizesRepository,
            IShoppingCartsRepository shoppingCartsRepository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _shoeSizeRepository = shoeSizesRepository ?? throw new ArgumentException("Dependencies not set");
            _shoppingCartsRepository = shoppingCartsRepository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(orderHeader);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }
        public OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<OrderHeader> GetAll(Expression<Func<OrderHeader, bool>>? filter = null,
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames)!;
        }
        public void Save(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (orderHeader.OrderHeaderId == 0)
                {
                    _repository?.Add(orderHeader);
                    //_unitOfWork.SaveChanges();
                    foreach (var item in orderHeader.OrderDetail)
                    {
                        item.ShoeSizes = _shoeSizeRepository!.Get(
                            filter: p => p.ShoeSizeId==item.ShoeSizeId);
                        item.ShoeSizes!.QuantityInStock -= item.Quantity;
                        item.ShoeSizes.StockInCarts -= item.Quantity;
                        _shoeSizeRepository.Editar(item.ShoeSizes);

                        var shoppingCart = _shoppingCartsRepository!.Get(
                                filter: sc => sc.ShoeSizeId == item.ShoeSizeId
                                && sc.ApplicationUserId == orderHeader.ApplicationUserId);
                        _shoppingCartsRepository.Delete(shoppingCart);
                    }
                }
                else
                {
                    _repository?.Update(orderHeader);
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
