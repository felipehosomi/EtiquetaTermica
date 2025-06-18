using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.Core.EtiquetaTermica.BLL
{
    public class UserFieldsBLL
    {

        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
                        
            userObjectController.CreateUserTable("CVA_ETIQUETA", "[CVA] Cadastro de Etiquetas ", BoUTBTableType.bott_MasterData);

            userObjectController.InsertUserField("@CVA_ETIQUETA", "descricao", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None,254);
            userObjectController.InsertUserField("@CVA_ETIQUETA", "linguagem", "Linguagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_ETIQUETA", "script", "Script de Impressão", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);            
            userObjectController.InsertUserField("@CVA_ETIQUETA", "procedure", "Stored Procedure", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_ETIQUETA", "local", "Local Saída", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_ETIQUETA", "procpesq", "Proc Pesquisa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 250);
            userObjectController.InsertUserField("@CVA_ETIQUETA", "caminhoscript", "Caminho do Script", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 250);

            
            userObjectController.CreateUserObject("CVA_ETIQUETA", "[CVA] Cadastro Etiqueta", "@CVA_ETIQUETA", BoUDOObjType.boud_MasterData);

            userObjectController.InsertUserField("ITM1", "CVA_Validade", "Validade", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);

            userObjectController.InsertUserField("OINV", "SO_EtiqPadrao1", "Etiqueta Padrão 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("OINV", "SO_EtiqPadrao2", "Etiqueta Padrão 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);            

            userObjectController.InsertUserField("OINV", "CVA_EtiquetaImpressa", "Etiqueta Impressa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("OINV", "CVA_EtiquetaImpressa", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OINV", "CVA_EtiquetaImpressa", "Y", "Sim");

            userObjectController.InsertUserField("OWOR", "SO_EtiqPadrao1", "Etiqueta Padrão 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("OWOR", "SO_EtiqPadrao2", "Etiqueta Padrão 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);

            userObjectController.CreateUserTable("SO_ETIQ_ITEM", "SO|Etiqueta - Item", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.InsertUserField("@SO_ETIQ_ITEM", "ItemCode", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@SO_ETIQ_ITEM", "CodEtiq", "Cód. Etiqueta", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@SO_ETIQ_ITEM", "Padrao", "Etiqueta Padrão", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@SO_ETIQ_ITEM", "Valida", "Etiqueta Válida", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("SO_ETIQ_PN", "SO|Etiqueta - PN", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.InsertUserField("@SO_ETIQ_PN", "CardCode", "Cód. PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@SO_ETIQ_PN", "CodEtiq", "Cód. Etiqueta", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@SO_ETIQ_PN", "Padrao", "Etiqueta Padrão", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
        }
    }
}