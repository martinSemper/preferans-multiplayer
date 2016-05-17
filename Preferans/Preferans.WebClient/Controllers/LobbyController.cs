using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Preferans.WebClient.Controllers
{
    [Authorize]
    public class LobbyController : Controller
    {
        // GET: Lobby
        public ActionResult Index()
        {
            return View();
        }
    }
}