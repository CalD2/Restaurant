using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly CommandeContext _context;

        public RestaurantsController(CommandeContext context)
        {
            _context = context;
        }

        /////////////// PLAT

        /**
        * Délivre une commande.
        * https://localhost:7197/api/restaurants/getAll
        */
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
          if (_context.Commandes == null)
          {
              return NotFound();
          }
            List<Commande> commandes = await _context.Commandes.Include(c => c.Plats).Include(c => c.Boissons).ToListAsync();

            return commandes;
        }

        /**
        * Obtient une commande à l'aide de son ID.
        * https://localhost:7197/api/restaurants/getCommande/{ID}
        */
        [HttpGet("getCommande/{id}")]
        public async Task<ActionResult<Commande>> GetCommandeById(long id)
        {
            if (_context.Commandes == null)
            {
                return NotFound();
            }
            Commande? commande = await _context.Commandes.Include(c => c.Plats).Include(c => c.Boissons).FirstOrDefaultAsync(c => c.Id == id);

            if (commande == null)
            {
                return NotFound();
            }

            return commande;
        }

        /**
        * Termine la préparation d'un plat
        * - Le plat ne doit pas être déjà terminé.
        * https://localhost:7197/api/restaurants/platok/{ID}
        */
        [HttpGet("platok/{id}")]
        public async Task<ActionResult<Plat>> PlatEnded(long id)
        {
            if (_context.Plats == null)
            {
                return NotFound();
            }
            // Recherche de plats
            var plat = await _context.Plats.FindAsync(id);

            if (plat == null)
            {
                return NotFound();
            }
            else // Le plat existe
            {
                if(true == plat.IsComplete)
                {
                    return Problem("La plat " + plat.Id + " est déjà prêt.");
                }
                plat.IsComplete = true;
                _context.Entry(plat).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return plat;
        }

        /**
        * Termine la préparation d'une boisson
        * - La boisson ne doit pas être déjà terminé.
        * https://localhost:7197/api/restaurants/platok/{ID}
        */
        [HttpGet("boissonok/{id}")]
        public async Task<ActionResult<Boisson>> BoissonEnded(long id)
        {
            if (_context.Boissons == null)
            {
                return NotFound();
            }
            // Recherche de boissons
            var boisson = await _context.Boissons.FindAsync(id);

            if (boisson == null)
            {
                return NotFound();
            }
            else // La boisson existe
            {
                if(true == boisson.IsComplete)
                {
                    return Problem("La boisson " + boisson.Id + " est déjà prête.");
                }
                boisson.IsComplete = true;
                _context.Entry(boisson).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return boisson;
        }

        /**
        * Délivre une commande.
        * - La commande ne doit pas déjà avoir été livrée.
        * - Les plats doivent tous être prêt.
        * https://localhost:7197/api/restaurants/delivred/{ID}
        */
        [HttpGet("delivred/{id}")]
        public async Task<ActionResult<Commande>> DelivredCommande(long id)
        {
            if (_context.Commandes == null)
            {
                return NotFound();
            }
            // Recherche de la commande
            Commande? commande = await _context.Commandes.Include(c => c.Plats).Include(c => c.Boissons).FirstOrDefaultAsync(c => c.Id == id);

            // N'existe pas
            if (commande == null)
            {
                return NotFound();
            }
            else // On délivre la commande
            {
                // Plat
                if(commande.Plats != null)
                {
                    foreach(Plat plat in commande.Plats)
                    {
                        if(!plat.IsComplete)
                        {
                            return Problem("Le plat " + plat.Id + " n'est pas terminé.");
                        }
                    }
                }
                // Boisson
                if(commande.Boissons != null)
                {
                    foreach(Boisson boisson in commande.Boissons)
                    {
                        if(!boisson.IsComplete)
                        {
                            return Problem("La boisson " + boisson.Id + " n'est pas prête.");
                        }
                    }
                }
                
                // Status
                if("DELIVRED" == commande.Status)
                {
                    return Problem("La commande " + commande.Id + " est déjà livrée.");
                }
                commande.Status = "DELIVRED";
                _context.Entry(commande).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

           

            return commande;
        }

     
        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {
          if (_context.Commandes == null)
          {
              return Problem("Entity set 'PlatContext.Plats'  is null.");
          }

            // On complète la commande
            commande.Status = "RECEIVED";
            
            _context.Commandes.Add(commande); 
            // Alimentation de l'id de commande 
            if(commande.Plats != null)
            {
                foreach(Plat plat in commande.Plats)
                {
                    plat.IdCommande = commande.Id;
                }
            }
            if(commande.Boissons != null)
            {
                foreach(Boisson boisson in commande.Boissons)
                {
                    boisson.IdCommande = commande.Id;
                }
            }
            
            await _context.SaveChangesAsync();
             

            //return CreatedAtAction("GetPlat", new { id = plat.Id }, plat);
            return CreatedAtAction(nameof(GetCommandeById), new { id = commande.Id }, commande);
        }



        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommande(long id)
        {
            if (_context.Commandes == null)
            {
                return NotFound();
            }
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandeExists(long id)
        {
            return (_context.Commandes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
