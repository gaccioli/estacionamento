using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace estacionamento.Data.Migrations
{
    public partial class AddTabelasBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VeiculoEstacionado",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoraEntrada = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue:DateTime.Now),
                    HoraSaida = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue:null),
                    Duracao = table.Column<TimeSpan>(type: "time", nullable: true, defaultValue: null),
                    HorasCobradas = table.Column<int>(type: "int", nullable: true),
                    PlacaVeiculo = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    ValorHora = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue:1),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue:0.00),
                    Status = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeiculoEstacionado", x => x.TicketId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VeiculoEstacionado");
        }
    }
}
