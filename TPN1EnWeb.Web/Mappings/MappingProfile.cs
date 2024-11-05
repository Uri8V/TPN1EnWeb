using AutoMapper;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades.ViewModels;

namespace TPN1EnWeb.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LoadBrandMapping();
            LoadColourMapping();
            LoadGenreMapping();
            LoadSportMapping();
            LoadShoeMapping();
        }

        private void LoadShoeMapping()
        {
            CreateMap < Shoe, ShoeListVM>().
               ForMember(dest => dest.brand,
               opt => opt.MapFrom(b => b.Brands!.BrandName))
               .ForMember(dest => dest.color,
               opt => opt.MapFrom(c => c.Color!.ColorName))
                .ForMember(dest => dest.genre,
               opt => opt.MapFrom(g => g.Genres!.GenreName))
                 .ForMember(dest => dest.sport,
               opt => opt.MapFrom(s => s.Sports!.SportName))
                 .ForMember(dest=>dest.price, 
                 opt=>opt.MapFrom(p=>p.Price))
                 .ForMember(dest => dest.descripcion,
                 opt => opt.MapFrom(p => p.Descripcion))
                 .ForMember(dest => dest.model,
                 opt => opt.MapFrom(p => p.Model)).ReverseMap();
            CreateMap<Shoe, ShoeEditVM>().
                ForMember(dest=>dest.ColorId,opt=>opt.MapFrom(c=>c.ColourId)).ReverseMap();
            CreateMap<Shoe, ShoeListCustomerAreaVM>().
               ForMember(dest => dest.brand,
               opt => opt.MapFrom(b => b.Brands!.BrandName))
               .ForMember(dest => dest.color,
               opt => opt.MapFrom(c => c.Color!.ColorName))
                .ForMember(dest => dest.genre,
               opt => opt.MapFrom(g => g.Genres!.GenreName))
                 .ForMember(dest => dest.sport,
               opt => opt.MapFrom(s => s.Sports!.SportName))
                 .ForMember(dest => dest.price,
                 opt => opt.MapFrom(p => p.Price))
                 .ForMember(dest=>dest.CashPrice, opt=>opt.MapFrom(s=>s.Price*0.7m))
                 .ForMember(dest => dest.descripcion,
                 opt => opt.MapFrom(p => p.Descripcion))
                 .ForMember(dest => dest.model,
                 opt => opt.MapFrom(p => p.Model)).ReverseMap();
        }

        private void LoadSportMapping()
        {
            CreateMap<Sport,SportEditVM>().ReverseMap();
        }

        private void LoadGenreMapping()
        {
            CreateMap<Genre, GenreEditVM>().ReverseMap();   
        }

        private void LoadBrandMapping()
        {
            CreateMap<Brand, BrandEditVM>().ReverseMap();
        }

        private void LoadColourMapping()
        {
            CreateMap<Colour, ColourEditVM>().ReverseMap();//Acá genero un mapeo de distintas entidades automaticamente
                                                           //y le digo que tambien lo haga en viceversa
                                                           //Se da cuenta como mapear debido a que los nombres de las propiedades de ambas entidades son iguales
        }
    }
}
