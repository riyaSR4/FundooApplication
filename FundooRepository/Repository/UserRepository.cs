using FundooModel.User;
using FundooRepository.Context;
using FundooRepository.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace FundooRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly UserDbContext context;
        private readonly IConfiguration iconfiguration;
        public string Key = "ankit@@sehrawat@@";
        Nlog nlog = new Nlog();
        public UserRepository(UserDbContext context, IConfiguration iconfiguration)
        {
            this.context = context;
            this.iconfiguration = iconfiguration;
        }
        public Task<int> RegisterUser(Register register)
        {
            var password = EncryptPassword(register.Password);
            register.Password = password;
            this.context.Register.Add(register);
            var result = this.context.SaveChangesAsync();
            nlog.LogInfo("User Registered");
            return result;
        }
        public string LoginUser(Login login)
        {
            try
            {
                var result = this.context.Register.Where(x => x.Email.Equals(login.Email)).FirstOrDefault();
                var decryptPassword = DecryptPassword(result.Password);
               
                if (result != null && decryptPassword.Equals(login.Password))
                {
                    nlog.LogInfo("User Logged In");
                    var token = GenerateSecurityToken(result.Email, result.Id);
                    return token;
                }
                nlog.LogError("User not Logged In");
                return null;
            }
            catch (Exception)
            {
                nlog.LogError("Error in User Login");
                //return null;
                throw;
            }
        }

        public string GenerateSecurityToken(string email, int userId)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.iconfiguration[("JWT:Key")]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Id",userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }
        public string ForgetPassword(string Email)
        {
            try
            {
                var emailcheck = this.context.Register.FirstOrDefault(x => x.Email == Email);
                if (emailcheck != null)
                {
                    var token = GenerateSecurityToken(emailcheck.Email, emailcheck.Id);
                    MSMQ msmq = new MSMQ();
                    msmq.sendData2Queue(token);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Register ResetPassword(ResetPassword reset)
        {
            if(reset.NewPassword.Equals(reset.ConfirmPassword))
            {
                var input = this.context.Register.Where(x => x.Email.Equals(reset.Email)).FirstOrDefault();
                if (input != null)
                {
                    var password = EncryptPassword(reset.NewPassword);
                    input.Password = password;
                    this.context.Register.Update(input);
                    var result = this.context.SaveChangesAsync();
                    nlog.LogInfo("Password Reset Successful");
                    return input;
                }
                nlog.LogError("Password Reset Unsuccessful");
                return null;
            }
            nlog.LogError("Error in Password Reset");
            return null;
        }
        //Password Encrypt and Decrypt
        public string EncryptPassword(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            nlog.LogInfo("Password Encrypted");
            return strmsg;
        }
        public string DecryptPassword(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new string(decoded_char);
            nlog.LogInfo("Password Decrypted");
            return decryptpwd;
        }
    }
}
