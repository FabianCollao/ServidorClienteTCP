using ServidorSocketUtils;
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
        //Colores de texto de la consola
        static ConsoleColor colorServerNombre = ConsoleColor.DarkCyan;
        static ConsoleColor colorClienteNombre = ConsoleColor.DarkGreen;
        static ConsoleColor colorServerMsg = ConsoleColor.Cyan;
        static ConsoleColor colorClienteMsg = ConsoleColor.Green;
        static ConsoleColor colorApp = ConsoleColor.Blue;
        static ConsoleColor colorSucces = ConsoleColor.Green;
        static ConsoleColor colorError = ConsoleColor.Red;
        static ConsoleColor colorAlerta = ConsoleColor.Yellow;

        //Función para personalizar el texto y agregar salto de linea
        static void ImprimirColor(string msg, ConsoleColor color, bool saltoLinea)
        {
            Console.ForegroundColor = color;
            if (saltoLinea)
            { Console.WriteLine(msg); }
            else
            { Console.Write(msg); }
            Console.ResetColor();
        }
        static void generarComunicacion(ClienteCom cliente)
        {
            bool seguir;
            string server_msg;
            //bucle infinito mientras ninguno escriba "chao"
            do
            {
                //Mensaje para el cliente
                ImprimirColor("Server: ", colorServerNombre, false);
                Console.ForegroundColor = colorServerMsg;
                server_msg = Console.ReadLine();

                //Validaciones para comandos personalizados
                switch (server_msg)
                {
                    case "!clear":
                        Console.Clear();
                        ImprimirColor("Server: ", colorServerNombre, false);
                        Console.ForegroundColor = colorServerMsg;
                        server_msg = Console.ReadLine();
                        break;
                    case "!zumbido":
                        ImprimirColor("LE HAS ENVIADO UN ZUMBIDO!!", colorAlerta, true);
                        server_msg = "Te ha enviado un zumbido!!";
                        break;
                    default:
                        break;
                }
                //Enviamos el mensaje al cliente
                cliente.Escribir(server_msg);
                string respuesta = cliente.Leer();
                //Obtenemos la respuesta y validamos comandos personalizados
                switch (respuesta)
                {
                    case "Te ha enviado un zumbido!!":
                        ImprimirColor("EL CLIENTE TE HA ENVIADO UN ZUMBIDO!!!", colorAlerta, true);
                        break;
                    default:
                        ImprimirColor("Cliente: ", colorClienteNombre, false);
                        ImprimirColor(respuesta, colorClienteMsg, true);
                        break;
                }
                //validación para terminar el bucle y cerrar la conexion con el cliente
                if (respuesta == "chao" || server_msg == "chao")
                {
                    seguir = false;
                    ImprimirColor("**Cliente Desconectado**", colorError, true);
                    cliente.Escribir("Adios!");
                    cliente.Desconectar();
                }
                else
                {
                    seguir = true;
                }
            } while (seguir);

        }
        static void Main(string[] args)
        {

            //intentamos iniciar servidor con los ajustes de la app
            try
            {
                //obtenemos el puerto desde la configuracion de la app
                int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
                ImprimirColor("Iniciando servidor en el puerto: ", colorApp, false);
                ImprimirColor(Convert.ToString(puerto), colorServerMsg, true);

                //Instanciamos nuesta clase con el puerto para que esté escuchando conexiones
                ServerSocket servidor = new ServerSocket(puerto);

                
                if (servidor.Iniciar())
                {
                    //Logramos iniciar el servidor
                    ImprimirColor("Servidor Iniciado", colorSucces, true);

                    //Bucle infinito donde espera una conexión
                    while (true)
                    {
                        ImprimirColor("Esperando Cliente...", colorApp, true);
                        Socket socketCliente = servidor.ObtenerCliente();

                        ClienteCom cliente = new ClienteCom(socketCliente);
                        ImprimirColor("                               Cliente conectado",ConsoleColor.Yellow,true);
                        generarComunicacion(cliente);

                    }
                }
                else
                {
                    Console.WriteLine("¡ERROR!... El puerto:{0} ya está en uso.", puerto);
                }

            } 
            catch (Exception ex)
            {
                ImprimirColor("Error "+ex.Message, colorError, true);
            }
        }
    }
}
