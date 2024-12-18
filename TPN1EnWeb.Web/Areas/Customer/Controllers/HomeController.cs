using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using TPN1EnWeb.Web.Models;
using X.PagedList.Extensions;

namespace TPN1EnWeb.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IShoeService? _shoeService;
        private readonly IMapper? _mapper;
        private readonly IShoeSizeService? _shoeSizeService;
        private readonly ISizeService? _sizeService;
        private readonly IShoppingCartsService? _shoppingCartService;

        public HomeController(IShoeService? shoeService, IMapper? mapper, IShoeSizeService? shoeSizeService, ISizeService? sizeService, IShoppingCartsService? shoppingCartService)
        {
            _shoeService = shoeService;
            _mapper = mapper;
            _shoeSizeService = shoeSizeService;
            _sizeService = sizeService;
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 12;
            IEnumerable<Shoe> listaShoe = _shoeService!.GetShoes(orderBy: o => o.OrderBy(p => p.Price), propertiesNames: "Sports,Brands,Color,Genres")!;
            var listaShoePaginada = _mapper!.Map<IEnumerable<ShoeListCustomerAreaVM>>(listaShoe);
            ViewBag.currentPage=pageNumber;
            return View(listaShoePaginada.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Details(int id, int Page, string? returnurl=null)
        {
            List<ShoeSizeDetailsCustomerAreaVM> list = new List<ShoeSizeDetailsCustomerAreaVM>();
            var shoe = _shoeService!.GetShoe(s => s.ShoeId == id, propertiesNames: "Sports,Brands,Color,Genres");
            var listaSizes = _shoeService.GetSizesPorShoes(shoe!.ShoeId);
            for (int i = 0; i < listaSizes!.Count; i++)
            {
                ShoeSizeDetailsCustomerAreaVM shoeSizes = new ShoeSizeDetailsCustomerAreaVM();
                shoeSizes.Shoe = _mapper!.Map<ShoeListCustomerAreaVM>(shoe);
                shoeSizes.ShoeId = shoe.ShoeId;
                shoeSizes.Size = listaSizes[i];
                shoeSizes.SizeId = listaSizes[i].SizeId;    
                shoeSizes.AvailableStock = _shoeSizeService!.GetShoeSize(filter:s=>s.SizeId==listaSizes[i].SizeId && s.ShoeId==shoe.ShoeId)!.QuantityInStock;
                shoeSizes.Page = Page;
                list.Add(shoeSizes);
            }
            ViewBag.ReturnUrl= returnurl;
            return View(list);
        }
        [Authorize(Roles = "Customer")]
        public IActionResult AgregarAlCarrito(int id, int sizeid, int Page)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userid = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var shoe = _shoeService!.GetShoe(filter: s => s.ShoeId == id, propertiesNames:"Sports,Brands,Color,Genres");
            var size = _sizeService!.GetSize(filter: s => s.SizeId == sizeid);
            if (shoe is null)
            {
                ModelState.AddModelError(string.Empty, "Shoe doesn´t found");
            }
            if (shoe is null)
            {
                ModelState.AddModelError(string.Empty, "Size doesn´t found");
            }
            ShoeSizeDetailsCustomerAreaVM shoeSizes = new ShoeSizeDetailsCustomerAreaVM();
            shoeSizes.Shoe = _mapper!.Map<ShoeListCustomerAreaVM>(shoe);
            shoeSizes.ShoeId = shoe!.ShoeId;
            shoeSizes.Size = size!;
            shoeSizes.SizeId = size!.SizeId;
            shoeSizes.AvailableStock = _shoeSizeService!.GetIdShoeSize(size!.SizeId, shoe.ShoeId).QuantityInStock;
            ShoppingCartDetailVm sh = new ShoppingCartDetailVm()
            {
                ShoeSizeId=_shoeSizeService.GetShoeSize(filter:s=>s.SizeId==size.SizeId && s.ShoeId==shoe.ShoeId)!.ShoeSizeId,
                Quantity = 1,
                ShoeSizeDetails = shoeSizes,
                ApplicationUserId = userid,
            };
            ShoppingCart shoppingCart= _mapper.Map<ShoppingCart>(sh);
            var cartInDb = _shoppingCartService!.Get(filter: s => s.ShoeSizeId == shoppingCart.ShoeSizeId &&
                    s.ApplicationUserId == shoppingCart.ApplicationUserId);
            var RelacionShoeSize = _shoeSizeService.GetShoeSize(filter: s=>s.ShoeId==shoe.ShoeId && s.SizeId==size.SizeId, propertiesNames:"Shoe,Size");
            if (RelacionShoeSize!.AvailableStock>=shoppingCart.Quantity)
            {
                if (cartInDb == null)
                {
                    RelacionShoeSize.StockInCarts += shoppingCart.Quantity;
                    _shoppingCartService!.Save(shoppingCart);
                }
                else
                {
                    RelacionShoeSize.StockInCarts += shoppingCart.Quantity;
                    cartInDb.Quantity += shoppingCart.Quantity;
                    _shoppingCartService.Save(cartInDb);
                }
                _shoeSizeService.Save(RelacionShoeSize);
                TempData["success"] = "Shoe added successfully to shopping cart";

            }
            else
            {
                TempData["error"] = "Not enuogh available";
            }

            return RedirectToAction("Index", new {page=Page});
        }
        public IActionResult Hero()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
