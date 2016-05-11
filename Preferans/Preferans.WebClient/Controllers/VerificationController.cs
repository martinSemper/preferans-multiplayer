using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Preferans.WebClient.Controllers
{
    public class VerificationController : Controller
    {

        // GET: Verification
        [Authorize]
        public string Index()
        {
            return "ok";
        }
    }
}