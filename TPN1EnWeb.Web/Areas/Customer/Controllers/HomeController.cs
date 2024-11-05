using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public HomeController(IShoeService? shoeService, IMapper? mapper, IShoeSizeService? shoeSizeService)
        {
            _shoeService = shoeService;
            _mapper = mapper;
            _shoeSizeService = shoeSizeService;
        }

        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 12;
            IEnumerable<Shoe> listaShoe = _shoeService!.GetShoes(orderBy: o => o.OrderBy(p => p.Price), propertiesNames: "Sports,Brands,Color,Genres")!;
            var listaShoePaginada = _mapper!.Map<IEnumerable<ShoeListCustomerAreaVM>>(listaShoe);
            return View(listaShoePaginada.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Details(int id)
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
                shoeSizes.QuantityInStock = _shoeSizeService!.GetIdShoeSize(listaSizes[i].SizeId, shoe.ShoeId).QuantityInStock;
                list.Add(shoeSizes);
            }
            return View(list);
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
