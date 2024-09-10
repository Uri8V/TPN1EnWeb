using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList.Extensions;

namespace TPN1EnWeb.Web.Controllers
{
    public class ShoeController : Controller
    {
        private readonly IShoeService? _shoeService;
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;
        private readonly IGenreService _genreService;
        private readonly ISportService _sportService;
        private readonly IMapper? _mapper;

        public ShoeController(IShoeService? shoeService, IMapper? mapper, IBrandService brandService, IColorService colorService, IGenreService genreService, ISportService sportService)
        {
            _shoeService = shoeService;
            _mapper = mapper;
            _brandService = brandService;
            _colorService = colorService;
            _genreService = genreService;
            _sportService = sportService;
        }

        public IActionResult Index(int? page, string? searchTerm = null, int pageSize = 10, bool viewAll = false)
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm = searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            IEnumerable<Shoe> shoes;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                { //Hay que escribir las propiedades como estan en la entidad
                    shoes = _shoeService!.GetShoes(propertiesNames: "Sports,Brands,Color,Genres",orderBy: o => o.OrderBy(c => c.Price), filter: f => f.Brands!.BrandName.Contains(searchTerm!) || f.Color!.ColorName.Contains(searchTerm) || f.Sports!.SportName.Contains(searchTerm) || f.Genres!.GenreName.Contains(searchTerm))!;
                }
                else
                {
                    shoes = _shoeService!.GetShoes(orderBy: o => o.OrderBy(b => b.Price), propertiesNames: "Sports,Brands,Color,Genres")!;
                }
            }
            else
            {
                searchTerm = string.Empty;
                ViewBag.currentsearchTerm = searchTerm;
                shoes = _shoeService!.GetShoes(orderBy: o => o.OrderBy(b => b.Price), propertiesNames: "Sports,Brands,Color,Genres")!;
            }

            var listavm = _mapper?.Map<IEnumerable<ShoeListVM>>(shoes).ToList();
            return View(listavm!.ToPagedList(pagenumber, pageSize));
        }

        public IActionResult UpSert(int? id)
        {
            ShoeEditVM? shoeEditVM;
            if (id is null || id.Value == 0)
            {
                shoeEditVM = new ShoeEditVM();
                RercargarCombos(shoeEditVM);
            }
            else
            {
                try
                {
                    Shoe? shoe = _shoeService?.GetShoe(s=>s.ShoeId==id.Value);
                    if (shoe == null)
                    {
                        return NotFound();
                    }
                    shoeEditVM = _mapper?.Map<ShoeEditVM>(shoe);
                    RercargarCombos(shoeEditVM);
                    return View(shoeEditVM);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(shoeEditVM);
        }
        private void RercargarCombos(ShoeEditVM? shoeEditVM)
        {
            shoeEditVM!.Brands = _brandService!.GetBrands()!
                .Select(b => new SelectListItem
                {
                    Text = b.BrandName,
                    Value = b.BrandId.ToString()
                }
                ).ToList();
            shoeEditVM.Colours = _colorService!.GetColours()!
              .Select(c => new SelectListItem
              {
                  Text = c.ColorName,
                  Value = c.ColourId.ToString()
              }
              ).ToList();
            shoeEditVM.Genres = _genreService!.GetGenres()!
              .Select(g => new SelectListItem
              {
                  Text = g.GenreName,
                  Value = g.GenreId.ToString()
              }
              ).ToList();
            shoeEditVM.Sports = _sportService!.GetSports()!
              .Select(s => new SelectListItem
              {
                  Text = s.SportName,
                  Value = s.SportId.ToString()
              }
              ).ToList();
        }
        [HttpPost]
        public IActionResult UpSert(ShoeEditVM shoeEditVM)
        {
            if (!ModelState.IsValid)
            {
                RercargarCombos(shoeEditVM);
                return View(shoeEditVM);
            }
            try
            {
                Shoe shoe = _mapper!.Map<Shoe>(shoeEditVM);
                if (_shoeService!.Existe(shoe))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    RercargarCombos(shoeEditVM);
                    return View(shoeEditVM);
                }

               // _shoeService.Guardar(shoe);
                //TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                RercargarCombos(shoeEditVM);
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(shoeEditVM);
            }
        }
    }
}
