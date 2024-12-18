using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TPN1EnWeb.Servicios.Interfaces;

namespace TPN1EnWeb.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer, Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderHeadersService? _headersService;
        private readonly IShoeSizeService _shoeSizeService;
        private readonly IMapper? _mapper;

        public OrdersController(IOrderHeadersService? headersService,
            IShoeSizeService shoeSizeService,
            IMapper? mapper)
        {
            _headersService = headersService;
            _shoeSizeService = shoeSizeService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var orderHeader = _headersService!.Get(filter: o => o.OrderHeaderId == id,
                propertiesNames: "OrderDetail");
            foreach (var detail in orderHeader!.OrderDetail)
            {
                var shoesizeInDetail = _shoeSizeService.GetShoeSize(filter: p => p.ShoeSizeId == detail.ShoeSizeId, propertiesNames:"Shoe,Size");
                detail.ShoeSizes = shoesizeInDetail;
            }
            return View(orderHeader);
        }
        #region API CALLS
        [HttpGet]
        public JsonResult GetAll() 
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var orderList = _headersService!.GetAll(filter:
                o => o.ApplicationUserId == claims!.Value);
            return Json(new { data = orderList });
        }
        #endregion
    }
}
