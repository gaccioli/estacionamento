using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace estacionamento.Models
{
    public class SaidaVeiculoEstacionado : EntradaVeiculoEstacionado
    {
        public DateTime HoraSaida { get; set; }
        public TimeSpan Duracao { get; set; }
        public decimal ValorHora { get; set; } = 1;
        public decimal ValorTotal { get; set; }
        public int HorasCobradas {get; set;}
        public int Status { get; set; }
    }
}
