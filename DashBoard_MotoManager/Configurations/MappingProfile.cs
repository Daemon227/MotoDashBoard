using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;

namespace DashBoard_MotoManager.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MotoBike, MotoVM>()
                .ForMember(dest => dest.MotoVersions, opt => opt.MapFrom(src => src.MotoVersions))
                .ForMember(dest => dest.MaLibraryNavigation, opt => opt.MapFrom(src => src.MaLibraryNavigation))
                .ForMember(dest => dest.MaHangSanXuatNavigation, opt => opt.MapFrom(src => src.MaHangSanXuatNavigation))
                .ForMember(dest => dest.MaLoaiNavigation, opt => opt.MapFrom(src => src.MaLoaiNavigation));

            CreateMap<MotoVM, MotoBike>()
                .ForMember(dest => dest.MaLibraryNavigation, opt => opt.MapFrom(src => src.MaLibraryNavigation))
                .ForMember(dest => dest.MotoVersions, opt => opt.MapFrom(src => src.MotoVersions))  
                .ForMember(dest => dest.MaHangSanXuatNavigation, opt => opt.MapFrom(src => src.MaHangSanXuatNavigation))
                .ForMember(dest => dest.MaLoaiNavigation, opt => opt.MapFrom(src => src.MaLoaiNavigation));

            CreateMap<MotoVersion, MotoVersionVM>()
                .ForMember(dest => dest.VersionColorVM, opt => opt.MapFrom(src => src.VersionColors)); 
            
            CreateMap<VersionColor, VersionColorVM>()
                .ForMember(dest => dest.VersionImageVM, opt => opt.MapFrom(src => src.VersionImages)); 
            
            CreateMap<VersionImage, VersionImageVM>(); 
            
            CreateMap<MotoLibrary, LibraryVM>()
                .ForMember(dest => dest.LibraryImageVM, opt => opt.MapFrom(src => src.LibraryImages));

            CreateMap<LibraryVM, MotoLibrary>()
                .ForMember(dest => dest.LibraryImages, opt => opt.MapFrom(src => src.LibraryImageVM));

            CreateMap<LibraryImage, LibraryImageVM>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.MaLibrary, opt => opt.MapFrom(src => src.MaLibrary))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<LibraryImageVM, LibraryImage>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.MaLibrary, opt => opt.MapFrom(src => src.MaLibrary))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
             
            CreateMap<MotoVersionVM, MotoVersion>()
                .ForMember(dest => dest.VersionColors, opt => opt.MapFrom(src => src.VersionColorVM));
            
            CreateMap<VersionColorVM, VersionColor>()
                .ForMember(dest => dest.VersionImages, opt => opt.MapFrom(src => src.VersionImageVM)); 
            
            CreateMap<VersionImageVM, VersionImage>();            

            CreateMap<BrandVM, Brand>()
                .ForMember(dest => dest.MaHangSanXuat, opt => opt.MapFrom(src => src.MaHangSanXuat))
                .ForMember(dest => dest.TenHangSanXuat, opt => opt.MapFrom(src => src.TenHangSanXuat))
                .ForMember(dest => dest.QuocGia, opt => opt.MapFrom(src => src.QuocGia))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan));

            // Cấu hình ánh xạ giữa Brand và BrandVM
            CreateMap<Brand, BrandVM>()
                .ForMember(dest => dest.MaHangSanXuat, opt => opt.MapFrom(src => src.MaHangSanXuat))
                .ForMember(dest => dest.TenHangSanXuat, opt => opt.MapFrom(src => src.TenHangSanXuat))
                .ForMember(dest => dest.QuocGia, opt => opt.MapFrom(src => src.QuocGia))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan));

            CreateMap<MotoTypeVM, MotoType>()
                .ForMember(dest => dest.MaLoai, opt => opt.MapFrom(src => src.MaLoai))
                .ForMember(dest => dest.TenLoai, opt => opt.MapFrom(src => src.TenLoai))
                .ForMember(dest => dest.DoiTuongSuDung, opt => opt.MapFrom(src => src.DoiTuongSuDung))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan));

            // Cấu hình ánh xạ giữa MotoType và MotoTypeVM
            CreateMap<MotoType, MotoTypeVM>()
                .ForMember(dest => dest.MaLoai, opt => opt.MapFrom(src => src.MaLoai))
                .ForMember(dest => dest.TenLoai, opt => opt.MapFrom(src => src.TenLoai))
                .ForMember(dest => dest.DoiTuongSuDung, opt => opt.MapFrom(src => src.DoiTuongSuDung))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan));
        }
    }
}
