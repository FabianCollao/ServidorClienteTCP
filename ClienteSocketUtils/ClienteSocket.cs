using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClienteSocketUtils
{
    public class ClienteSocket
    {
        private int puerto;
        private string servidor;
        private Socket cliente_socket;
        private StreamReader reader;
        private StreamWriter writer;

        public ClienteSocket(string servidor, int puerto)
        {
            this.servidor = servidor;
            this.puerto = puerto;
        }
        public bool Conectar()
        {
            try
            {
                this.cliente_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
                IPEndPoint endPoint= new IPEndPoint(IPAddress.Parse(this.servidor),this.puerto);
                this.cliente_socket.Connect(endPoint);
                Stream stream = new NetworkStream(this.cliente_socket);
                this.reader= new StreamReader(stream);
                this.writer= new StreamWriter(stream);
                return true;
            }
            catch (SocketException ex)
            {

                return false;
            }
        }
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
        public void Escribir(string msg)
        {
            try
            {
                this.writer.WriteLine(msg);
                this.writer.Flush();
            }
            catch (Exception ex)
            {

            }
        }
        public void Desconectar()
        {
            try
            {
                this.cliente_socket.Close();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
