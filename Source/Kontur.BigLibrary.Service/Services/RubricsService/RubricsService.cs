using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Events;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;


namespace Kontur.BigLibrary.Service.Services.RubricsService;

public class RubricsService : IRubricsService
{
    private readonly IBookRepository bookRepository;
    private readonly IEventService eventService;
    private readonly ISynonymMaker synonymMaker;

    public RubricsService(IBookRepository bookRepository, IEventService eventService, ISynonymMaker synonymMaker)
    {
        this.bookRepository = bookRepository;
        this.eventService = eventService;
        this.synonymMaker = synonymMaker;
    }

    public async Task<Rubric> GetRubricAsync(int id, CancellationToken cancellation) =>
        await bookRepository.GetRubricAsync(id, cancellation);

    public async Task<IReadOnlyList<RubricSummary>> SelectRubricsSummaryAsync(CancellationToken cancellation) =>
        await bookRepository.SelectRubricsSummaryAsync(cancellation);

    public async Task<IReadOnlyList<RubricSummaryGroup>> SelectGroupsRubricSummaryAsync(CancellationToken cancellation)
    {

        var parentRubrics = await bookRepository.SelectParentRubricsSummaryAsync(cancellation);
        
        Console.WriteLine(JsonSerializer.Serialize(parentRubrics));
        Console.WriteLine("_________________________");
        var childRubrics = await bookRepository.SelectChildRubricsSummaryAsync(cancellation);

        Console.WriteLine(JsonSerializer.Serialize(childRubrics));
        Console.WriteLine("_________________________");
        
        var groupedRubrics = childRubrics.GroupBy(x => x.ParentId).ToArray();

        return parentRubrics.OrderBy(pr => pr.OrderId)
            .Select(pr =>
            {
                return new RubricSummaryGroup
                {
                    ParentRubric = pr,
                    Rubrics = groupedRubrics.FirstOrDefault(r => r.Key == pr.Id)?.ToArray()
                };
            }).ToList();
    }

    public async Task<Rubric> GetRubricBySynonymAsync(string synonym, CancellationToken cancellation) =>
        await bookRepository.GetRubricBySynonymAsync(synonym, cancellation);

    public async Task<RubricSummary> GetRubricSummaryBySynonymAsync(string synonym, CancellationToken cancellation) =>
        await bookRepository.GetRubricSummaryBySynonymAsync(synonym, cancellation);

    public async Task<Rubric> SaveRubricAsync(Rubric rubric, CancellationToken cancellation)
    {
        if (!rubric.Id.HasValue)
        {
            rubric.Id = await bookRepository.GetNextRubricIdAsync(cancellation);
            rubric.OrderId ??= rubric.Id;
        }

        var actualRubric = await bookRepository.SaveRubricAsync(rubric, cancellation);

        await SaveRubricIndexAsync(actualRubric, cancellation);
        var rubricChangedEvent = actualRubric.CreateChangedEvent();
        await eventService.PublishEventAsync(rubricChangedEvent, cancellation);

        return actualRubric;
    }

    private async Task SaveRubricIndexAsync(Rubric rubric, CancellationToken cancellation)
    {
        var synonym = synonymMaker.Create(rubric.Name);
        await bookRepository.SaveRubricIndexAsync(rubric.Id.Value, synonym, cancellation);
    }
}