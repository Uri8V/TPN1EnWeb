  using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Mono.TextTemplating;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList;
using X.PagedList.Extensions;

namespace TPN1EnWeb.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService? _brandService; //Le pido los datos al Servicio
        private readonly IMapper? mapper;
        private readonly IShoeService? _shoeService;

        public BrandController(IBrandService? brandService, IMapper? _mapper, IShoeService? shoeService)
        {
            _brandService = brandService;
            mapper = _mapper;
            _shoeService = shoeService;
        }

        public IActionResult Index(int? page, string? searchTerm=null, int pageSize = 10, bool viewAll = false) //Muestro todas las Brand
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm=searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            IEnumerable<Brand> brands;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    brands = _brandService!.GetBrands(orderBy: o => o.OrderBy(b => b.BrandName), filter: f => f.BrandName.Contains(searchTerm!))!;
                }
                else
                {
                    brands = _brandService!.GetBrands(orderBy: o => o.OrderBy(b => b.BrandName))!;
                } 
            }
            else
            {
                searchTerm = string.Empty;
                ViewBag.currentsearchTerm = searchTerm; 
                brands = _brandService!.GetBrands(orderBy: o => o.OrderBy(b => b.BrandName))!;

            }
            var listabrandvm = new List<BrandListVM>();
            foreach (var brand in brands)
            {
                var brandVm = new BrandListVM() {
                    BrandId = brand.BrandId,
                    BrandName = brand.BrandName,
                    ShoeCount = _shoeService!.GetCantidad(s=>s.BrandId==brand.BrandId)
                };
                listabrandvm.Add(brandVm);
            }
            PagedList<BrandListVM> listaVm = new PagedList<BrandListVM>(listabrandvm, pagenumber, pageSize);
            listaVm.ToPagedList(pagenumber, pageSize);
            return View(listaVm);
        }
        public IActionResult UpSert(int? id) 
        {
            BrandEditVM? brandEditVM;
            if (id is null || id.Value == 0)
            {
                brandEditVM = new BrandEditVM();
            }
            else
            {
                Brand? brand = _brandService?.GetBrand(filter:brand=>brand.BrandId==id);
                if (brand == null)
                {
                    return NotFound();
                }
                brandEditVM = mapper?.Map<BrandEditVM>(brand);
            }
            return View(brandEditVM);
        }
        [HttpPost]
        public IActionResult UpSert(BrandEditVM? brandEditVM)
        {
            if (!ModelState.IsValid)
            {
                return View(brandEditVM);
            }
            Brand? brand = mapper?.Map<Brand>(brandEditVM);
            if (brand == null)
            {
                ModelState.AddModelError(string.Empty, "No Brand has been supplied");
                return View(brandEditVM);
            }
            if (_brandService?.Existe(brand)??true)
            {
                ModelState.AddModelError(string.Empty, "Registro Duplicado¡¡¡¡");
                return View(brandEditVM);
            }
            _brandService.Guardar(brand);
            TempData["success"] = "Record added/edited successfully";
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Brand? brand = _brandService?.GetBrand(filter:brand => brand.BrandId == id);
            if (brand is null)
            {
                return NotFound();
            }
            if (_brandService?.EstaRelacionado(brand)??true)
            {
                return Json(new
                {
                    success = false,
                    message = "Related Record..."
                });
            }
            _brandService?.Borrar(brand);
            return Json(new { success = true, message = "Record deleted successfully" });
        }
    }
}
