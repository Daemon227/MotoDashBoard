using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;

namespace DashBoard_MotoManager.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MotoBike, MotoDetailVM>()
                .ForMember(dest => dest.MotoVersions, opt => opt.MapFrom(src => src.MotoVersions))
                .ForMember(dest => dest.MaLibraryNavigation, opt => opt.MapFrom(src => src.MaLibraryNavigation))
                .ForMember(dest => dest.MaHangSanXuatNavigation, opt => opt.MapFrom(src => src.MaHangSanXuatNavigation))
                .ForMember(dest => dest.MaLoaiNavigation, opt => opt.MapFrom(src => src.MaLoaiNavigation));

            CreateMap<MotoDetailVM, MotoBike>()
                .ForMember(dest => dest.MotoVersions, opt => opt.MapFrom(src => src.MotoVersions))
                .ForMember(dest => dest.MaLibraryNavigation, opt => opt.MapFrom(src => src.MaLibraryNavigation))
                .ForMember(dest => dest.MaHangSanXuatNavigation, opt => opt.MapFrom(src => src.MaHangSanXuatNavigation))
                .ForMember(dest => dest.MaLoaiNavigation, opt => opt.MapFrom(src => src.MaLoaiNavigation));
            
            CreateMap<MotoVersion, MotoVersionVM>()
                .ForMember(dest => dest.VersionColorsVM, opt => opt.MapFrom(src => src.VersionColors)); 
            
            CreateMap<VersionColor, VersionColorVM>()
                .ForMember(dest => dest.versionImageVM, opt => opt.MapFrom(src => src.VersionImages)); 
            
            CreateMap<VersionImage, VersionImageVM>(); 
            
            CreateMap<MotoLibrary, MotoLibraryVM>()
                .ForMember(dest => dest.LibraryImages, opt => opt.MapFrom(src => src.LibraryImages)); 

            CreateMap<LibraryImage, LibraryImageVM>();

            CreateMap<MotoVersionVM, MotoVersion>()
                .ForMember(dest => dest.VersionColors, opt => opt.MapFrom(src => src.VersionColorsVM));
            
            CreateMap<VersionColorVM, VersionColor>()
                .ForMember(dest => dest.VersionImages, opt => opt.MapFrom(src => src.versionImageVM)); 
            
            CreateMap<VersionImageVM, VersionImage>(); 
            
            CreateMap<MotoLibraryVM, MotoLibrary>()
                .ForMember(dest => dest.LibraryImages, opt => opt.MapFrom(src => src.LibraryImages)); 
            
            CreateMap<LibraryImageVM, LibraryImage>();
    
            CreateMap<BrandVM, Brand>()
                .ForMember(dest => dest.MaHangSanXuat, opt => opt.MapFrom(src => src.MaHangSanXuat))
                .ForMember(dest => dest.TenHangSanXuat, opt => opt.MapFrom(src => src.TenHangSanXuat))
                .ForMember(dest => dest.QuocGia, opt => opt.MapFrom(src => src.QuocGia))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan))
                .ForMember(dest => dest.MotoBikes, opt => opt.Ignore()); // Ignore MotoBikes if not needed 
            
            // Cấu hình ánh xạ giữa Brand và BrandVM
            CreateMap<Brand, BrandVM>() 
                .ForMember(dest => dest.MaHangSanXuat, opt => opt.MapFrom(src => src.MaHangSanXuat)) 
                .ForMember(dest => dest.TenHangSanXuat, opt => opt.MapFrom(src => src.TenHangSanXuat)) 
                .ForMember(dest => dest.QuocGia, opt => opt.MapFrom(src => src.QuocGia)) 
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan)) 
                .ForMember(dest => dest.MotoBikes, opt => opt.MapFrom(src => src.MotoBikes));

            CreateMap<MotoTypeVM, MotoType>()
                .ForMember(dest => dest.MaLoai, opt => opt.MapFrom(src => src.MaLoai))
                .ForMember(dest => dest.TenLoai, opt => opt.MapFrom(src => src.TenLoai))
                .ForMember(dest => dest.DoiTuongSuDung, opt => opt.MapFrom(src => src.DoiTuongSuDung))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan))
                .ForMember(dest => dest.MotoBikes, opt => opt.Ignore()); // Ignore MotoBikes if not needed 
            // Cấu hình ánh xạ giữa MotoType và MotoTypeVM
            CreateMap<MotoType, MotoTypeVM>() 
                .ForMember(dest => dest.MaLoai, opt => opt.MapFrom(src => src.MaLoai)) 
                .ForMember(dest => dest.TenLoai, opt => opt.MapFrom(src => src.TenLoai))
                .ForMember(dest => dest.DoiTuongSuDung, opt => opt.MapFrom(src => src.DoiTuongSuDung))
                .ForMember(dest => dest.MoTaNgan, opt => opt.MapFrom(src => src.MoTaNgan))
                .ForMember(dest => dest.MotoBikes, opt => opt.MapFrom(src => src.MotoBikes));
        }
    }
}
