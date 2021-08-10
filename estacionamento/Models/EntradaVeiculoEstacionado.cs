using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace estacionamento.Models
{
    public class EntradaVeiculoEstacionado
    {

        [Key]
        public int TicketId { get; set; }

        public DateTime HoraEntrada { get; set; }    

        [Required(ErrorMessage = "A Placa é obrigatório, insira uma placa válida", AllowEmptyStrings = false)]
        [StringLength(7, MinimumLength = 7)]
        public string PlacaVeiculo { get; set; }
        
        
        
    }
}
