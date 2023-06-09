﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor_ClienteTCP.Comunication
{
    public class ServerSocket
    {
        //Declaramos los atributos de esta clase
        private int puerto;
        private Socket servidor;

        //Creamos el set del puerto
        public ServerSocket(int puerto)
        {
            this.puerto = puerto;
        }
        public bool Iniciar()
        {
            try
            {
                this.servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.servidor.Bind(new IPEndPoint(IPAddress.Any, this.puerto));
                this.servidor.Listen(10);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
        public void Cerrar()
        {
            try
            {
                this.servidor.Close();
            }
            catch (Exception ex)
            {
            }

        }
        public Socket ObtenerCliente()
        {
            return this.servidor.Accept();

        }
    }
}
