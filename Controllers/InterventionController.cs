using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Models;
using Newtonsoft.Json.Linq;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionController : ControllerBase
    {
        private readonly app_developmentContext _context;

        public InterventionController(app_developmentContext context)
        {
            _context = context;
        }

        // GET: api/Intervention/Pending
        [HttpGet("Pending")]
        public ActionResult<List<Intervention>> GetAll ()
        {
            var list = _context.interventions.ToList();
            if (list == null)
            {
                return NotFound();
            }
            List<Intervention> pending_intervention_list = new List<Intervention>();
            foreach (var intervention in list)
            {
                if (intervention.start_date_and_time_of_the_intervention == null && intervention.status == "Pending") {
                    pending_intervention_list.Add(intervention);
                }
            }
            return pending_intervention_list;
        }

        [HttpGet("Customer/{customer_id}")]
        public async Task<ActionResult<Intervention>> GetCustomer(long customer_id)
        {
            var intervention = await _context.interventions.FindAsync(customer_id);

            if (intervention == null)
            {
                return NotFound();
            }

            return intervention;
        }

        // PUT: api/Intervention/InProgress/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("InProgress/{id}")]
        public async Task<IActionResult> InProgresstIntervention(long id, Intervention intervention)
        {
           var i = await _context.interventions.FindAsync (id);
            if (i == null) {
                return NotFound ();
           }


            DateTime aDate = DateTime.Now;
            i.status = intervention.status;
            i.start_date_and_time_of_the_intervention = aDate;

            _context.interventions.Update (i);
            _context.SaveChanges ();

            // Create a message to show the new status
            var status = new JObject ();
            status["message"] = "The status of the Intervention with the id number #" + i.Id + " have been changed to " + i.status;
            return Content (status.ToString (), "application/json");
        }
        // PUT: api/Intervention/Completed/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("Completed/{id}")]
        public async Task<IActionResult> CompletedIntervention(long id, Intervention intervention)
        {
           var i = await _context.interventions.FindAsync (id);
            if (i == null) {
                return NotFound ();
           }


            DateTime aDate = DateTime.Now;
            i.status = intervention.status;
            i.end_date_and_time_of_the_intervention = aDate;

            _context.interventions.Update (i);
            _context.SaveChanges ();

            // Create a message to show the new status
            var status = new JObject ();
            status["message"] = "The status of the Intervention with the id number #" + i.Id + " have been changed to " + i.status;
            return Content (status.ToString (), "application/json");
        }
        [HttpPost]
        public async Task<ActionResult<Intervention>> PostIntervention(Intervention intervention)
        {
            _context.interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntervention", new { id = intervention.Id }, intervention);
        }
    }
}
