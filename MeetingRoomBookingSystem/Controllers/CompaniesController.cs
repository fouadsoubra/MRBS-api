using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRBS.Core.Models;
using MRBS.Services.Interfaces;
using AutoMapper;
using MeetingRoomBookingSystem.Resources;
using MeetingRoomBookingSystem.Validators;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.AspNetCore.Authorization;
using MRBS.Services;

namespace MeetingRoomBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyService companyService, IMapper mapper)
        {
            this._mapper = mapper;
            this._companyService = companyService;
        }

        // GET: api/Companies
        [HttpGet("getAllCompanies")]
        public async Task<ActionResult<IEnumerable<CompanyResource>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompanies();
            var companyResources = _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyResource>>(companies);

            return Ok(companyResources);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyResource>> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyById(id);
            var companyResource = _mapper.Map<Company, CompanyResource>(company);

            return Ok(companyResource);
        }
  



        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("editCompany")]
        public async Task<ActionResult<CompanyResource>> UpdateCompany(int id, [FromBody] SaveCompanyResource saveCompanyResource)
        {
            var validator = new SaveCompanyResourceValidator();
            var validationResult = await validator.ValidateAsync(saveCompanyResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var companyToBeUpdated = await _companyService.GetCompanyById(id);

            if (companyToBeUpdated == null)
                return NotFound();

            var company = _mapper.Map<SaveCompanyResource, Company>(saveCompanyResource);

            await _companyService.UpdateCompany(companyToBeUpdated, company);

            var updatedCompany = await _companyService.GetCompanyById(id);

            var updatedCompanyResource = _mapper.Map<Company, CompanyResource>(updatedCompany);

            return Ok(updatedCompanyResource);
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
         [HttpPost("addCompany")]
         public async Task<ActionResult<CompanyResource>> CreateCompany([FromBody] SaveCompanyResource saveCompanyResource)
         {
             var validator = new SaveCompanyResourceValidator();
             var validationResult = await validator.ValidateAsync(saveCompanyResource);
             if (await _companyService.CompanyExistsAsync(saveCompanyResource.EmailAddress))
             {
                 return BadRequest("Company with that email already Registered.");
             }
             if (await _companyService.CompanyNameExistsAsync(saveCompanyResource.Name))
             {
                 return BadRequest("This Company name is already Registered");
             }

             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

             var companyToCreate = _mapper.Map<SaveCompanyResource, Company>(saveCompanyResource);

             var newCompany = await _companyService.CreateCompany(companyToCreate);

             var company = await _companyService.GetCompanyById(newCompany.Id);

             var companyResource = _mapper.Map<Company, CompanyResource>(company);

             return Ok(companyResource);
         }




        // DELETE: api/Companies/5
        [HttpDelete("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _companyService.GetCompanyById(id);

            await _companyService.DeleteCompany(company);

            return NoContent();
        }

    }
}
