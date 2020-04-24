using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Models;
using System.Net.Mail;
using System;
using System.Text;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManagementContext _context = new UserManagementContext();

        Logger l = new Logger();


        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            l.LogAction("Requested list of all users.");
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                l.LogAction("Requested user(s) by id with no results.");
                return NotFound();
            }

            l.LogAction("Requested user(s) by id="+id+".");
            return users;
        }

        //Validate User
        [Route("validate{id}")]
        public async Task<ActionResult<Users>> ValidateUser(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                l.LogAction("User validation failed. (id=" + id + ")");
                return NotFound();
            }
            else
            {
                users.IsVerified = true;

                try
                {
                    await _context.SaveChangesAsync();
                    l.LogAction("User validation success! (id="+id+")");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(id))
                    {
                        l.LogAction("User validation failed. (id=" + id + ")");
                        return NotFound();
                    }
                    else
                    {
                        l.LogAction("User validation failed. (id=" + id + ")");
                        throw;
                    }
                }
            }

            return users;
        }

        //// PUT: api/Users/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUsers(int id, Users users)
        //{
        //    if (id != users.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(users).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UsersExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();
            
            try
            {
                int userId = users.Id;
                var emailLink = new StringBuilder("");
                emailLink.AppendFormat("<a href='https://localhost:44322/api/Users/validate{0}' target='_blank'>Click here to verify your account .. </a>", userId);
                string emailLinkStr = emailLink.ToString();
                string emailBody = $"Dear {users.FirstName}, {Environment.NewLine} Please, click on the following link to verify your account: {Environment.NewLine}" +
                    $" {emailLinkStr} {Environment.NewLine} SD-Air team";
            
                MailMessage message = new MailMessage("sd.airline@gmail.com", users.EmailAddress, "Account verification", emailBody);                
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("sd.airline@gmail.com", "Az123456!");
                client.Send(message);

                Console.WriteLine("Message send");
                l.LogAction("Email has been sent to user. (id=" + userId + ")");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.StackTrace);
                l.LogAction("Error while sending email.");
            }

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Users>> DeleteUsers(int id)
        //{
        //    var users = await _context.Users.FindAsync(id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(users);
        //    await _context.SaveChangesAsync();

        //    return users;
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> isDeletedUser(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                l.LogAction("Deletion failed. User could not be found.");
                return NotFound();
            }

            users.IsDeleted = true;
            l.LogAction("User is set to 'Deleted'. (id=" + id + ")");

            _context.Entry(users).State = EntityState.Modified;

            //_context.Users.Remove(users);

            await _context.SaveChangesAsync();

            return users;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
