using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;
using CampManagerWebUI.Models;

namespace CampManagerWebUI
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<SupplierOrganization, SupplierOrganizationViewModel>()
                       .ForMember(dest => dest.IdOrganization, opts => opts.MapFrom(src => src.Organization.Id));

                    cfg.CreateMap<MeasureOrganization, MeasureOrganizationViewModel>()
                        .ForMember(dest => dest.IdOrganization, opts => opts.MapFrom(src => src.Organization.Id));

                    cfg.CreateMap<ProductOrganization, ProductOrganizationViewModel>()
                        .ForMember(dest => dest.IdOrganization, opts => opts.MapFrom(src => src.Organization.Id))
                        .ForMember(dest => dest.MeasureName, opts => opts.MapFrom(src => src.Measure.Name))
                        .ForMember(dest => dest.IdMeasure, opts => opts.MapFrom(src => src.Measure.Id));

                    cfg.CreateMap<BaseOrganization, BaseOrganizationViewModel>()
                        .ForMember(dest => dest.IdOrganization, opts => opts.MapFrom(src => src.Organization.Id));

                    cfg.CreateMap<SeasonOrganization, SeasonOrganizationViewModel>()
                        .ForMember(dest => dest.IdBase, opts => opts.MapFrom(src => src.Base.Id))
                        .ForMember(dest => dest.BaseName, opts => opts.MapFrom(src => src.Base.Name));

                    cfg.CreateMap<Invoice, InvoiceViewModel>()
                        .ForMember(dest => dest.IdSeason, opts => opts.MapFrom(src => src.Season.Id))
                        .ForMember(dest => dest.SeasonName, opts => opts.MapFrom(src => src.Season.Name))
                        .ForMember(dest => dest.IdSupplier, opts => opts.MapFrom(src => src.Supplier.Id))
                        .ForMember(dest => dest.SupplierName, opts => opts.MapFrom(src => src.Supplier.Name));

                    cfg.CreateMap<InvoicePosition, InvoicePositionViewModel>()
                        .ForMember(dest => dest.IdProduct, opts => opts.MapFrom(src => src.Product.Id))
                        .ForMember(dest => dest.ProductName, opts => opts.MapFrom(src => src.Product.Name));

                });
        }
    }
}