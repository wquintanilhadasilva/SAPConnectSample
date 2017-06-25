using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;

namespace SAPConnectSample
{
    /// <summary>
    /// Implementa as configurações para conexão com o servidor SAP.
    /// Classe Singleton
    /// </summary>
    public sealed class SAPConnect
    {
        private static SAPConnect instance;
        public enum DESTINATION_DEF {APP_CONFIG, DATABASE};

        private SAPConnect() { }

        /// <summary>
        /// Devolve uma instância única singleton para essa classe
        /// </summary>
        /// <returns></returns>
        public static SAPConnect getInstance()
        {
            Object o = new Object();

            lock(o){
                if (instance == null)
                {
                    instance = new SAPConnect();
                }
            }
            return instance;
        }

        /// <summary>
        /// Cria uma function encapsulando um programa ABAP
        /// </summary>
        /// <param name="nameFunction">Nome do Programa ABAP a ser utilizado</param>
        /// <param name="dest">Identificação do Servidor SAP onde está o programa desejado</param>
        /// <returns>WRAPER para o programa ABAP</returns>
        public IRfcFunction getFunction(String nameFunction, RfcDestination dest)
        {
            RfcRepository repo = dest.Repository;
            IRfcFunction fReadTable = repo.CreateFunction(nameFunction);
            return fReadTable;
        }

        /// <summary>
        /// Retorna uma Destination conforme configuração e nome informado
        /// </summary>
        /// <param name="destinationName"></param>
        /// <param name="localDestination"></param>
        /// <returns></returns>
        public RfcDestination getDestination(String destinationName, DESTINATION_DEF localDestination)
        {
            //se for app.config, obtém logo do arquivo
            if (localDestination == DESTINATION_DEF.APP_CONFIG)
            {
                var destnation = RfcDestinationManager.GetDestination(destinationName);
                return destnation;
            }
            else
            {
                //Do contrário, carrega dinamicamente a destination para ser usada
                return RfcDestinationManager.GetDestination(this.loadConfigDynamicDestination(destinationName));
            }
        }
        
        private RfcConfigParameters loadConfigDynamicDestination(String name)
        {
            RfcConfigParameters conf = new RfcConfigParameters();
            //Dados fictícios, o argumento name pode ser uma chave para identificar uma configuração no meio de várias...
            conf.Add(RfcConfigParameters.AppServerHost, "server");
            conf.Add(RfcConfigParameters.SystemNumber, "01");
            conf.Add(RfcConfigParameters.SystemID, "mar");
            conf.Add(RfcConfigParameters.User, "joseph_climber");
            conf.Add(RfcConfigParameters.Password, "secret");
            conf.Add(RfcConfigParameters.Client, "1");
            conf.Add(RfcConfigParameters.Language, "PT");
            conf.Add(RfcConfigParameters.PoolSize, "5");
            conf.Add(RfcConfigParameters.PeakConnectionsLimit, "10");
            conf.Add(RfcConfigParameters.ConnectionIdleTimeout, "600");

            return conf;
        }
    }   
     
}
