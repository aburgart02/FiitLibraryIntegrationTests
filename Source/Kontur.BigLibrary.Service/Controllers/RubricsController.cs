using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Filters;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.RubricsService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Kontur.BigLibrary.Service.Controllers
{
    [Authorize]
    [Route("api/rubrics")]
    [ApiController]
    public class RubricsController : ControllerBase
    {
        private readonly IRubricsService rubricsService;


        public RubricsController(IRubricsService rubricsService) => this.rubricsService = rubricsService;

        [HttpGet("{id}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult<Rubric>> Get(int id) =>
            await rubricsService.GetRubricAsync(id, CancellationToken.None);

        [HttpGet("synonym/{synonym}")]
        public async Task<ActionResult<Rubric>> GetBySynonym(string synonym) =>
            await rubricsService.GetRubricBySynonymAsync(synonym, CancellationToken.None);

        [HttpPost]
        public async Task<ActionResult<Rubric>> Post([FromBody] Rubric rubric) =>
            await rubricsService.SaveRubricAsync(rubric, CancellationToken.None);

        [HttpGet("summary/{synonym}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult<RubricSummary>> GetSummary(string synonym) =>
            await rubricsService.GetRubricSummaryBySynonymAsync(synonym, CancellationToken.None);

        [HttpGet("summary/select")]
        public async Task<ActionResult<IEnumerable<RubricSummaryGroup>>> SelectSummary()
        {
            var result = await rubricsService.SelectGroupsRubricSummaryAsync(CancellationToken.None);
            return Ok(result);
        }
    }
}