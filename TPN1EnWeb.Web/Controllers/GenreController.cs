using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList;
using X.PagedList.Extensions;

namespace TPN1EnWeb.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService? _genreService;
        private readonly IMapper? _mapper;
        private readonly IShoeService? _shoeService;

        public GenreController(IGenreService? genreService, IMapper? mapper, IShoeService? shoeService)
        {
            _genreService = genreService;
            _mapper = mapper;
            _shoeService = shoeService;
        }

        public IActionResult Index(int? page, string? searchTerm = null, int pageSize = 10, bool viewAll = false)
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm = searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            IEnumerable<Genre> genres;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    genres = _genreService!.GetGenres(orderBy: o => o.OrderBy(c => c.GenreName), filter: f => f.GenreName.Contains(searchTerm!))!;
                }
                else
                {
                    genres = _genreService!.GetGenres(orderBy: o => o.OrderBy(b => b.GenreName))!;
                }
            }
            else
            {
                searchTerm = string.Empty;
                ViewBag.currentsearchTerm = searchTerm;
                genres = _genreService!.GetGenres(orderBy: o => o.OrderBy(b => b.GenreName))!;

            }
            var listagenrevm = new List<GenreListVM>();
            foreach (var genre in genres!)
            {
                var genreVm = new GenreListVM()
                {
                    GenreId = genre.GenreId,
                    GenreName = genre.GenreName,
                    ShoeCount = _shoeService!.GetCantidad(s => s.GenreId==genre.GenreId)
                };
                listagenrevm.Add(genreVm);
            }
            PagedList<GenreListVM> listaVm = new PagedList<GenreListVM>(listagenrevm, pagenumber, pageSize);
            listaVm.ToPagedList(pagenumber, pageSize);
            return View(listaVm);
        }
        public IActionResult UpSert(int? id)  
        {
            GenreEditVM? genreEditVM;
            if (id is null || id.Value==0)
            {
                genreEditVM = new GenreEditVM();
            }
            else
            {
                Genre? genre = _genreService?.GetGenre(filter:g=>g.GenreId==id);
                if (genre==null)
                {
                    return NotFound();
                }
                genreEditVM = _mapper?.Map<GenreEditVM>(genre);
            }
            return View(genreEditVM); 
        }
        [HttpPost]
        public IActionResult UpSert(GenreEditVM genreEditVM) 
        {
            if (!ModelState.IsValid)
            {
                return View(genreEditVM);
            }
            Genre? genre = _mapper?.Map<Genre>(genreEditVM);
            if (genre == null)
            {
                ModelState.AddModelError(string.Empty, "No Genre has been supplied");
                return View(genreEditVM);
            }
            if (_genreService?.Existe(genre) ?? true)
            {
                ModelState.AddModelError(string.Empty, "Registro Duplicado¡¡¡¡");
                return View(genreEditVM);
            }
            _genreService.Guardar(genre);
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
            Genre? genre = _genreService?.GetGenre(filter: g => g.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }
            if (_genreService?.EstaRelacionado(genre) ?? true)
            {
                return Json(new
                {
                    success = false,
                    message = "Related Record..."
                });
            }
            _genreService?.Borrar(genre);
            return Json(new { success = true, message = "Record deleted successfully" });

        }
        public IActionResult Details(int? id)
        {
            Genre genre = _genreService?.GetGenre(filter: filter => filter.GenreId == id)!;
            if (genre is null)
            {
                return NotFound();
            }
            IEnumerable<Shoe> listaShoeFiltradaPorBrand;
            listaShoeFiltradaPorBrand = _shoeService?.GetShoes(filter: filter => filter.GenreId == id, propertiesNames: "Genres,Color,Sports,Brands")!;
            var listavm = _mapper?.Map<IEnumerable<ShoeListVM>>(listaShoeFiltradaPorBrand).ToList();
            return View(listavm);
        }
    }
}
