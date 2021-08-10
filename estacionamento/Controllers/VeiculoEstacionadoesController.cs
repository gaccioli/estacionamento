using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using estacionamento.Data;
using estacionamento.Models;

namespace estacionamento.Controllers
{
    public class VeiculoEstacionadoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculoEstacionadoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VeiculoEstacionadoes
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.VeiculoEstacionado.ToListAsync());
        }

        // GET: VeiculoEstacionadoes/Details/5      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculoEstacionado = await _context.VeiculoEstacionado
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (veiculoEstacionado == null)
            {
                return NotFound();
            }
            // Aqui faz a regra para calculo do valor e tempo total

            

            veiculoEstacionado.HoraSaida = DateTime.Now;

            veiculoEstacionado.Duracao = veiculoEstacionado.HoraSaida.Subtract(veiculoEstacionado.HoraEntrada);

            veiculoEstacionado.ValorHora = 1;

            int horasCobradas = HoraAdicional(veiculoEstacionado.Duracao.Minutes);
            
            veiculoEstacionado.HorasCobradas = veiculoEstacionado.Duracao.Hours + horasCobradas;

            veiculoEstacionado.ValorTotal = veiculoEstacionado.HorasCobradas * veiculoEstacionado.ValorHora;
                                   
            return View(veiculoEstacionado);
        }

        // GET: VeiculoEstacionadoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VeiculoEstacionadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,HoraEntrada,HoraSaida,Duracao,PlacaVeiculo,ValorHora, HoraTotal, Status")] SaidaVeiculoEstacionado veiculoEstacionado)
        {
            if (ValidaEntradaVeiculo(veiculoEstacionado.PlacaVeiculo))
            {
                return NotFound();
            }
            veiculoEstacionado.HoraEntrada = DateTime.Now;
            veiculoEstacionado.Status = 0;

            if (ModelState.IsValid)
            {
                _context.Add(veiculoEstacionado);
                await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            return View(veiculoEstacionado);
        }

        // GET: VeiculoEstacionadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculoEstacionado = await _context.VeiculoEstacionado.FindAsync(id);

            int horasCobradas = HoraAdicional(veiculoEstacionado.Duracao.Minutes);
            veiculoEstacionado.HorasCobradas = veiculoEstacionado.Duracao.Hours + horasCobradas;
            veiculoEstacionado.ValorTotal = veiculoEstacionado.HorasCobradas * veiculoEstacionado.ValorHora;

            if (veiculoEstacionado == null)
            {
                return NotFound();
            }
            return View(veiculoEstacionado);
        }
      

        // POST: VeiculoEstacionadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,HoraEntrada,HoraSaida,Duracao,PlacaVeiculo,ValorHora, HoraTotal, Status")] SaidaVeiculoEstacionado veiculoEstacionado)
        {
            //veiculoEstacionado.Duracao = veiculoEstacionado.HoraSaida.Subtract(veiculoEstacionado.HoraEntrada);
            if (id != veiculoEstacionado.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    veiculoEstacionado.HoraSaida = DateTime.Now;

                    veiculoEstacionado.Duracao = veiculoEstacionado.HoraSaida.Subtract(veiculoEstacionado.HoraEntrada);

                    veiculoEstacionado.ValorHora = 1;

                    int horasCobradas = HoraAdicional(veiculoEstacionado.Duracao.Minutes);

                    veiculoEstacionado.HorasCobradas = veiculoEstacionado.Duracao.Hours + horasCobradas;

                    veiculoEstacionado.ValorTotal = veiculoEstacionado.HorasCobradas * veiculoEstacionado.ValorHora;

                    veiculoEstacionado.Status = 1;
                    

                    _context.Update(veiculoEstacionado);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoEstacionadoExists(veiculoEstacionado.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(veiculoEstacionado);
        }

        // GET: VeiculoEstacionadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculoEstacionado = await _context.VeiculoEstacionado
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (veiculoEstacionado == null)
            {
                return NotFound();
            }

            return View(veiculoEstacionado);
        }


        // POST: VeiculoEstacionadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculoEstacionado = await _context.VeiculoEstacionado.FindAsync(id);
            _context.VeiculoEstacionado.Remove(veiculoEstacionado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("SaidaVeiculo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmedExit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculoEstacionado = await _context.VeiculoEstacionado
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (veiculoEstacionado == null)
            {
                return NotFound();
            }

            _context.VeiculoEstacionado.Update(veiculoEstacionado);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoEstacionadoExists(int id)
        {
            return _context.VeiculoEstacionado.Any(e => e.TicketId == id);
        }

        private bool ValidaEntradaVeiculo(string placa)
        {
            return _context.VeiculoEstacionado.Any(e => e.PlacaVeiculo == placa && e.Status == 0);
        }

        private int HoraAdicional (int i)
        {
            if (i >= 10)
            {
                return 1;
            }
            else return 0;

            
        }
    }
}
