using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using PGPARS.Services;
using System.Collections.Generic;
using System.Linq;


namespace PGPARS.Controllers
{
    public class FundingController : Controller
    {
        private readonly IFundingRepository _fundingRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly AuditLogService _logger;
        

        public FundingController(IFundingRepository fundingRepository, IApplicantRepository applicantRepository, AuditLogService auditLogService)
        {
            _fundingRepository = fundingRepository;
            _applicantRepository = applicantRepository;
            _logger = auditLogService;
        }
        
       
        [HttpGet]
        public IActionResult AddFunding()
        {
            return View(new Funding()); // Return a blank form for adding funding
        }

        [HttpPost]
        public IActionResult AddFunding(Funding funding)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation Errors:");
                errors.ForEach(error => Console.WriteLine(error));
                return View(funding);
            }

            Console.WriteLine("ModelState is valid. Funding is being added.");
            _fundingRepository.AddFunding(funding);
            return RedirectToAction("FundingDirectory");
        }

        // GET: EditFunding
        [HttpGet]
        public IActionResult EditFunding(int id)
        {
            var funding = _fundingRepository.GetFundingById(id);
            if (funding == null)
            {
                return NotFound();
            }
            return View(funding); // Return the populated form for editing
        }

        // POST: EditFunding
        [HttpPost]
        public IActionResult EditFunding(Funding funding)
        {
            if (ModelState.IsValid)
            {
                // Call the repository method to update the funding in the database
                _fundingRepository.UpdateFunding(funding);

                //log for editing
                _logger.LogAction("Edit", User.Identity.Name, "Edited " + funding.Source, "INFO");

                // Redirect back to the directory after updating
                return RedirectToAction("FundingDirectory");
            }

            // If the model state is invalid, return to the edit page with errors
            return View(funding);
        }

        // POST: DeleteFunding
        [HttpPost]
        public IActionResult DeleteFunding(int id)
        {
            var funding = _fundingRepository.GetFundingById(id);
            if (funding != null)
            {
                _fundingRepository.DeleteFunding(id); // Delete the funding from the repository
            }
            _logger.LogAction("Delete", User.Identity.Name, "Deleted " + funding.Source, "INFO");
            return RedirectToAction("FundingDirectory");
        } 

        // GET: FundingDirectory
        public IActionResult FundingDirectory(string searchQuery)
        {
            IEnumerable<Funding> fundingList;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                fundingList = _fundingRepository.SearchFunding(searchQuery);
            }
            else
            {
                fundingList = _fundingRepository.GetFunding();
            }

            ViewData["SearchQuery"] = searchQuery; // Preserve the search query in the view
            return View(fundingList);
        }

        [HttpGet]
        public IActionResult Assign(int id)
        {
            var funding = _fundingRepository.GetFundingById(id);
            if (funding == null)
            {
                return NotFound("Funding not found.");
            }

            var applicants = _applicantRepository.GetApplicants().ToList();

            ViewBag.Applicants = applicants;
            ViewBag.FundingSource = funding.Source;
            ViewBag.FundingAmount = funding.Amount;
            ViewBag.FundingRemaining = funding.Remaining;

            return View(new FundingAllocation
            {
                FundingID = funding.Id
            });
        }


        [HttpPost]
        public IActionResult Assign(FundingAllocation allocation)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                errors.ForEach(error => System.Diagnostics.Debug.WriteLine("❌ " + error));

                ViewBag.Applicants = _applicantRepository.GetApplicants();
                return View(allocation);
            }

            // Manually create a new FundingAllocation object to ensure all fields are correctly mapped ( was having an issue with direct model binding where the 
            // Id field was being set manually and causing an error as it is supposed to autoincrement)
            var newAllocation = new FundingAllocation
            {
                FundingID = allocation.FundingID,
                Nnumber = allocation.Nnumber,
                AllocatedAmount = allocation.AllocatedAmount,
                StipendValue = allocation.StipendValue,
                TuitionWaiver = allocation.TuitionWaiver,
                TuitionWaiverType = allocation.TuitionWaiverType,
                Status = allocation.Status
            };

            _fundingRepository.AddAllocation(newAllocation);

            return RedirectToAction("FundingDirectory");
        }






    }
}
