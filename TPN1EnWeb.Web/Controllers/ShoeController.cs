using AutoMapper;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Composition;
using System.Drawing.Drawing2D;
using System.Drawing;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList;
using X.PagedList.Extensions;
using Size = TPN1EnWeb.Entidades.Size;

namespace TPN1EnWeb.Web.Controllers
{
    public class ShoeController : Controller
    {
        private readonly IShoeService? _shoeService;
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;
        private readonly IGenreService _genreService;
        private readonly ISportService _sportService;
        private readonly ISizeService _sizeService;
        private readonly IMapper? _mapper;

        public ShoeController(IShoeService? shoeService, IMapper? mapper, IBrandService brandService, IColorService colorService, IGenreService genreService, ISportService sportService, ISizeService sizeService)
        {
            _shoeService = shoeService;
            _mapper = mapper;
            _brandService = brandService;
            _colorService = colorService;
            _genreService = genreService;
            _sportService = sportService;
            _sizeService = sizeService;
        }

        public IActionResult Index(int? page, string? searchTerm = null, int? FilterBrandId = null, int? FilterColorId=null, int? FilterGenreId=null,int? FilterSportId=null, int pageSize = 10, bool viewAll = false, string orderBY="Brand")
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm = searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            ViewBag.currentFilterBrandId = FilterBrandId;
            ViewBag.currentFilterColorId = FilterColorId;
            ViewBag.currentFilterGenreId = FilterGenreId;
            ViewBag.currentFilterSportId = FilterSportId;
            ViewBag.currentOrderBy = orderBY;
            IEnumerable<Shoe> shoes;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm) || FilterBrandId!=null || FilterColorId!=null || FilterGenreId!=null || FilterSportId!=null)
                {
                    if (searchTerm!=null && FilterBrandId==null && FilterColorId==null && FilterGenreId==null && FilterSportId==null)
                    {
                        FilterBrandId = null;
                        FilterColorId = null;
                        FilterGenreId = null;
                        FilterSportId = null;
                        ViewBag.currentFilterBrandId = FilterBrandId;
                        ViewBag.currentFilterColorId = FilterColorId;
                        ViewBag.currentFilterGenreId = FilterGenreId;
                        ViewBag.currentFilterSportId = FilterSportId;
                        //Hay que escribir las propiedades como estan en la entidad
                        shoes = _shoeService!.GetShoes(propertiesNames: "Sports,Brands,Color,Genres", orderBy: o => o.OrderBy(c => c.Price),
                                    filter: f => f.Brands!.BrandName.Contains(searchTerm!) || f.Color!.ColorName.Contains(searchTerm) || f.Sports!.SportName.Contains(searchTerm) ||
                                    f.Genres!.GenreName.Contains(searchTerm))!; 
                    }
                    else
                    {
                        searchTerm = string.Empty;
                        ViewBag.currentsearchTerm = searchTerm;
                        shoes = _shoeService!.GetShoes(propertiesNames: "Sports,Brands,Color,Genres", orderBy: o => o.OrderBy(order => order.Price),
                            filter: f => f.BrandId == FilterBrandId || f.ColourId == FilterColorId || f.GenreId == FilterGenreId || f.SportId == FilterSportId)!;
                    }

                }
                else
                {
                    shoes = _shoeService!.GetShoes(orderBy: o => o.OrderBy(b => b.Price), propertiesNames: "Sports,Brands,Color,Genres")!;
                }
            }
            else
            {
                searchTerm = string.Empty;
                FilterBrandId = null;
                FilterColorId = null;
                FilterGenreId = null;
                FilterSportId = null;
                ViewBag.currentsearchTerm = searchTerm;
                ViewBag.currentFilterBrandId = FilterBrandId;
                ViewBag.currentFilterColorId = FilterColorId;
                ViewBag.currentFilterGenreId = FilterGenreId;
                ViewBag.currentFilterSportId = FilterSportId;
                shoes = _shoeService!.GetShoes(orderBy: o => o.OrderBy(b => b.Price), propertiesNames: "Sports,Brands,Color,Genres")!;
            }

            var listavm = _mapper?.Map<IEnumerable<ShoeListVM>>(shoes).ToList();
            if (orderBY == "Brand")
            {
                listavm = listavm!.OrderBy(o => o.brand).ToList();
            }
            if (orderBY=="Colour")
            {
                listavm = listavm!.OrderBy(o => o.color).ToList();
            }
            if (orderBY == "Genre")
            {
                listavm = listavm!.OrderBy(o => o.genre).ToList();
            }
            if (orderBY == "Sport")
            {
                listavm = listavm!.OrderBy(o => o.sport).ToList();
            }
            var shoeFilterVM=new ShoeFilterVm()
            {
                Shoes = listavm!.ToPagedList(pagenumber, pageSize),
                Brands = _brandService.GetBrands(orderBy: o => o.OrderBy(order => order.BrandName))!.Select(s => new SelectListItem { Text = s.BrandName, Value = s.BrandId.ToString() }).ToList(),
                Genres = _genreService.GetGenres(orderBy: o => o.OrderBy(order => order.GenreName))!.Select(s => new SelectListItem { Text = s.GenreName, Value = s.GenreId.ToString() }).ToList(),
                Colors = _colorService.GetColours(orderBy: o => o.OrderBy(order => order.ColorName))!.Select(s => new SelectListItem { Text = s.ColorName, Value = s.ColourId.ToString() }).ToList(),
                Sports = _sportService.GetSports(orderBy: o => o.OrderBy(order => order.SportName))!.Select(s => new SelectListItem { Text = s.SportName, Value = s.SportId.ToString() }).ToList(),

            };
            return View(shoeFilterVM);
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
                    Shoe? shoe = _shoeService?.GetShoe(s => s.ShoeId == id.Value);
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
                var brand = _brandService.GetBrand(filter: filter => filter.BrandId == shoe.BrandId);
                var sport = _sportService.GetSport(filter: filter => filter.SportId == shoe.SportId);
                var genre = _genreService.GetGenre(filter: filter => filter.GenreId == shoe.GenreId);
                var color = _colorService.GetColour(filter: filter => filter.ColourId == shoe.ColourId);
                shoe.Color = color;
                shoe.Brands = brand;
                shoe.Genres = genre;
                shoe.Sports = sport;
                if (_shoeService!.Existe(shoe))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    RercargarCombos(shoeEditVM);
                    return View(shoeEditVM);
                }
                _shoeService.Guardar(shoe);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
                //if (shoe.ShoeId != 0)
                //{
                //    return RedirectToAction("Index");
                //}
                //else
                //{
                //    return RedirectToAction("AddSize", shoe);
                //}
            }
            catch (Exception)
            {
                RercargarCombos(shoeEditVM);
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(shoeEditVM);
            }
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Shoe? shoe = _shoeService?.GetShoe(filter: g => g.ShoeId == id);
            if (shoe == null)
            {
                return NotFound();
            }
            List<Size> listaDeSizes = new List<Size>();
            listaDeSizes= _shoeService!.GetSizesPorShoes(shoe.ShoeId)!;
            foreach (var item in listaDeSizes)
            {
                if (_shoeService?.ExisteRelacion(shoe, item) ?? true)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Related Record..."
                    });
                }
            }
          
            _shoeService.Borrar(shoe);
            return Json(new { success = true, message = "Record deleted successfully" });

        }


        public IActionResult AddSize(int? page, Shoe shoe)
        {
            int pagenumber = page ?? 1;
            int pagesize = 7;
            var lista = _sizeService?.GetSizes();
            PagedList<Size> sizes=new PagedList<Size>(lista!, pagenumber, pagesize);
            ShoeSizeListVM shoeSizeEditVM = new ShoeSizeListVM();
            shoeSizeEditVM.shoe = shoe;
            shoeSizeEditVM.sizes = sizes;
            return View(shoeSizeEditVM);
        }

        public IActionResult AddStock(ShoeSizeListVM? shoe, int id)
        {
            var size=_sizeService.GetSize(filter:filter=>filter.SizeId == id);
            if (size == null)
            {
                return NotFound();
            }
            var sizes=new List<Size>();
            sizes.Add(size);
            
            return View();
        }
    }
}
