using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList;
using X.PagedList.Extensions;

namespace TPN1EnWeb.Web.Controllers
{
    public class ColourController : Controller
    {
        private readonly IColorService? _colorService;
        private readonly IMapper? mapper;
        private readonly IShoeService? _shoeService;
        public ColourController(IColorService? colorService, IMapper? _mapper, IShoeService? shoeService)
        {
            _colorService = colorService;
            mapper = _mapper;
            _shoeService = shoeService;
        }
        public IActionResult Index(int? page, string? searchTerm= null, int pageSize = 10, bool viewAll = false)
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm = searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            IEnumerable<Colour> colours;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    colours = _colorService!.GetColours(orderBy: o => o.OrderBy(c=>c.ColorName), filter: f => f.ColorName.Contains(searchTerm!))!;
                }
                else
                {
                    colours = _colorService!.GetColours(orderBy: o => o.OrderBy(b => b.ColorName))!;
                }
            }
            else
            {
                searchTerm = string.Empty;
                ViewBag.currentsearchTerm = searchTerm;
                colours = _colorService!.GetColours(orderBy: o => o.OrderBy(b => b.ColorName))!;

            }
            var listacolourdvm = new List<ColourListVM>();
            foreach (var colour in colours!)
            {
                var colourVm = new ColourListVM()
                {
                    ColourId = colour.ColourId,
                    ColorName = colour.ColorName,
                    ShoeCount = _shoeService!.GetCantidad(s => s.ColourId == colour.ColourId)
                };
                listacolourdvm.Add(colourVm);
            }
            PagedList<ColourListVM> listaVm = new PagedList<ColourListVM>(listacolourdvm, pagenumber, pageSize);
            listaVm.ToPagedList(pagenumber, pageSize);
            return View(listaVm);
        }
        public IActionResult UpSert(int? id)
        {
            ColourEditVM? colourEditVM;
            if (id is null || id.Value == 0)
            {
                colourEditVM = new ColourEditVM();
            }
            else
            {
                Colour? colour = _colorService?.GetColour(filter: c => c.ColourId == id);
                if (colour == null)
                {
                    return NotFound();
                }
                colourEditVM = mapper?.Map<ColourEditVM>(colour);
            }
            return View(colourEditVM);
        }
        [HttpPost]
        public IActionResult UpSert(ColourEditVM? colourEditVM)
        {
            if (!ModelState.IsValid)
            {
                return View(colourEditVM);
            }
            Colour? colour = mapper?.Map<Colour>(colourEditVM);
            if (colour == null)
            {
                ModelState.AddModelError(string.Empty, "No Colour has been supplied");
                return View(colourEditVM);
            }
            if (_colorService?.Existe(colour)??true)
            {
                ModelState.AddModelError(string.Empty, "Registro Duplicado¡¡¡¡");
                return View(colourEditVM);
            }
            _colorService.Guardar(colour);
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
            Colour? colour = _colorService?.GetColour(filter: c=>c.ColourId==id);
            if (colour == null)
            {
                return NotFound();
            }
            if (_colorService?.EstaRelacionado(colour) ?? true)
            {
                return Json(new
                {
                    success = false,
                    message = "Related Record..."
                });
            }
            _colorService?.Borrar(colour);
            return Json(new { success = true, message = "Record deleted successfully" });

        }
    }
}
