using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCrudApi.Models;
using Utils.Infrastructure;
using CoreCrudApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CoreCrudApi.Controllers
{
    
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IHubContext<ChatHub> HubContext
        {
            get;
            set;
        }
        private readonly UserTestDBContext _context;

        public UsersController(UserTestDBContext context, IHubContext<ChatHub> hubcontext)
        {
            try
            {
                HubContext = hubcontext;
                _context = context;
            }
            catch (Exception ex)
            {

                FileLogger.Info(ex.Message);
            }
            
            
        }

        // GET: api/TblUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblUser>>> List()
        {
            try
            {
                var result = await _context.TblUser.OrderByDescending(e => e.Id).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {

                FileLogger.Info(ex.Message);
                throw;
            }

            
        }

        // GET: api/TblUsers/5
        //[HttpGet("{id}")]
        public async Task<ActionResult<TblUser>> GetById(int id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);

            if (tblUser == null)
            {
                return NotFound();
            }

            return tblUser;
        }

        // PUT: api/TblUsers/5
        [HttpPost]
        public async Task<ActionResult<TblUser>> update(TblUser tblUser)
        {
            //if (id != tblUser.Id)
            //{
            //    return BadRequest();
            //}

            _context.Entry(tblUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //await _signalRClient.SendMessage("", "");
                return tblUser;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblUserExists(tblUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
           
            
        }

        // POST: api/TblUsers
        [HttpPost]
        public async Task<ActionResult<TblUser>> Insert(TblUser tblUser)
        {
            try
            {
                _context.TblUser.Add(tblUser);
                await _context.SaveChangesAsync();
            
                await this.HubContext.Clients.All.SendAsync("ReceiveMessage", "WEB","Insert");
                return CreatedAtAction("GetById", new { id = tblUser.Id }, tblUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }

        // DELETE: api/TblUsers/5
        [HttpGet]
        public async Task<ActionResult<TblUser>> DeleteById(int id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);
            if (tblUser == null)
            {
                return NotFound();
            }

            _context.TblUser.Remove(tblUser);
            await _context.SaveChangesAsync();
            //await _signalRClient.SendMessage("", "");
            return tblUser;
        }

        private bool TblUserExists(int id)
        {
            return _context.TblUser.Any(e => e.Id == id);
        }
    }
}


//https://stackoverflow.com/questions/46904678/call-signalr-core-hub-method-from-controller