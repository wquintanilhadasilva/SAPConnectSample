using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;

namespace SAPConnectSample
{
    /// <summary>
    /// Implementa os exemplos de conexão com o SAP
    /// </summary>
    class SAPConnectSample
    {

        private static string DEST_NAME = "RW_SAPTEST"; //Definida no App.config

        public void processe()
        {

            //Isolei os métodos de acesso ao SAP na classe SAPConnect. Adotei o Singleton Pattern
            SAPConnect conn = SAPConnect.getInstance();

            //Cria um Destination (destino ou servidor). Neste caso implementei duas possibilidades: 1 - Obter do app.config ou de outro local (BD por exemplo - não implementei)
            RfcDestination dest = conn.getDestination(DEST_NAME, SAPConnect.DESTINATION_DEF.APP_CONFIG);
            
            //Configura o nome do programa ABAP que será chamado, no destino obtido no passo anterior
            IRfcFunction func = conn.getFunction("ZCO_FROTA", dest);

            /*
             * Neste exemplo, a Função SAP recebe uma tabela contendo as placas de veículos e a função ABAP calcula,
             * para cada veículo, o valor da despesa e o combustível usado no período de referência informado.
             * */

            //A Função ABAP define um parâmetro do tipo tabela que recebe várias frotas como filtro.
            //Essa tabela tem uma estrutura definida lá no ABAP
            //Neste exemplo, a função ABAP define 3 parêmetros, sendo duas datas (Início e Fim)
            //e uma tabela que pode conter várias linhas de parâmetros
            IRfcTable filtroFrotas = func.GetTable("FROTA");
            
            //Adiciona uma linha para informar os parâmetros esperados pela estrutura -> há várias formas de fazer isso...
            filtroFrotas.Append(); 

            //Informa os valores dos parametros da linha atual da tabela de parâmetros
            filtroFrotas.SetValue("AUFNR", "000000000357");
            filtroFrotas.SetValue("KMROD", "00000000");
            filtroFrotas.SetValue("PLACA", "357 - KBV-5953");

            //Adiciona nova linha e configura os parâmetros dessa nova linha
            filtroFrotas.Append();
            filtroFrotas.SetValue("AUFNR", "000000000100");
            filtroFrotas.SetValue("KMROD", "00002131");
            filtroFrotas.SetValue("PLACA", "100 - KFB-2220");

            //Adiciona nova linha e configura os parâmetros dessa nova linha
            filtroFrotas.Append();
            filtroFrotas.SetValue("AUFNR", "000000010007");
            filtroFrotas.SetValue("KMROD", "00000722");
            filtroFrotas.SetValue("PLACA", "10007 - MWE-8625");

            //Informa os valores dos parâmetros globais da função ABAP (fora da tabela)
            func.SetValue("DATA_FIM", "20130731");
            func.SetValue("DATA_INI", "20130701");
            
            //Executa a funçao no SAP
            func.Invoke(dest);
            
            //Para ler o retorno....caso necessário....
            IRfcTable data = func.GetTable("FROTA");  //Obtém a tabela de retorno - o nome é definido na função ABAP

            StringBuilder sb = new StringBuilder();

            //Percorre cada linha da tabela retornada
            foreach (IRfcStructure linha in data)
            {
                //Percorre as linhas retornadas obtendo os valores das colunas que eu preciso
                sb.Clear();
                sb.Append("Placa: ");
                sb.Append(linha.GetValue("PLACA")); //Ler o valor do campo da linha atual
                sb.Append(" - Combustível: ");
                sb.Append(linha.GetValue("COMBS")); //Ler o valor do campo da linha atual
                sb.Append(" - Despesa: ");
                sb.Append(linha.GetValue("DESPE")); //Ler o valor do campo da linha atual

                Console.WriteLine(sb.ToString());
            }
          
        }

    }
}
