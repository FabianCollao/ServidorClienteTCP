using Servidor_ClienteTCP.Comunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor_ClienteTCP
{
    public class Program
    {
        static void Main(string[] args)
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            Console.WriteLine("Iniciando servidor en el puerto:{0}",puerto);
            ServerSocket servidor= new ServerSocket(puerto);

            if (servidor.Iniciar())
            {
                Console.WriteLine("Servidor Iniciado");
                while (true)
                {
                    Console.WriteLine("Esperando Cliente...");
                    Socket socketCliente = servidor.ObtenerCliente();

                    ClienteCom cliente = new ClienteCom(socketCliente);

                    cliente.Escribir("Hola cliente, enviame algo:");
                    string respuesta = cliente.Leer();
                    Console.WriteLine("El Cliente envió: {0}",respuesta);
                    cliente.Escribir("¡Adios!");
                    cliente.Desconectar();

                }
            }
            else
            {
                Console.WriteLine("¡ERROR!... El puerto:{0} ya está en uso.",puerto);
            }
        }
    }
}
