using Preferans.WebClient.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Preferans.WebClient.ApiControllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage VerifyUserAuthenticity()
        {
            HttpResponseMessage response = Request.CreateResponse();

            if (User.Identity.IsAuthenticated)            
                response.Content = new StringContent(User.Identity.Name);

            response.StatusCode = HttpStatusCode.OK;

            return response;
        }
    }
}
