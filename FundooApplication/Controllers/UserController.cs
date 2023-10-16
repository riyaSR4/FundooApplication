using FundooManager.IManager;
using FundooModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserManager userManager;
        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> UserRegister(Register register)
        {
            try
            {
                var result = await this.userManager.RegisterUser(register);
                if(result ==1)
                {
                    return this.Ok(new { Status = true, Message = "User Registration Successful", Data = register });
                }
                return this.BadRequest(new { Status = false, Message = "User Registration Unsuccessful" });
            }catch(Exception ex)
            {
                return this.NotFound(new {Status = false, Message = ex.Message});
            }
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult UserLogin(Login login)
        {
            try
            {
                var result = this.userManager.LoginUser(login);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "User Login Successful", Data = result });
                }
                return this.BadRequest(new { Status = false, Message = "User Login Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("ResetPassword")]
        public ActionResult UserResetPassword(ResetPassword resetPassword)
        {
            try
            {
                var result = this.userManager.ResetPassword(resetPassword);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "User Password Reset Successful", Data = resetPassword });
                }
                return this.BadRequest(new { Status = false, Message = "User Password Reset Unsuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("ForgetPassword")]

        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var resultLog = this.userManager.ForgetPassword(email);

                if (resultLog != null)
                {
                    return Ok(new { success = true, message = "Reset Email Send" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Reset UnSuccessful" });
                }

            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}
