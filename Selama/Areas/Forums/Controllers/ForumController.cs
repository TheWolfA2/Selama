﻿using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selama.Areas.Forums.Controllers
{
    public class ForumController : Controller
    {
        public ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Forums/Forum
        public ActionResult Index()
        {
            List<ForumSectionViewModel> forums = new List<ForumSectionViewModel>();
            foreach (ForumSection section in _db.ForumSections.Where(f => f.IsActive).OrderBy(f => f.DisplayOrder))
            {
                forums.Add(new ForumSectionViewModel(section));
            }
            return View(forums);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}