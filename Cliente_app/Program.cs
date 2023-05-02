using ClienteSocketUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_app
{
    public class Program
    {

        //Colores de la aplicación
        static ConsoleColor colorServerNombre = ConsoleColor.DarkCyan;
        static ConsoleColor colorClienteNombre = ConsoleColor.DarkGreen;
        static ConsoleColor colorServerMsg = ConsoleColor.Cyan;
        static ConsoleColor colorClienteMsg = ConsoleColor.Green;
        static ConsoleColor colorApp = ConsoleColor.Blue;
        static ConsoleColor colorSucces = ConsoleColor.Green;
        static ConsoleColor colorError = ConsoleColor.Red;
        static ConsoleColor colorAlerta = ConsoleColor.Yellow;
        //Instanciamos sonidos, con su ubicación
        static string path_zumbido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sonido\zumbido.wav");
        static SoundPlayer snd_zumbido = new SoundPlayer(path_zumbido);
        static string path_encender = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sonido\startSystem.wav");
        static SoundPlayer snd_encender = new SoundPlayer(path_encender);
        static string path_apagar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sonido\shutdown.wav");
        static SoundPlayer snd_apagar = new SoundPlayer(path_apagar); 

        static void generarComunicacion(ClienteSocket clienteSocket)
        {
            string msg_server = "";
            string msg_client = "";
            //Variables solo de esta conexión
            bool seguir;

            do
            {
                msg_server = clienteSocket.Leer();  //Obtenemos el mensaje del servidor
                                                    //switch para validar mensajes personalizados que envia el servidor
                switch (msg_server)
                {
                    case "Te ha enviado un zumbido!!":
                        ImprimirColor("EL SERVIDOR TE HA ENVIADO UN ZUMBIDO!!!", colorAlerta, true);
                        snd_zumbido.Play();
                        break;
                    default:        //Mensajes normales
                        ImprimirColor("Server: ", colorServerNombre, false);
                        ImprimirColor(msg_server, colorServerMsg, true);
                        break;
                }
                ImprimirColor("Tú: ", colorClienteNombre, false);
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
                    ImprimirColor("**Desconectado del servidor**", colorError, true);
                    clienteSocket.Desconectar();

                    //sonido al desconectar
                    snd_apagar.Play();
                    seguir = false;
                }
                else
                {
                    seguir = true;
                }
            } while (seguir);

        }
        //Funcion para personalizar color del texto de la consola, también el salto de linea.
        static void ImprimirColor(string msg, ConsoleColor color, bool saltoLinea)
        {
            Console.ForegroundColor = color;
            if (saltoLinea)
            { Console.WriteLine(msg); }
            else
            { Console.Write(msg); }
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            //Variables para conectarse al servidor
            int puerto;
            string servidor;


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
                    ImprimirColor("**Conexión exitosa**", colorSucces, true);
                    //sonido de conexion exitosa
                    snd_encender.Play();
                    //Ciclo de Chat mientras ninguno escriba "chao"
                    generarComunicacion(clienteSocket);
                }
                else
                {
                    ImprimirColor("No se pudo conectar",colorError,true);
                    ImprimirColor("Intenta de nuevo ;)",colorAlerta,true);
                }
            }

        }
    }
}
