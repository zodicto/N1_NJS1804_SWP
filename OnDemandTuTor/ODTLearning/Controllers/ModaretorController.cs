﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModaretorController : ControllerBase
    {
        private readonly IModaretorRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public ModaretorController(IModaretorRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }
        [HttpGet("ViewProfileTT")]
        public IActionResult GetProfileToConFirm( string IdTutor)
        {
            var list = _repo.GetTutorProfileToConFirm(IdTutor);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "get list tutor successfully",
                Data = list
            });
        }
    }
}