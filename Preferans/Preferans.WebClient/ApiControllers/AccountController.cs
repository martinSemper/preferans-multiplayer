﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Preferans.WebClient.ApiControllers
{
    public class AccountController : ApiController
    {
        [Authorize]
        [HttpGet]
        public bool VerifyUserAuthenticity()
        {
            return true;
        }
    }
}
