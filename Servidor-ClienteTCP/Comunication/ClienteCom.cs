using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor_ClienteTCP.Comunication
{
    public class ClienteCom
    {
        //Declaramos atributos de esta clase
        private Socket cliente;
        private StreamReader reader;
        private StreamWriter writer;

        //Constructor de la clase
        public ClienteCom(Socket socket)
        {
            this.cliente = socket;
            Stream stream= new NetworkStream(this.cliente);
            this.reader= new StreamReader(stream);
            this.writer= new StreamWriter(stream);
        }

        //Funcion para desconectar el cliente del servidor
        public void Desconectar()
        {
            try
            {
                this.cliente.Close();

            }
            catch (Exception ex)
            {
            }
        }
        //Funcion para que el servidor le escriba al cliente
        public bool Escribir(string mensaje)
        {
            try
            {
                this.writer.WriteLine(mensaje);
                this.writer.Flush();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //Funcion para leer los mensajes del cliente
        public string Leer()
        {
            try
            {
                return this.reader.ReadLine().Trim();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
