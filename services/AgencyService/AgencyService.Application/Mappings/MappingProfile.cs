
using AgencyService.Application.Features.Agency.Commands.CreateAgency;
using AgencyService.Application.Features.Agency.Commands.UpdateAgency;
using AgencyService.Application.Features.Certificate.Commands;
using AgencyService.Application.Features.Certificate.Queries;
using AgencyService.Application.Models.Maintenances;
using AgencyService.Application.Models.SortCodes;
using AgencyService.Application.Models.Webhooks;
using AgencyService.Domain.Entities;
using AutoMapper;
using System.Reflection;
using AgencyResponse = AgencyService.Application.Features.Agency.Queries.GetAgency.AgencyResponse;

namespace AgencyService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Agency, AgencyResponse>()
            .ForMember(dest => dest.AgencyId, opt => opt.MapFrom(src => src.UId))
            .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AgencyCode, opt => opt.MapFrom(src => src.Code))
            .ReverseMap();
        
        CreateMap<Agency, CreateAgencyCommand>();

        CreateMap<CreateAgencyCommand, Agency>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AgencyCode))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AgencyName));

        CreateMap<CreateAgencyCommand, AgencyResponse>();

        CreateMap<Agency, UpdateAgencyCommand>();
        CreateMap<UpdateAgencyCommand, Agency>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AgencyCode))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AgencyName))
            .ForMember(dest => dest.UId, opt => opt.MapFrom(src => src.AgencyId));

        //CreateMap<List<AgencyIdSortCodeTuple>, List<AgencyIdSortCodeTupleDto>>();

        CreateMap<CreateCertificateCommand, Certificate>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CertificateName))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.CertificateKey))
            .ForPath(dest => dest.AgencyId, _=>_.Ignore());

        CreateMap<Certificate, CertificateResponse>()
            .ForMember(dest => dest.CertificateName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CertificateKey, opt => opt.MapFrom(src => src.Key))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.AgencyId, opt => opt.Ignore());

        CreateMap<SortCode, SortCodeDto>()
            .ReverseMap();

        CreateMap<CreateMaintenanceDto, Maintenance>();
        CreateMap<Maintenance, MaintenanceDto>()
            .ForMember(dest => dest.ResponseCode, opt => opt.MapFrom(src => src.ResponseCode.ToString()));

        
        CreateMap<Webhook, WebhookResponseDto>();
        CreateMap<Webhook, CreateWebhookDto>();

        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);

        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

        var argumentTypes = new Type[] { typeof(Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
