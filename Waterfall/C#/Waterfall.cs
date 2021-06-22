using System;
using System.Collections.Generic;
using System.Text;
using ConnectTo;
using PesquisarWS;
using System.Xml;
using System.Text.RegularExpressions;

namespace Waterfall
{
    public class Enviar
    {
        Comando Mirror = new Comando();

        public void Enviar()
        {
            string strSql;
            Mirror.Banco = "NEO";

            strSql = "";
            Mirror.ExecuteQuery(strSql);

        }
        public void PesquisarFone(String strCPF, String strUF, String strTipo)
        {
            String strFones;
            XmlDocument xmlFinder = new XmlDocument();
            Pesquisa WSPesquisa = new Pesquisa();
            String strReturn;
            short z;
            
            try
            {
                WSPesquisa.BuscarAFinder = true;
                WSPesquisa.BuscarAFinder = true;
                WSPesquisa.UserFinder = "credicard1";
                WSPesquisa.PassFinder = "caneta";
                WSPesquisa.BuscaCPF = true;
                WSPesquisa.CPF = strCPF;
                WSPesquisa.Registros = 5;
                WSPesquisa.Bases = strTipo;

                strReturn = WSPesquisa.Localizar("XML");
                xmlFinder.LoadXml(strReturn);

                if (xmlFinder.InnerXml.ToString == "<ROOT xmlns=></ROOT>"){return "";}
                strFones = "";
                for (z = 0; z <= xmlFinder.DocumentElement.ChildNodes.Count - 1; z++)
                {
                    if (z > 0){strFones = strFones & ",";}
                    strFones = strFones & Trim(xmlFinder.DocumentElement.ChildNodes(z).Attributes.GetNamedItem("DDD").Value.ToString) & Trim(xmlFinder.DocumentElement.ChildNodes(z).Attributes.GetNamedItem("FONE").Value.ToString)
                }
                return strFones.Trim;
            }
            finally
            {
                return "";
            }
        }
    }
}
