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

                });
        }
    }
}