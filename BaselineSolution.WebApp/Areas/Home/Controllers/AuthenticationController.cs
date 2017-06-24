﻿using System.Web.Mvc;
using BaselineSolution.Bo.Internal.Security;
using BaselineSolution.Facade.Internal;
using BaselineSolution.WebApp.Areas.Home.ViewModels.Authentication;
using BaselineSolution.WebApp.Components.Models.Authentication;
using BaselineSolution.WebApp.Infrastructure.Bases;
using BaselineSolution.WebApp.Infrastructure.Extensions;

namespace BaselineSolution.WebApp.Areas.Home.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly ISecurityService _securityService;

        public AuthenticationController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        // GET: Home/Authentication
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View();

            UserSecurityBo bo;
            var response = _securityService.Login(model.UserName, model.Password, out bo);
            if (response.IsSuccess)
            {
                if (!response.GetValue())
                {
                    ModelState.AddModelErrorFor((LoginViewModel x) => x.Password,
                        Resources.WebApp.NoMatchingUsernameAndPassword);
                    return View(model);
                }
            }
            else
            {
                response.Messages.ForEach(x =>
                {
                    ModelState.AddModelError("", x.MessageText);
                });
                return View(model);
            }

            var languagesResponse = _securityService.GetLanguages();
            if (!languagesResponse.IsSuccess)
            {
                languagesResponse.Messages.ForEach(x =>
                {
                    ModelState.AddModelError("", x.MessageText);
                });
                return View(model);
            }

            User = new AuthenticatedCustomPrincipal(bo, languagesResponse.Values);
            new Authenticator(Request.RequestContext, bo).BuildCache(bo);

            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        public ActionResult Logout()
        {
            User = null;
            Session.Abandon();


            return RedirectToAction("Login");
        }
    }
}