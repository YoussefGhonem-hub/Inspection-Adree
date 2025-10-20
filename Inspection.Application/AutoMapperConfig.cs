using AutoMapper;
using Inspection.Application.Features.Inspectors.Queries.GetAllInspectorsQuery;
using Inspection.Domain.Entities;

namespace Inspection.Application;
public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {

        CreateMap<Inspector, InspectorDto>();

    }
}

