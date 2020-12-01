namespace EvgeniForum.Web.Areas.Administration.Controllers
{
    using EvgeniForum.Common;
    using EvgeniForum.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
