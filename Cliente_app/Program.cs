using Cliente_app.Comunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Variables para conectarse al servidor
            int puerto;
            string servidor;

            //Colores de la aplicación
            ConsoleColor colorServerNombre = ConsoleColor.DarkCyan;
            ConsoleColor colorClienteNombre = ConsoleColor.DarkGreen;
            ConsoleColor colorServerMsg = ConsoleColor.Cyan;
            ConsoleColor colorClienteMsg = ConsoleColor.Green;
            ConsoleColor colorApp = ConsoleColor.Blue;
            ConsoleColor colorSucces= ConsoleColor.Green;
            ConsoleColor colorError= ConsoleColor.Red;
            ConsoleColor colorAlerta= ConsoleColor.Yellow;

            //Espera infinitamente una conexión con algún cliente
            while (true)
            {
                //Ingreso de ip y puerto del servidor para conectarse
                ImprimirColor("Ingresa la ip del servidor :",colorApp,false);
                servidor = Console.ReadLine().Trim();
                ImprimirColor("Ingresa el puerto :",colorApp,false);
                puerto = Convert.ToInt32(Console.ReadLine().Trim());
                ImprimirColor("Conectando a servidor ",colorApp,false);
                ImprimirColor(servidor+":"+Convert.ToString(puerto),colorServerMsg,true);

                //Creamos el socket del cliente para conectarnos al servidor
                ClienteSocket clienteSocket = new ClienteSocket(servidor, puerto);

                //Validamos si podemos iniciar una conexión
                if (clienteSocket.Conectar())
                {
                   //Variables solo de esta conexión
                    bool seguir;
                    string msg_server;
                    string msg_client;

                    ImprimirColor("**Conexión exitosa**", colorSucces, true);
                    //Ciclo de Chat mientras ninguno escriba "chao"
                    do
                    {
                        msg_server = clienteSocket.Leer();  //Obtenemos el mensaje del servidor
                        //switch para validar mensajes personalizados que envia el servidor
                        switch (msg_server)
                        {
                            case "Te ha enviado un zumbido!!":
                                ImprimirColor("EL SERVIDOR TE HA ENVIADO UN ZUMBIDO!!!",colorAlerta,true);
                                break;
                            default:        //Mensajes normales
                                ImprimirColor("Server: ", colorServerNombre, false);
                                ImprimirColor(msg_server, colorServerMsg, true);
                                break;
                        }
                        ImprimirColor("Tú: ",colorClienteNombre,false);
                        Console.ForegroundColor = colorClienteMsg;
                        msg_client = Console.ReadLine().Trim();
                        Console.ResetColor();
                        //switch para validar comandos que escriba el cliente
                        switch (msg_client)
                        {
                            case "!zumbido":
                                ImprimirColor("HAS ENVIADO UN ZUMBIDO!!", colorAlerta, true);
                                msg_client = "Te ha enviado un zumbido!!";
                                break;
                            default:
                                break;
                        }
                        //Enviamos el mensaje escrito
                        clienteSocket.Escribir(msg_client);
                        //Validacion para terminar la conexion del chat
                        if (msg_client == "chao" || msg_server == "chao")
                        {
                            ImprimirColor("**Desconectado del servidor**",colorError,true);
                            clienteSocket.Desconectar();
                            seguir = false;
                        }
                        else
                        {
                            seguir = true;
                        }
                    } while (seguir);
                }
                else
                {
                    ImprimirColor("No se pudo conectar",colorError,true);
                    ImprimirColor("Intenta de nuevo ;)",colorAlerta,true);
                }
            }

            //Funcion para personalizar color del texto de la consola, también el salto de linea.
            void ImprimirColor(string msg, ConsoleColor color, bool saltoLinea)
            {
                Console.ForegroundColor = color;
                if (saltoLinea)
                { Console.WriteLine(msg); }
                else
                { Console.Write(msg); }
                Console.ResetColor();
            }
        }
    }
}
