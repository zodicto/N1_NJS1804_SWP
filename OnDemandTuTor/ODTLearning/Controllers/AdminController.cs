using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public AdminController(IAdminRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("viewAllTutor")]
        public async Task<IActionResult> ViewListTutor()
        {
            try
            {
                var response = await _repo.GetListTutor();

                if (!response.Success)
                {
                    return NotFound(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }

        [HttpGet("viewAllRequestPending")]
        public async Task<IActionResult> ViewAllRequestPending()
        {
            try
            {
                var response = await _repo.GetListRequestPending();

                if (!response.Success)
                {
                    return NotFound(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }

        [HttpGet("viewAllRequestApproved")]
        public async Task<IActionResult> ViewAllRequestApproved()
        {
            try
            {
                var response = await _repo.GetListRequestApproved();

                if (!response.Success)
                {
                    return NotFound(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }

        [HttpGet("viewAllRequestReject")]
        public async Task<IActionResult> ViewAllRequetReject()
        {
            try
            {
                var response = await _repo.GetListRequestReject();

                if (!response.Success)
                {
                    return NotFound(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }
        [HttpGet("viewAllStudent")]
        public async Task<IActionResult> ViewListStudent()
        {
            try
            {
                var response = await _repo.GetListStudent();

                if (!response.Success)
                {
                    return NotFound(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }

        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var response = await _repo.DeleteAccount(id);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpGet("ViewAllComplaint")]
        public async Task<IActionResult> ViewAllComplaint()
        {
            var response = await _repo.GetAllComplaint();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewAllTransaction")]
        public async Task<IActionResult> ViewAllTransaction()
        {
            var response = await _repo.GetAllTransaction();

            return Ok(new
            {
                response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewRevenueByMonth")]
        public async Task<IActionResult> ViewRevenueByMonth(int year)
        {
            var response = await _repo.GetRevenueByMonth(year);

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewRevenueByWeek")]
        public async Task<IActionResult> ViewRevenueByWeek(int month, int year)
        {
            var response = await _repo.GetRevenueByWeek(month, year);

            return Ok(new
            {
                response.Success,
                response.Message,
                response.Data
            });
        }

        [HttpGet("ViewRevenueByYear")]
        public async Task<IActionResult> ViewRevenueByYear(int year)
        {
            var response = await _repo.GetRevenueByYear(year);

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewRevenueThisMonth")]
        public async Task<IActionResult> ViewRevenueThisMonth()
        {
            var response = await _repo.GetRevenueThisMonth();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewAmountStudent")]
        public async Task<IActionResult> ViewAmountStudent()
        {
            var response = await _repo.GetAmountStudent();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewAmountTutor")]
        public async Task<IActionResult> ViewAmountTutor()
        {
            var response = await _repo.GetAmountTutor();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}