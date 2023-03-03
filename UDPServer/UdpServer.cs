using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UDPServer
{
    class UdpServer
    {
        static void Main(string[] args)
        {
            int portaServidor = 15000;

            // Cria o Cliente UDP permitindo a comunnicação através do protocolo porém passando a porta 15000 para sinalizar a criação do servidor
            UdpClient servidor = new UdpClient(portaServidor);
            montarString($"Servidor iniciado na porta {portaServidor}.");

            // Cria o endpoint com IpAddress.Any para permitir que qualquer ip mande request para o servidor, seguindo a mesma ideia setando 0 na porta
            IPEndPoint endpointCliente = new IPEndPoint(IPAddress.Any, 0);

            // Gera a lista de locais
            Local local = new Local();

            while (true)
            {
                // Recebe o request em bytes do cliente
                byte[] bytes = servidor.Receive(ref endpointCliente);

                // Converte o request em string
                string request = Encoding.UTF8.GetString(bytes);
                montarString($"Request vindo de {endpointCliente}: {request}");

                // Monta o Json com o payload de resposta
                JObject json = JObject.Parse(request);
                string locate = (string)json["locate"];
                string command = (string)json["command"];
                string value = string.Empty;
                string payload = string.Empty;

                switch (command)
                {
                    case "get":
                        payload = PegarEstado(locate, local);
                        break;
                    case "set":
                        value = (string)json["value"];
                        payload = AlterarEstado(locate, value, local);
                        break;
                }

                // Converte a string payload gerada em bytes
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

                // Manda o payload para o cliente
                servidor.Send(payloadBytes, payloadBytes.Length, endpointCliente);

                // Printa a confirmação de envio no console
                Console.WriteLine($"Resposta enviada para {endpointCliente}: {payload}");
            }
        }

        #region metodos

        static void montarString(string texto)
        {
            Console.WriteLine();
            Console.WriteLine(texto);
            Console.WriteLine();
        }

        static string PegarEstado(string locate, Local local)
        {
            string status = string.Empty;

            if (local.PegarEstado(locate))
            {
                status = "on";
                return JsonConvert.SerializeObject(new { locate, status });
            }
            else
            {
                status = "off";
                return JsonConvert.SerializeObject(new { locate, status });
            }
        }

        static string AlterarEstado(string locate, string status, Local local)
        {
            local.AlterarEstado(locate, status);
            return JsonConvert.SerializeObject(new { locate, status });
        }

        #endregion
    }
}
