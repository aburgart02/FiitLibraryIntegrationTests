using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.RubricsService;

public interface IRubricsService
{
    Task<Rubric> GetRubricAsync(int id, CancellationToken cancellation);
    Task<IReadOnlyList<RubricSummaryGroup>> SelectGroupsRubricSummaryAsync(CancellationToken cancellation);
    Task<Rubric> GetRubricBySynonymAsync(string synonym, CancellationToken cancellation);
    Task<RubricSummary> GetRubricSummaryBySynonymAsync(string synonym, CancellationToken cancellation);
    Task<Rubric> SaveRubricAsync(Rubric rubric, CancellationToken cancellation);
}