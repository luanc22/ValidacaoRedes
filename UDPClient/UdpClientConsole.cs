using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace UDPClient
{
    class UdpClientConsole
    {
        static void Main(string[] args)
        {
            int portaServidor = 15000;
            string ipServidor = "127.0.0.1";

            // Seta o endpoint para encontrar o servidor, no caso o ip local da máquina e na porta 15000
            // O ip e a porta tem que ser os mesmos configurados no servidor
            IPEndPoint endpointServidor = new IPEndPoint(IPAddress.Parse(ipServidor), portaServidor);

            // Cria o Cliente UDP permitindo a comunnicação através do protocolo
            UdpClient cliente = new UdpClient();

            // Gera o loop para manter rodando
            bool rodar = true;

            while (rodar)
            {
                // Define os valores a serem enviados no payload.
                Local local = new Local();
                bool validacao = false;
                string command = string.Empty;
                string locate = string.Empty;
                string? value = string.Empty;

                while (!validacao)
                {
                    Console.Clear();

                    Console.WriteLine("Selecione e digite um comando.");
                    mensagemComando();
                    command = Console.ReadLine().Trim().ToLower();
                    if (command == string.Empty || command != "get" && command != "set")
                    {
                        mensagemInvalida();
                        continue;
                    }
                    Console.WriteLine();

                    Console.WriteLine("Selecione e digite um local.");
                    mensagemLocal(local);
                    locate = Console.ReadLine().Trim().ToLower();
                    if (locate == string.Empty || !local.locais.Contains(locate))
                    {
                        mensagemInvalida();
                        continue;
                    }
                    Console.WriteLine();

                    if (command == "set")
                    {
                        Console.WriteLine("Selecione e digite um valor.");
                        mensagemValor();
                        value = Console.ReadLine().Trim().ToLower();
                        if (value == string.Empty || value != "on" && value != "off")
                        {
                            mensagemInvalida();
                            continue;
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        value = null;
                    }

                    validacao = true;
                    Console.Clear();
                }

                // Monta o payload a ser enviado
                string payload;

                if (command == "get")
                {
                    payload = JsonConvert.SerializeObject(new { command, locate });
                }
                else
                {
                    payload = JsonConvert.SerializeObject(new { command, locate, value });
                }

                // Converte a string payload gerada em bytes
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

                // Manda o payload para o servidor
                cliente.Send(payloadBytes, payloadBytes.Length, endpointServidor);

                // Recebe a resposta de volta do servidor
                byte[] respostaBytes = cliente.Receive(ref endpointServidor);

                // Converte a resposta em string
                string resposta = Encoding.UTF8.GetString(respostaBytes);

                // Pega o retorno do JSON e desserializa para exibir no console
                dynamic dadosResposta = JsonConvert.DeserializeObject(resposta);

                // Printa a resposta no console
                Console.WriteLine($"locate: {dadosResposta.locate}, status: {dadosResposta.status}");

                Console.WriteLine();
                Console.WriteLine("Aperte qualquer tecla para continuar");
                Console.WriteLine("Caso deseje sair, digite 0");
                ConsoleKeyInfo input = Console.ReadKey();
                if(input.KeyChar.ToString().Trim() == "0")
                {
                    rodar = false;
                }
            }
        }

        #region metodos

        private static void mensagemInvalida()
        {
            Console.WriteLine();
            Console.WriteLine("Valor inválido, tente novamente com um valor válido.");
            Console.WriteLine("Aperte ENTER para tentar novamente");
            Console.ReadKey();
        }

        private static void mensagemComando()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("GET");
            Console.WriteLine("SET");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.Write("Comando selecionado: ");

        }

        private static void mensagemLocal(Local local)
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            foreach (string l in local.locais)
            {
                Console.WriteLine(l);
            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.Write("Local selecionado: ");
        }

        private static void mensagemValor()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("ON");
            Console.WriteLine("OFF");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.Write("Valor selecionado: ");
        }

        #endregion
    }
}
