using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.DAL.Entities;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Quản trị viên")]
        public async Task<IActionResult> ViewListTutor()
        {
            try
            {
                var response = await _repo.GetListTutor();

                if (!response.Success)
                {
                    return BadRequest(new
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [HttpGet("ViewAllReview")]
        [Authorize(Roles = "Quản trị viên")]
        public async Task<IActionResult> ViewAllReview()
        {
            var response = await _repo.GetAllReview();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewAllTransaction")]
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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
        [Authorize(Roles = "Quản trị viên")]
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

        [HttpGet("ViewRevenueToday")]
        [Authorize(Roles = "Quản trị viên")]
        public async Task<IActionResult> ViewRevenueToday()
        {
            var response = await _repo.GetRevenueToday();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}