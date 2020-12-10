namespace EvgeniForum.Web.Controllers
{
    using EvgeniForum.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        [HttpPost]
        public ActionResult Post()
        {
            return this.Ok();
        }
    }
}
