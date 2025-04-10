using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using PGPARS.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
        public JsonResult GetFundTypes(string term)
        {
            var results = _fundingRepository.GetFundTypesStartingWith(term);
            return Json(results);
        }

        [HttpGet]
        public JsonResult GetSources(string term)
        {
            var results = _fundingRepository.GetSourcesStartingWith(term);
            return Json(results);
        }


        [HttpGet]
        public IActionResult AddFunding() => View(new Funding());

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
            TempData["SuccessMessage"] = "New funding created!";
             _logger.LogAction("Add", User.Identity.Name, "Added " + funding.Source, "FUNDING");
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
                _logger.LogAction("Edit", User.Identity.Name, "Edited " + funding.Source, "FUNDING");

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
            var allocations = _fundingRepository.GetFundingAllocations().Where(a => a.FundingID == id).ToList();

            if (funding == null)
            {
                TempData["ErrorMessage"] = "Funding record not found.";
                return RedirectToAction("FundingDirectory");
            }

            // Delete allocations first
            if (allocations.Any())
            {
                foreach (var allocation in allocations)
                {
                    _fundingRepository.DeleteAllocation(allocation.Id);
                }

                _logger.LogAction("Delete", User.Identity.Name, $"Deleted allocations for funding: {funding.Source}", "ALLOCATIONS");
            }

            // Proceed with deleting the funding record
            _fundingRepository.DeleteFunding(id);
            _logger.LogAction("Delete", User.Identity.Name, $"Deleted funding: {funding.Source}", "FUNDING");

            TempData["SuccessMessage"] = "Funding and associated allocations deleted successfully.";
            return RedirectToAction("FundingDirectory");
        }

        // Show confirmation warning before deleting
        public IActionResult ConfirmDeleteFunding(int id)
        {
            // Fetch the funding source
            var fundingSource = _fundingRepository.GetFundingById(id);

            if (fundingSource == null)
            {
                return NotFound();
            }

            // Fetch associated allocations
            var associatedAllocations = _fundingRepository.GetFundingAllocations()
                .Where(a => a.FundingID == id)
                .ToList();

            // Pass data to the view using ViewBag or ViewData
            ViewBag.FundingSource = fundingSource.Source;
            ViewBag.Amount = fundingSource.Amount;
            ViewBag.Remaining = fundingSource.Remaining;
            ViewData["AssociatedAllocations"] = associatedAllocations;

            return View(fundingSource); // Pass the full model to the view
        }

        // GET: FundingDirectory
        public IActionResult FundingDirectory(
      string searchQuery,
      List<string> selectedCohorts,
      List<string> selectedSources,
      List<string> selectedFundTypes)
        {
            IEnumerable<Funding> fundingList = _fundingRepository.GetFunding();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                fundingList = _fundingRepository.SearchFunding(searchQuery);
            }
            else
            {
                fundingList = _fundingRepository.GetFunding();
            }

            // Apply Cohort filter
            if (selectedCohorts != null && selectedCohorts.Any())
            {
                var cohortInts = selectedCohorts.Select(int.Parse).ToList();
                fundingList = fundingList.Where(f => cohortInts.Contains(f.Cohort));
            }


            if (selectedSources != null && selectedSources.Any())
            {
                fundingList = fundingList.Where(f => selectedSources.Contains(f.Source));
            }

            if (selectedFundTypes != null && selectedFundTypes.Any())
            {
                fundingList = fundingList.Where(f => selectedFundTypes.Contains(f.FundType));
            }

            ViewData["SearchQuery"] = searchQuery;
            ViewData["SelectedCohorts"] = selectedCohorts ?? new List<string>();
            ViewData["SelectedSources"] = selectedSources ?? new List<string>();
            ViewData["SelectedFundTypes"] = selectedFundTypes ?? new List<string>();

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
        public async Task<IActionResult> Assign(FundingAllocation allocation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Applicants = _applicantRepository.GetApplicants();
                return View(allocation);
            }

            var funding = _fundingRepository.GetFundingById(allocation.FundingID);
            var applicant = await _applicantRepository.GetApplicantByIdAsync(allocation.Nnumber); // Ensure Applicant is loaded

            if (funding == null)
            {
                return NotFound("Funding not found.");
            }
            if (applicant == null)
            {
                return NotFound("Applicant not found.");
            }

            // Check if allocated amount exceeds remaining funds
            if (allocation.AllocatedAmount > funding.Remaining)
            {
                TempData["ErrorMessage"] = "Allocated amount exceeds remaining funding.";
                return View(allocation);
            }
            _fundingRepository.UpdateFunding(funding);  // Save the updated remaining amount

            // Create the allocation record
            var newAllocation = new FundingAllocation
            {
                FundingID = allocation.FundingID,
                Nnumber = allocation.Nnumber,
                Applicant = allocation.Applicant, // Include the applicant
                AllocatedAmount = allocation.AllocatedAmount,
                StipendValue = allocation.StipendValue,
                TuitionWaiver = allocation.TuitionWaiver,
                TuitionWaiverType = allocation.TuitionWaiverType,
                Status = allocation.Status,
                AllocatedNotes = allocation.AllocatedNotes
            };

            _fundingRepository.AddAllocation(newAllocation);
            TempData["SuccessMessage"] = "Funding allocation added successfully.";
            _logger.LogAction("Assigned", User.Identity.Name,
                $"Assigned {allocation.AllocatedAmount:C} to {applicant.FullName}",
                "ALLOCATION");

            return RedirectToAction("FundingDirectory");
        }


        [HttpGet]
        public IActionResult FundingAllocations()
        {
            var allocations = _fundingRepository.GetFundingAllocations();
            return View(allocations);
        }

        public async Task<IActionResult> FundingDetails(int id)
        {
            var funding = await _fundingRepository.GetFundingDetailsByIdAsync(id);
            if (funding == null)
            {
                return NotFound();
            }

            // Now funding.RemainingAmount will return the amount left after allocations
            return View(funding);
        }

        [HttpGet]
        public IActionResult EditAllocation(int id)
        {
            var allocation = _fundingRepository.GetFundingAllocationById(id);
            if (allocation == null)
            {
                return NotFound();
            }

            // Load the list of applicants for the dropdown
            ViewBag.Applicants = _applicantRepository.GetApplicants();

            // Additionally, load funding details for display if needed
            var funding = _fundingRepository.GetFundingById(allocation.FundingID);
            if (funding != null)
            {
                ViewBag.FundingSource = funding.Source;
                ViewBag.FundingAmount = funding.Amount;
                ViewBag.FundingRemaining = funding.Remaining;
            }

            return View(allocation);
        }

        [HttpPost]
        public IActionResult EditAllocation(FundingAllocation allocation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Applicants = _applicantRepository.GetApplicants();
                return View(allocation);
            }

            var existingAllocation = _fundingRepository.GetFundingAllocationById(allocation.Id);
            if (existingAllocation == null)
            {
                TempData["ErrorMessage"] = "Allocation not found.";
                return RedirectToAction("FundingAllocations");
            }

            var funding = _fundingRepository.GetFundingById(existingAllocation.FundingID);
            if (funding == null)
            {
                TempData["ErrorMessage"] = "Funding source not found.";
                return RedirectToAction("FundingAllocations");
            }

            // Calculate the difference in allocated amounts
            decimal amountDifference = allocation.AllocatedAmount - existingAllocation.AllocatedAmount;

            // Update remaining amount
            funding.Remaining -= amountDifference;

            if (funding.Remaining < 0)
            {
                TempData["ErrorMessage"] = "Allocation exceeds available funds.";
                ViewBag.Applicants = _applicantRepository.GetApplicants();
                return View(allocation);
            }

            _fundingRepository.UpdateFunding(funding);
            _fundingRepository.UpdateAllocation(allocation);

            // Ensure the Applicant is included before logging
            allocation = _fundingRepository.GetFundingAllocationById(allocation.Id);
            if (allocation.Applicant != null)
            {
                _logger.LogAction("Edit", User.Identity.Name,
                    $"Edited allocation for {allocation.Applicant.FullName}", "ALLOCATION");
            }

            TempData["SuccessMessage"] = "Funding allocation updated successfully.";
            return RedirectToAction("FundingAllocations");
        }



        [HttpPost]
        public IActionResult DeleteAllocation(int id)
        {
            var allocation = _fundingRepository.GetFundingAllocationById(id);
            if (allocation == null)
            {
                TempData["ErrorMessage"] = "Allocation not found.";
                return RedirectToAction("FundingAllocations");
            }

            var funding = _fundingRepository.GetFundingById(allocation.FundingID);
            if (funding != null)
            {
                // Restore the allocated amount back to remaining funds
                funding.Remaining += allocation.AllocatedAmount;
                _fundingRepository.UpdateFunding(funding);
            }

            _fundingRepository.DeleteAllocation(id);
            _logger.LogAction("Delete", User.Identity.Name, $"Deleted allocation for {allocation.Applicant.FullName}", "ALLOCATION");

            TempData["SuccessMessage"] = "Funding allocation deleted successfully.";
            return RedirectToAction("FundingAllocations");
        }

        public IActionResult AllocationDetails(int id)
        {
            var allocation = _fundingRepository.GetFundingAllocationById(id);
            var funding = _fundingRepository.GetFundingById(allocation.FundingID);

            ViewBag.ApplicantName = allocation.Applicant.FullName;
            ViewBag.Amount = allocation.AllocatedAmount;
            ViewBag.RemainingAmount = funding.Remaining;
            ViewBag.Source = funding.Source;
            ViewBag.FundType = funding.FundType;

            if (allocation == null)
                return NotFound();

            return View(allocation);
        }


    }
}
