﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using PGPARS.Data;
using System.Linq;

namespace PGPARS.Controllers;
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        // GET: AddReview
        [HttpGet]
        public IActionResult AddReview()
        {
            return View(new Review()); //blank form
        }
        // POST: AddReview
        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            if (ModelState.IsValid) 
            {
                _reviewRepository.AddReview(review);
                return RedirectToAction("ReviewDirectory");
            }
            return View(review); //return with errors
        }
    // GET: EditReview
    [HttpGet]
    public IActionResult EditReview(int id)
    {
        var review = _reviewRepository.GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }

        var model = new Review
        {
            ReviewNumber = review.ReviewNumber,
            NNumber = review.NNumber,
            FullName = review.FullName,
            Email = review.Email,
            PhoneNumber = review.PhoneNumber,
            Status = review.Status,
            Reviewer = review.Reviewer,
            AllGPA = review.AllGPA,
            PsychGPA = review.PsychGPA,
            GPAComment = review.GPAComment,
            CourseReqMet = review.CourseReqMet,
            CourseReqComments = review.CourseReqComments,
            LetterQuality = review.LetterQuality,
            ResumeQuality = review.ResumeQuality,
            ResExpQuality = review.ResExpQuality,
            ResumeComments = review.ResumeComments,
            WritingSampleQuality = review.WritingSampleQuality,
            WritingSampleComments = review.WritingSampleComments,
            LORRelevance = review.LORRelevance,
            LORQuality = review.LORQuality,
            LORComments = review.LORComments,
            OverallFitQuality = review.OverallFitQuality,
            OverallFitComments = review.OverallFitComments,
            DecRec = review.DecRec,
            FollowUp = review.FollowUp,
            FinalComments = review.FinalComments,
            Applicant = review.Applicant,
        };

        return View(model); // return with form for editing
    }
    // POST: EditReview
    [HttpPost]
        public IActionResult EditReview(Review review)
        {
            if (ModelState.IsValid)
            { 
                _reviewRepository.UpdateReview(review);
                return RedirectToAction("ReviewDirectory");
            }
            return View(review);
        }
        // POST: DeleteReview
        [HttpPost]
        public IActionResult DeleteReview(int id)
        {
            var review = (_reviewRepository.GetReviewById(id));
            if (review != null)
            {
                _reviewRepository.DeleteReview(id);
            }
            return RedirectToAction("ReviewDirectory");
        }
        public IActionResult ReviewDirectory()
        {
            var reviewList = _reviewRepository.GetReviews();
            return View(reviewList);
        }
    }


