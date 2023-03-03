using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPServer
{
    public class Local
    {
        // Inicia as variáveis de locais em um dicionário para fazer a relação entre nome do local e o valor, todas são startadas como desligadas (false)
        Dictionary<string, bool> locais = new Dictionary<string, bool>
        {
            { "luz_guarita", false },
            { "ar_guarita", false },
            { "luz_estacionamento", false },
            { "luz_galpao_externo", false },
            { "luz_galpao_interno", false },
            { "luz_escritorios", false },
            { "ar_escritorios", false },
            { "luz_sala_reunioes", false },
            { "ar_sala_reunioes", false }
        };

        // Método chamado para pegar o estado de um local
        public bool PegarEstado(string local) 
        {
            return locais[local];
        }

        // Método chamado para alterar o estado de um local
        public void AlterarEstado(string local, string status)
        {
            if (locais.ContainsKey(local))
            {
                if (status.ToLower() == "off")
                {
                    locais[local] = false;
                }
                else if (status.ToLower() == "on")
                {
                    locais[local] = true;
                }  
            }
        }
    }
}
