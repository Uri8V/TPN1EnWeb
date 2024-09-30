using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using X.PagedList.Extensions;

namespace TPN1EnWeb.Web.Controllers
{
    public class SizeController : Controller
    {
        private readonly ISizeService? _sizeService;
        private readonly IMapper? _mapper;
        private readonly IShoeService? _shoeService;
        private readonly IShoeSizeService? _shoeSizeService;

        public SizeController(ISizeService? sizeService, IMapper? mapper, IShoeService? shoeService, IShoeSizeService? shoeSizeService)
        {
            _sizeService = sizeService;
            _mapper = mapper;
            _shoeService = shoeService;
            _shoeSizeService = shoeSizeService;
        }

        public IActionResult Index(int? page)
        {
            int pagenumber = page ?? 1;
            int pagesize = 7;
            var lista=_sizeService?.GetSizes().ToPagedList(pagenumber, pagesize);
            return View(lista);
        }
        public IActionResult Details(int id)
        {
            var size = _sizeService?.GetSize(filter:s=>s.SizeId==id);
            var listashoe = _sizeService?.GetShoePorSize(size!);
            var listasizevm= new List<SizeListVM>();
            foreach (var shoeVM in listashoe!)
            {
                var SIZEFORVIEW = new SizeListVM()
                {
                    SizeId = size!.SizeId,
                    BrandName = shoeVM.brand!,
                    ColorName = shoeVM.color!,
                    GenreName = shoeVM.genre!,
                    SportName = shoeVM.sport!,
                    Model = shoeVM.model,
                    Description = shoeVM.descripcion,
                    Price = shoeVM.price.ToString(),
                    ShoeId = shoeVM.shoeId,
                    SizeNumber = size.SizeNumber,
                    Stock = _shoeSizeService!.GetIdShoeSize(size.SizeId, shoeVM.shoeId).QuantityInStock
                };
                listasizevm.Add(SIZEFORVIEW);
            }
            return View(listasizevm);
        } 
    }
}
