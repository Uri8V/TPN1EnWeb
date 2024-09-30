  using AutoMapper;
using Microsoft.AspNetCore;
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
        private readonly IWebHostEnvironment? _webHostEnvironment;

        public BrandController(IBrandService? brandService, IMapper? _mapper, IShoeService? shoeService, IWebHostEnvironment? webHostEnvironment)
        {
            _brandService = brandService;
            mapper = _mapper;
            _shoeService = shoeService;
            _webHostEnvironment = webHostEnvironment;//TRAE IFORMACIÓN DE MIS CARPETAS EN MI SERVIDOR, en este caso la vamos usar para la carpeta images en wwwroot
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
                //string? wwwWebRoot = _webHostEnvironment!.WebRootPath;
                Brand? brand = _brandService?.GetBrand(filter: brand => brand.BrandId == id);
                if (brand == null)
                {
                    return NotFound();
                }
                //if (brand.BrandId!=0 && brand.imageURL==null)
                //{
                //    //string filePath = Path.Combine(wwwWebRoot, brand.imageURL.TrimStart('/')); ESTO DA ERRROR NULL CON LOS NUEVOS OBJETOS SI NO SELECCIONAS UN ARCHIVO
                //                                                                              //POR LO QUE ESTE PEDAZO DE CÓDIGO SOLO FUNCIONARÍA CON LOS REGISTROS YA AGREGADOS QUE TENGAN IMAGEN
                //    //ViewData["filExist"] = System.IO.File.Exists(filePath);
                //}
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
            if (brandEditVM!.ImageFile != null)
            {
                var permittedExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" }; //Estas van a ser las extesiones que voy a permitir subir, si no hay alguna que yo desee, la agrego
                string fileExtension = Path.GetExtension(brandEditVM.ImageFile.FileName); //Obtenemos la extension del nombre de nuestro archivo
                if (!permittedExtensions.Contains(fileExtension)) // si la extension que obtuvimos de nuestro archivo no esta en los permitidos, va a tirar error y volver a la vista
                {
                    ModelState.AddModelError(string.Empty, "File not allowed");
                    return View(brandEditVM);
                }

                string? webroot = _webHostEnvironment!.WebRootPath;
                if (brandEditVM.imageURL!=null)
                {
                    string? oldPath = Path.Combine(webroot, brand.imageURL!.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) //Me fijo si existe esta ruta
                    {
                        System.IO.File.Delete(oldPath);//La doy de baja porque voy a ingresar una nueva en el objeto
                    }
                }
                string fileName= $"{Guid.NewGuid()}{Path.GetExtension(brandEditVM.ImageFile.FileName)}";
                string pathName=Path.Combine(webroot, "images", fileName); //Nombre de la ruta combinando el webroot que es la ruta web,el nombre del archivo y el string "images"
                using (var filestream= new FileStream(pathName,FileMode.Create))
                {
                    brandEditVM.ImageFile.CopyTo(filestream); // Con esto subo mi imagen a la carpeta images en wwroot
                }
                brand.imageURL = $"/images/{fileName}";
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
            //PREGUNTAR, POR QUÉ ES NECESARIO BORRAR LA RUTA SI YA LA DEBERÍA BORRAR CON EL OBJETO?
            if (brand.imageURL!=null)
            {
                string? webroot = _webHostEnvironment!.WebRootPath;
                string? oldPath = Path.Combine(webroot, brand.imageURL!.TrimStart('/'));
                if (System.IO.File.Exists(oldPath)) //Me fijo si existe esta ruta
                {
                    System.IO.File.Delete(oldPath);//La doy de baja porque voy a ingresar una nueva en el objeto
                } 
            }
            _brandService?.Borrar(brand);
            return Json(new { success = true, message = "Record deleted successfully" });
        }

        public IActionResult Details(int? id)
        {
            Brand brand= _brandService?.GetBrand(filter:filter => filter.BrandId == id)!;
            if (brand is null)
            {
                return NotFound();
            }
            IEnumerable<Shoe> listaShoeFiltradaPorBrand;
            listaShoeFiltradaPorBrand = _shoeService?.GetShoes(filter: filter => filter.BrandId == id, propertiesNames:"Genres,Color,Sports,Brands")!;
            var listavm = mapper?.Map<IEnumerable<ShoeListVM>>(listaShoeFiltradaPorBrand).ToList();
            return View(listavm);
        }
    }
}
