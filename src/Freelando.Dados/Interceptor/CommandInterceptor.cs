using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Interceptor
{
    public class CommandInterceptor : DbCommandInterceptor // Herda o interceptor de comandos do EF Core
    {
        // Sobrescreve o método que intercepta a execução SINCRONA de comandos que retornam um DbDataReader
        // Isso é chamado antes do EF executar, por exemplo, um SELECT que será lido com um DataReader.
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, // 'command' é o DbCommand que o EF vai executar (CommandText, Parameters, etc.)
            CommandEventData eventData, // 'eventData' traz metadados sobre o evento (contexto, duração, etc.)
            InterceptionResult<DbDataReader> result // 'result' permite influenciar/suprimir a execução (normalmente só repassamos)
        )
        {
            // Guarda a cor de fundo atual para restaurar depois (boa prática)
            var previousBg = Console.BackgroundColor;

            // Muda o fundo do console para Verde para destacar a linha de log
            Console.BackgroundColor = ConsoleColor.Green;

            // Escreve no console o comando SQL que será executado e a data/hora
            // command.CommandText = texto SQL (ex.: "SELECT ...")
            Console.WriteLine($"Execução de comando: {command.CommandText} - Data/Hora: {DateTime.Now}");

            // Restaura a cor de fundo original (em vez de fixar Branco)
            Console.BackgroundColor = previousBg;

            // Chama a implementação base para continuar o pipeline normal de execução.
            // Se você quisesse cancelar/alterar a execução, poderia retornar um InterceptionResult customizado.
            return base.ReaderExecuting(command, eventData, result);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command, // Representa o comando SQL a ser executado (texto e parâmetros)
            CommandEventData eventData, // Contém informações sobre o evento (ex.: contexto, tempo, etc.)
            InterceptionResult<int> result // Permite influenciar o resultado da execução (ex.: cancelar ou substituir)
)
        {
            // Salva a cor de fundo atual do console (boa prática, para restaurar depois)
            var previousBg = Console.BackgroundColor;

            // Muda a cor de fundo para Verde para destacar o log
            Console.BackgroundColor = ConsoleColor.Green;

            // Escreve no console o comando SQL que será executado + a data/hora atual
            Console.WriteLine($"Execução de comando: {command.CommandText} - Data/Hora: {DateTime.Now}");

            // Restaura a cor de fundo anterior (em vez de fixar branco, que pode quebrar tema do console)
            Console.BackgroundColor = previousBg;

            // Continua o fluxo normal chamando a implementação base
            // Se quisesse impedir a execução ou alterar o resultado, poderia mexer no 'result'
            return base.NonQueryExecuting(command, eventData, result);
        }
    }
}