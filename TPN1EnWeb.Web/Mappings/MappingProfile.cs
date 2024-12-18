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
            LoadCountriesMapping();
            LoadStatesMapping();
            LoadCitiesMapping();
            LoadColourMapping();
            LoadGenreMapping();
            LoadSportMapping();
            LoadShoeMapping();
            LoadApplicationUsersMapping();
            LoadShoppingCartsMapping();
            LoadOrderHeadersMapping();
        }
        private void LoadOrderHeadersMapping()
        {
            CreateMap<OrderHeaderEditVm, OrderHeader>()
                .ForMember(dest => dest.OrderDetail, opt => opt.MapFrom(
                    src => src.OrderDetails));
        }
        private void LoadShoppingCartsMapping()
        {
            CreateMap<ShoppingCartDetailVm, ShoppingCart>()
                .ForMember(dest => dest.ShoeSizeId, opt => opt.MapFrom(src=>src.ShoeSizeId))
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.ApplicationUserId));

            CreateMap<ShoppingCart, OrderDetail>()
                .ForMember(dest => dest.OrderHeaderId, opt => opt.Ignore())
                .ForMember(dest=>dest.ShoeSizes, opt=>opt.Ignore())
                .ForMember(dest => dest.ShoeSizeId, opt=>opt.MapFrom(src=>src.ShoeSizeId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (src.Quantity == 1 ? src.ShoeSize.Shoe.Price : src.ShoeSize.Shoe.Price * 0.9M)));
        }

        private void LoadCountriesMapping()
        {
            CreateMap<Country, CountryListVm>();
            CreateMap<Country, CountryEditVm>().ReverseMap();
        }
        private void LoadCitiesMapping()
        {
            CreateMap<City, CityListVm>().
                ForMember(dest => dest.CountryName,
                opt => opt.MapFrom(c => c.Country.CountryName))
                .ForMember(dest => dest.StateName,
                opt => opt.MapFrom(s => s.State.StateName));
            CreateMap<City, CityEditVm>().ReverseMap();
        }

        private void LoadStatesMapping()
        {
            CreateMap<State, StateListVm>()
                .ForMember(dest => dest.Country,
                opt => opt.MapFrom(src => src.Country.CountryName));
            CreateMap<State, StateEditVm>().ReverseMap();
        }
        private void LoadApplicationUsersMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserListVm>()
                 .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName));

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
                 opt => opt.MapFrom(p => p.Model))
                 .ForMember(dest => dest.active,
                 opt => opt.MapFrom(p => p.Active)).ReverseMap();
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
