﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Areas.Forums.Controllers;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using Selama.Tests.Areas.Forums.Models.DAL;
using Selama.Tests.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Tests.Areas.Forums.Controllers
{
    [TestClass]
    public class ForumControllerTest : _BaseControllerUnitTest<ForumController>
    {
        #region Properties
        #region Private properties
        private const int NUM_AUTHORS = 2;
        private const int NUM_FORUM_SECTIONS = 2;
        private const int NUM_FORUMS = 2 * NUM_FORUM_SECTIONS;
        private const int NUM_THREADS = NUM_FORUMS * 3;

        private MockForumsUnitOfWork ControllerDb { get; set; }
        #endregion
        #endregion

        #region Setup tests
        protected override ForumController SetupController()
        {
            ControllerDb = new MockForumsUnitOfWork();
            return new ForumController(ControllerDb);
        }
        protected override void AdditionalSetup()
        {
            SetupAuthors();
            SetupForumSections();
            SetupForums();
            SetupThreads();
        }
        #endregion

        #region Methods
        #region Public methods
        [TestMethod]
        public void IndexIsNotNull()
        {
            // Arrange

            // Act
            ViewResult result = Controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #region Threads unit tests
        [TestMethod]
        public async Task ThreadsActionWithoutForumIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads() as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithInvalidThreadIdRedirectToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithPageNumLessThanOneRedirectsToPageOne()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 0) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToActionPageOne(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            // Arrange
            int forumId = 1;
            Forum forum = ControllerDb.Forums.FindById(forumId);
            ApplicationUser author = ControllerDb.Authors.Get().ToList()[0];
            for (int i = 0; i < ForumController.PAGE_SIZE; i++)
            {
                Thread tempThread = new Thread
                {
                    Content = string.Format("A sample paragraph for thread {0} in forum {1}", i, forumId),
                    IsActive = true,
                    IsLocked = false,
                    IsPinned = false,
                    PostDate = DateTime.Now,
                    Title = "Extra Thread " + i.ToString(),
                    Author = author,
                    AuthorId = author.Id,
                    Forum = forum,
                    ForumId = forum.Id,
                    Replies = new List<ThreadReply>(),
                };
                ControllerDb.Threads.Add(tempThread);
                forum.Threads.Add(tempThread);
            }

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 3) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Threads", result.RouteValues["action"].ToString());
            Assert.AreEqual(2, Convert.ToInt32(result.RouteValues["page"]));
        }

        [TestMethod]
        public async Task ThreadsWithInactiveForumRedirectsToIndex()
        {
            // Arrange
            ControllerDb.Forums.FindById(1).IsActive = false;

            // Act
            RedirectToRouteResult result = await Controller.Threads(1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithValidIdReturnsCorrectResult()
        {
            // Arrange

            // Act
            ViewResult result = await Controller.Threads(1) as ViewResult;

            // Assert
            ForumViewModel Model = result.Model as ForumViewModel;
            Assert.IsNotNull(result);
            Assert.IsNotNull(Model);
            Assert.AreEqual(ControllerDb.Forums.FindById(1).Title, Model.Title);
        }
        #endregion

        #region Thread unit tests
        [TestMethod]
        public async Task ThreadWithoutIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Thread() as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithInvalidIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Thread(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithInactiveThreadResultsInRedirectToIndex()
        {
            // Arrange
            ControllerDb.Threads.FindById(1).IsActive = false;

            // Act
            RedirectToRouteResult result = await Controller.Thread(1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithBelowOnePageNumRedirectsToPageOne()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Thread(1, 0) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToActionPageOne(result, "Thread");
        }

        [TestMethod]
        public void ThreadWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            // Arrange
            Thread t = ControllerDb.Threads.FindById(1);
            for (int i = 0; i < ForumController.PAGE_SIZE; i++)
            {
                var reply = new ThreadReply
                {
                    Thread = t,
                    ThreadId = t.Id,
                    Author = t.Author,
                    AuthorId = t.AuthorId,
                    Content = "Reply " + i + " to thread " + t.Title,
                    IsActive = true,
                    PostDate = DateTime.Now,
                    ReplyIndex = i,
                };
                t.Replies.Add(reply);
                ControllerDb.ThreadReplies.Add(reply);
            }

            // Act

            // Assert
        }

        [TestMethod]
        public async Task ThreadWithValidIdReturnsCorrectResult()
        {
            // Arrange

            // Act
            ViewResult result = await Controller.Thread(1) as ViewResult;

            // Assert
            ThreadViewModel Model = result.Model as ThreadViewModel;
            Assert.IsNotNull(result);
            Assert.IsNotNull(Model);
            Assert.AreEqual(ControllerDb.Threads.FindById(1).Title, Model.Title);
            Assert.AreEqual(ControllerDb.Threads.FindById(1).Content, Model.Content);
        }
        #endregion
        #endregion

        #region Private methods
        private void SetupAuthors()
        {
            for (int i = 0; i < NUM_AUTHORS; i++)
            {
                ControllerDb.Authors.Add(new ApplicationUser
                {
                    IsActive = true,
                    UserName = "App User " + i.ToString(),
                    Email = string.Format("appuser{0}@email.com", i),
                    EmailConfirmed = true,
                    ThreadReplies = new List<ThreadReply>(),
                    WaitingReview = false,
                    Threads = new List<Thread>(),
                });
            }
        }
        private void SetupForumSections()
        {
            for (int i = 0; i < NUM_FORUM_SECTIONS; i++)
            {
                ControllerDb.ForumSections.Add(new ForumSection
                {
                    DisplayOrder = i,
                    IsActive = true,
                    Title = "Forum Section " + i.ToString(),
                    Forums = new List<Forum>(),
                });
            }
        }
        private void SetupForums()
        {
            for (int i = 0; i < NUM_FORUMS; i++)
            {
                int forumSectionId = (i % NUM_FORUM_SECTIONS) + 1;
                ForumSection section = ControllerDb.ForumSections.FindById(forumSectionId);
                Forum forum = new Forum
                {
                    ForumSectionId = forumSectionId,
                    ForumSection = section,
                    IsActive = true,
                    Title = "Forum " + i.ToString(),
                    SubTitle = "Subtitle for forum " + i.ToString(),
                    Threads = new List<Thread>(),
                };
                section.Forums.Add(forum);
                ControllerDb.Forums.Add(forum);
            }
        }
        private void SetupThreads()
        {
            for (int i = 0; i < NUM_THREADS; i++)
            {
                int forumId = (i % NUM_FORUMS) + 1;
                Forum forum = ControllerDb.Forums.FindById(forumId);
                ApplicationUser author = ControllerDb.Authors.Get().ToList()[i % NUM_AUTHORS];
                Thread thread = new Thread
                {
                    Content = string.Format("This is a sample paragraph for thread {0} in forum {1}.", i, forumId),
                    Forum = forum,
                    ForumId = forumId,
                    IsActive = true,
                    IsLocked = false,
                    IsPinned = false,
                    PostDate = DateTime.Now,
                    Title = "Thread " + i.ToString(),
                    Replies = new List<ThreadReply>(),
                    Author = author,
                    AuthorId = author.Id
                };
                forum.Threads.Add(thread);
                ControllerDb.Threads.Add(thread);
            }
        }

        private void AssertIsRedirectToIndex(RedirectToRouteResult result, string redirectFromAction)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual(redirectFromAction, result.RouteValues["redirectFrom"].ToString());
        }

        private void AssertIsRedirectToActionPageOne(RedirectToRouteResult result, string redirectFromAction)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(redirectFromAction, result.RouteValues["action"].ToString());
            Assert.AreEqual(1, Convert.ToInt32(result.RouteValues["page"]));
        }
        #endregion
        #endregion
    }
}