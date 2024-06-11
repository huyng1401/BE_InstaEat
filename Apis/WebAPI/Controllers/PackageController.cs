﻿using Application.Interfaces;
using Application.ViewModels.PackageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PackageController : BaseController
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PackageViewModel>>> GetPackages()
        {
            var packages = await _packageService.GetPackagesAsync();
            return Ok(packages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageViewModel>> GetPackageById(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        [HttpPost]
        public async Task<ActionResult<PackageViewModel>> CreatePackage([FromBody] CreatePackageViewModel createPackageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var package = await _packageService.CreatePackageAsync(createPackageViewModel);
            if (package == null)
            {
                return BadRequest("Unable to create package.");
            }
            return CreatedAtAction(nameof(GetPackageById), new { id = package.PackageId }, package);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] UpdatePackageViewModel updatePackageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isUpdated = await _packageService.UpdatePackageAsync(id, updatePackageViewModel);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok("Successfully Updated!!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var isDeleted = await _packageService.DeletePackageAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok("Successfully Deleted!!");
        }
    }
}
