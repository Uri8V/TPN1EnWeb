using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList.Extensions;
using Size = TPN1EnWeb.Entidades.Size;

namespace TPN1EnWeb.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            var lista = _sizeService?.GetSizes().ToPagedList(pagenumber, pagesize);
            return View(lista);
        }
        public IActionResult Details(int id)
        {
            var size = _sizeService?.GetSize(filter: s => s.SizeId == id);
            var listashoe = _sizeService?.GetShoePorSize(size!);
            var listasizevm = new List<SizeShoeListVM>();
            foreach (var shoeVM in listashoe!)
            {
                var SIZEFORVIEW = new SizeShoeListVM()
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
        public IActionResult UpdateStock(int SizeId, int id)
        {
            var Size = _sizeService!.GetSizePorId(SizeId);
            var Shoe = _shoeService!.GetShoe(filter: shoe => shoe.ShoeId == id, propertiesNames: "Sports,Brands,Color,Genres");
            var shoeSize = _shoeSizeService!.GetIdShoeSize(SizeId, id);
            return View(shoeSize);
        }
        [HttpPost]
        public IActionResult UpdateStock(ShoeSizes shoeSize)
        {
            if (shoeSize.QuantityInStock == 0)
            {
                ModelState.AddModelError(string.Empty, "Record need to be positive");
                return View(shoeSize);
            }
            Size size = _sizeService!.GetSize(filter: filter => filter.SizeId == shoeSize.SizeId)!;
            var shoe = _shoeService?.GetShoe(s => s.ShoeId == shoeSize.ShoeId, propertiesNames: "Sports,Brands,Color,Genres");
            ShoeSizes shoeSizes = _shoeSizeService!.GetIdShoeSize(size.SizeId, shoe!.ShoeId);
            shoeSizes.QuantityInStock += shoeSize.QuantityInStock;
            if (shoeSizes.QuantityInStock < 0)
            {
                ModelState.AddModelError(string.Empty, "The record must to be Positive or equal to 0");
                return View(shoeSizes);
            }
            try
            {
                if (_shoeService!.ExisteRelacion(shoe, size))
                {
                    _shoeSizeService.Save(shoeSizes);
                    TempData["success"] = "Record successfully edited";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(shoeSize);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(shoeSize);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? shoeId,int? sizeId)
        {
            if (sizeId == null || sizeId == 0 || shoeId == null || shoeId == 0)
            {
                return NotFound();
            }
            Shoe? shoe = _shoeService?.GetShoe(filter: g => g.ShoeId == shoeId);
            Size? size = _sizeService?.GetSize(filter: s => s.SizeId == sizeId);

            ShoeSizes shoeSizes = _shoeSizeService?.GetShoeSize(filter:sh=>sh.ShoeId==shoe!.ShoeId && sh.SizeId==size!.SizeId)!;

            if (shoeSizes==null)
            {
                return NotFound();
            }
            if (_shoeSizeService!.ItsRelated(shoeSizes))
            {
                return Json(new
                {
                    success = false,
                    message = "Related Record..."
                });
            }
            _shoeSizeService!.Delete(shoeSizes);
            return Json(new { success = true, message = "Record deleted successfully" });

        }

    }
}
