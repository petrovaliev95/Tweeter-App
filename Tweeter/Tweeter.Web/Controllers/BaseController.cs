﻿namespace Tweeter.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.UnitOfWork;
    using Microsoft.AspNet.Identity;
    using Tweeter.Models;

    public abstract class BaseController : Controller
    {
        private ITweeterData data;
        private User userProfile;

        protected BaseController(ITweeterData data)
        {
            this.Data = data;
        }

        protected BaseController(ITweeterData data, User userProfile)
            : this(data)
        {
            this.UserProfile = userProfile;
        }

        public ITweeterData Data
        {
            get { return this.data; }
            private set { this.data = value; }
        }

        public User UserProfile
        {
            get { return this.userProfile; }
            private set { this.userProfile = value; }
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.GetUserName();
                var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
                this.UserProfile = user;
            }
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}