using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackHub.Demo.Services.Data;
using StackHub.Demo.Models.Request;
using System.Text;
using Hangfire;
using System.Security.Cryptography;

namespace StackHub.Demo.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministratorController (DataContext context) : Controller
    {
        private readonly DataContext _context = context;

        
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(AdministratorRequest administratorRequest)
        {
            byte[] passwordHash;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                passwordHash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(administratorRequest.Password!));
            }

            await _context!.Administrators!.AddAsync(new Models.Administrator()
            {
                Id = Guid.NewGuid(),
                Username = administratorRequest.Username,
                Email = administratorRequest.Email,
                PasswordHash = passwordHash,
                IsConnected = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.MinValue
            });

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                BackgroundJob.Schedule(() => Services.EmailServices.WelcomeEmail(administratorRequest.Email!, administratorRequest.Username!), new DateTimeOffset(DateTime.UtcNow));
                
                return Ok();
            }
            else
            {
                return StatusCode(500, "Unable to save changes.");
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<ActionResult<AdministratorLogInRequest>> LogIn(AdministratorLogInRequest administratorLogInRequest)
        {
            try
            {
                var administrator = await _context!.Administrators!.FirstOrDefaultAsync(a => a.Email == administratorLogInRequest.Email);

                if (administrator == null)
                {
                    return NotFound();
                }
                else
                {
                    byte[] passwordHash;

                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        passwordHash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(administratorLogInRequest.Password!));
                    }

                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        for (int i = 0; i < passwordHash.Length; i++)
                        {
                            if (passwordHash[i] != administrator.PasswordHash![i])
                            {
                                return Unauthorized();
                            }
                        }
                    }

                    return Ok(new Models.Administrator
                    {
                        Id = administrator.Id,
                        Username = administrator.Username,
                        Email = administrator.Email,
                        CreatedAt = administrator.CreatedAt,
                        UpdatedAt = administrator.UpdatedAt
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<Models.Administrator>>> GetAll()
        {
            var administrators = await _context!.Administrators!.ToListAsync();

            if(administrators == null)
            {
                return NotFound();
            }

            return Ok(administrators);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult> Get(Guid id)
        {
            var administrator = await _context!.Administrators!.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            return Ok(administrator);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> Update(Models.Administrator administrator)
        {
            var currentAdministrator = await _context!.Administrators!.FindAsync(administrator.Id);

            if (currentAdministrator == null)
            {
                return NotFound();
            }

            currentAdministrator!.Username = administrator.Username;
            currentAdministrator!.Email = administrator.Email;
            currentAdministrator!.PasswordHash = administrator.PasswordHash;
            currentAdministrator!.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var clientUser = await _context!.Administrators!.FindAsync(id);

            if (clientUser == null)
            {
                return NotFound();
            }

            _context!.Administrators!.Remove(clientUser!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
