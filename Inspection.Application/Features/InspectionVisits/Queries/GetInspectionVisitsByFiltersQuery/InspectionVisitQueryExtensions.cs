using Inspection.Domain.Entities;

namespace Inspection.Application.Features.InspectionVisits.Queries.GetInspectionVisitsByFiltersQuery;
public static class InspectionVisitQueryExtensions
{
    public static IQueryable<InspectionVisit> ApplyFilters(this IQueryable<InspectionVisit> query, GetInspectionVisitsByFiltersQuery filter)
    {
        if (filter.StartDate.HasValue)
        {
            query = query.Where(iv => iv.ScheduledAt >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(iv => iv.ScheduledAt <= filter.EndDate.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(iv => iv.Status == filter.Status.Value);
        }

        if (filter.InspectorId.HasValue)
        {
            query = query.Where(iv => iv.InspectorId == filter.InspectorId.Value);
        }

        if (!string.IsNullOrEmpty(filter.Category))
        {
            query = query.Where(iv => iv.EntityToInspect.Category.Contains(filter.Category));
        }

        return query;
    }
}
