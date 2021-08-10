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
        
        public async Task<IActionResult> Index(string SearchString)
        {
            var placaVeiculo = from m in _context.VeiculoEstacionado
                         select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                placaVeiculo = placaVeiculo.Where(s => s.PlacaVeiculo.Contains(SearchString));
            }


            return View(await placaVeiculo.ToListAsync());
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

                    int horasCobradas = HoraAdicional(veiculoEstacionado.Duracao.Minutes, veiculoEstacionado.Duracao.Hours);

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
        
        private bool VeiculoEstacionadoExists(int id)
        {
            return _context.VeiculoEstacionado.Any(e => e.TicketId == id);
        }

        private bool ValidaEntradaVeiculo(string placa)
        {
            return _context.VeiculoEstacionado.Any(e => e.PlacaVeiculo == placa && e.Status == 0);
        }

        private int HoraAdicional (int minutos, int horas)
        {

            if (horas > 0 && minutos < 10)
            {
                return 0;
            }
            else
            {
                return 1;
            }

            
        }
       
    }
}
