using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using log4net.Core;
using SAPbouiCOM;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
//using NMS.Tools;
using CVA.Core.EtiquetaTermica.Model;
using CVA.AddOn.Common.Util;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace CVA.Core.EtiquetaTermica.View
{
    public class f2000001001 : BaseForm
    {
        private SAPbobsCOM.Company company;
        private SAPbouiCOM.Application application;
        //private EtiquetaTermica.Services.TagServices etiquetaService;
        public ILogger Log { get; set; }

        private static bool x = true;
        private static bool y = true;
        private ComboBox cbLayout;
        private EditText txDocIni;
        private EditText txDocFim;
        private EditText txDtIni;
        private EditText txDtFim;
        private EditText txTermo;
        private EditText txCopias;
        private Button btImprimir;
        private Button btPesquisar;
        private Button btCheckAll;
        private Button btUnCheckAll;

        private Grid oGrid;
        private Form oForm;
        private UserDataSources oUDS;

        //campos livres

        private EditText txcl1;
        private EditText txcl2;
        private EditText txcl3;
        private EditText txcl4;
        private EditText txcl5;
        private EditText txcl6;
        private EditText txcl7;
        private EditText txcl8;


        #region Constructor
        public f2000001001()
        {
            FormCount++;

        }

        public f2000001001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;

        }
        #endregion

        public override bool ItemEvent()
        {
            if (x)
            {
                x = false;

                if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
                {
                    oForm = SBOApp.Application.Forms.ActiveForm;
                    oUDS = oForm.DataSources.UserDataSources;

                    cbLayout = (ComboBox)oForm.Items.Item("layout").Specific;
                    txDocIni = (EditText)oForm.Items.Item("docini").Specific;
                    txDocFim = (EditText)oForm.Items.Item("docfim").Specific;
                    txDtIni = (EditText)oForm.Items.Item("dtini").Specific;
                    txDtFim = (EditText)oForm.Items.Item("dtfim").Specific;
                    txTermo = (EditText)oForm.Items.Item("termo").Specific;
                    txCopias = (EditText)oForm.Items.Item("copias").Specific;
                    btImprimir = (Button)oForm.Items.Item("imprimir").Specific;
                    btPesquisar = (Button)oForm.Items.Item("pesquisar").Specific;
                    btCheckAll = (Button)oForm.Items.Item("btn_sel").Specific;
                    btUnCheckAll = (Button)oForm.Items.Item("btn_des").Specific;
                    

                    oGrid = (Grid)oForm.Items.Item("grid").Specific;


                    // campos livres
                    txcl1 = (EditText)oForm.Items.Item("txcl1").Specific;
                    txcl2 = (EditText)oForm.Items.Item("txcl2").Specific;
                    txcl3 = (EditText)oForm.Items.Item("txcl3").Specific;
                    txcl4 = (EditText)oForm.Items.Item("txcl4").Specific;
                    txcl5 = (EditText)oForm.Items.Item("txcl5").Specific;
                    txcl6 = (EditText)oForm.Items.Item("txcl6").Specific;
                    txcl7 = (EditText)oForm.Items.Item("txcl7").Specific;
                    txcl8 = (EditText)oForm.Items.Item("txcl8").Specific;


                    if (ItemEventInfo.ItemUID == "layout")
                    {
                        y = false;
                        if (SBOApp.Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                        {
                            var t = cbLayout.ValidValues.Count;
                            if (t <= 0)
                            {
                                var b = new BLL.TagServices();
                                if (t > 0)
                                {
                                    foreach (var item in b.GetTagListHANA())
                                    {
                                        cbLayout.ValidValues.Remove(item.Code, BoSearchKey.psk_Index);
                                    }

                                }


                                foreach (var item in b.GetTagListHANA())
                                {
                                    cbLayout.ValidValues.Add(item.Code, item.Name);
                                }

                                oForm.DataSources.UserDataSources.Add("U_docini", BoDataType.dt_SHORT_TEXT, 72);
                                SAPbouiCOM.EditText oEditDocIni = (SAPbouiCOM.EditText)Form.Items.Item("docini").Specific;
                                oEditDocIni.DataBind.SetBound(true, "", "U_docini");

                                oForm.DataSources.UserDataSources.Add("U_docfim", BoDataType.dt_SHORT_TEXT, 72);
                                SAPbouiCOM.EditText oEditDocFim = (SAPbouiCOM.EditText)Form.Items.Item("docfim").Specific;
                                oEditDocFim.DataBind.SetBound(true, "", "U_docfim");

                                oForm.DataSources.UserDataSources.Add("U_dtini", BoDataType.dt_DATE);
                                SAPbouiCOM.EditText oEditData = (SAPbouiCOM.EditText)Form.Items.Item("dtini").Specific;
                                oEditData.DataBind.SetBound(true, "", "U_dtini");

                                oForm.DataSources.UserDataSources.Add("U_dtfim", BoDataType.dt_DATE);
                                oEditData = (SAPbouiCOM.EditText)Form.Items.Item("dtfim").Specific;
                                oEditData.DataBind.SetBound(true, "", "U_dtfim");

                                oForm.DataSources.UserDataSources.Add("U_termo", BoDataType.dt_SHORT_TEXT, 250);
                                SAPbouiCOM.EditText oEditTermo = (SAPbouiCOM.EditText)Form.Items.Item("termo").Specific;
                                oEditTermo.DataBind.SetBound(true, "", "U_termo");

                                oForm.DataSources.UserDataSources.Add("U_copias", BoDataType.dt_SHORT_NUMBER, 250);
                                SAPbouiCOM.EditText oEditCopias = (SAPbouiCOM.EditText)Form.Items.Item("copias").Specific;
                                oEditCopias.DataBind.SetBound(true, "", "U_copias");


                                txCopias.Value = "1";

                                //txDocIni.AddUserDataSource(oForm, "docini", BoDataType.dt_SHORT_TEXT, 72);
                                //txDocFim.AddUserDataSource(oForm, "docfim", BoDataType.dt_SHORT_TEXT, 72);
                                //txDtIni.AddUserDataSource(oForm, "dtini", BoDataType.dt_DATE);
                                //txDtFim.AddUserDataSource(oForm, "dtfim", BoDataType.dt_DATE);
                                //txTermo.AddUserDataSource(oForm, "termo", BoDataType.dt_SHORT_TEXT, 250);
                                //txCopias.AddUserDataSource(oForm, "copias", BoDataType.dt_SHORT_NUMBER, 250);


                                // campos livres
                                //txcl1.AddUserDataSource(oForm, "txcl1", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl2.AddUserDataSource(oForm, "txcl2", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl3.AddUserDataSource(oForm, "txcl3", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl4.AddUserDataSource(oForm, "txcl4", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl5.AddUserDataSource(oForm, "txcl5", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl6.AddUserDataSource(oForm, "txcl6", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl7.AddUserDataSource(oForm, "txcl7", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl8.AddUserDataSource(oForm, "txcl8", BoDataType.dt_SHORT_TEXT, 100);

                                oForm.DataSources.UserDataSources.Add("txcl1", BoDataType.dt_SHORT_TEXT, 250);
                                SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl1").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl1");

                                oForm.DataSources.UserDataSources.Add("txcl2", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl2").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl2");

                                oForm.DataSources.UserDataSources.Add("txcl3", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl3").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl3");

                                oForm.DataSources.UserDataSources.Add("txcl4", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl4").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl4");

                                oForm.DataSources.UserDataSources.Add("txcl5", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl5").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl5");

                                oForm.DataSources.UserDataSources.Add("txcl6", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl6").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl6");

                                oForm.DataSources.UserDataSources.Add("txcl7", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl7").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl7");

                                oForm.DataSources.UserDataSources.Add("txcl8", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl8").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl8");



                            }
                        }
                        else
                        {
                            var b = new BLL.TagServices();

                            if (cbLayout.ValidValues.Count <= 0)
                            {
                                foreach (var item in b.GetTagListSQL())
                                {
                                    cbLayout.ValidValues.Add(item.Code, item.Name);
                                }

                                oForm.DataSources.UserDataSources.Add("U_docini", BoDataType.dt_SHORT_TEXT, 72);
                                SAPbouiCOM.EditText oEditDocIni = (SAPbouiCOM.EditText)Form.Items.Item("docini").Specific;
                                oEditDocIni.DataBind.SetBound(true, "", "U_docini");

                                oForm.DataSources.UserDataSources.Add("U_docfim", BoDataType.dt_SHORT_TEXT, 72);
                                SAPbouiCOM.EditText oEditDocFim = (SAPbouiCOM.EditText)Form.Items.Item("docfim").Specific;
                                oEditDocFim.DataBind.SetBound(true, "", "U_docfim");

                                oForm.DataSources.UserDataSources.Add("U_dtini", BoDataType.dt_DATE);
                                SAPbouiCOM.EditText oEditData = (SAPbouiCOM.EditText)Form.Items.Item("dtini").Specific;
                                oEditData.DataBind.SetBound(true, "", "U_dtini");

                                oForm.DataSources.UserDataSources.Add("U_dtfim", BoDataType.dt_DATE);
                                oEditData = (SAPbouiCOM.EditText)Form.Items.Item("dtfim").Specific;
                                oEditData.DataBind.SetBound(true, "", "U_dtfim");

                                oForm.DataSources.UserDataSources.Add("U_termo", BoDataType.dt_SHORT_TEXT, 250);
                                SAPbouiCOM.EditText oEditTermo = (SAPbouiCOM.EditText)Form.Items.Item("termo").Specific;
                                oEditTermo.DataBind.SetBound(true, "", "U_termo");

                                oForm.DataSources.UserDataSources.Add("U_copias", BoDataType.dt_SHORT_NUMBER, 250);
                                SAPbouiCOM.EditText oEditCopias = (SAPbouiCOM.EditText)Form.Items.Item("copias").Specific;
                                oEditCopias.DataBind.SetBound(true, "", "U_copias");

                                txCopias.Value = "1";

                                //txDocIni.AddUserDataSource(oForm, "docini", BoDataType.dt_SHORT_TEXT, 72);
                                //txDocFim.AddUserDataSource(oForm, "docfim", BoDataType.dt_SHORT_TEXT, 72);
                                //txDtIni.AddUserDataSource(oForm, "dtini", BoDataType.dt_DATE);
                                //txDtFim.AddUserDataSource(oForm, "dtfim", BoDataType.dt_DATE);
                                //txTermo.AddUserDataSource(oForm, "termo", BoDataType.dt_SHORT_TEXT, 250);
                                //txCopias.AddUserDataSource(oForm, "copias", BoDataType.dt_SHORT_NUMBER, 250);


                                // campos livres
                                //txcl1.AddUserDataSource(oForm, "txcl1", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl2.AddUserDataSource(oForm, "txcl2", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl3.AddUserDataSource(oForm, "txcl3", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl4.AddUserDataSource(oForm, "txcl4", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl5.AddUserDataSource(oForm, "txcl5", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl6.AddUserDataSource(oForm, "txcl6", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl7.AddUserDataSource(oForm, "txcl7", BoDataType.dt_SHORT_TEXT, 100);
                                //txcl8.AddUserDataSource(oForm, "txcl8", BoDataType.dt_SHORT_TEXT, 100);

                                oForm.DataSources.UserDataSources.Add("txcl1", BoDataType.dt_SHORT_TEXT, 250);
                                SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl1").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl1");

                                oForm.DataSources.UserDataSources.Add("txcl2", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl2").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl2");

                                oForm.DataSources.UserDataSources.Add("txcl3", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl3").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl3");

                                oForm.DataSources.UserDataSources.Add("txcl4", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl4").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl4");

                                oForm.DataSources.UserDataSources.Add("txcl5", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl5").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl5");

                                oForm.DataSources.UserDataSources.Add("txcl6", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl6").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl6");

                                oForm.DataSources.UserDataSources.Add("txcl7", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl7").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl7");

                                oForm.DataSources.UserDataSources.Add("txcl8", BoDataType.dt_SHORT_TEXT, 250);
                                oEdit = (SAPbouiCOM.EditText)Form.Items.Item("txcl8").Specific;
                                oEdit.DataBind.SetBound(true, "", "txcl8");
                            }


                        }
                    }
                    btImprimir.ClickAfter += BtImprimir_ClickAfter;
                    btPesquisar.ClickAfter += BtPesquisar_ClickAfter;
                    btCheckAll.ClickAfter += BtCheckAll_ClickAfter;
                    btUnCheckAll.ClickAfter += BtUnCheckAll_ClickAfter;

                }
            }
            x = true;
            return true;
        }

        private void BtUnCheckAll_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            UnCheckAll();
        }

        private void BtCheckAll_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            CheckAll();
        }

        private void BtPesquisar_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            AtualizaPesquisa();
        }

        private void BtImprimir_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (x)
            {
                x = true;

                var lista = new List<TagParameterModel>();
                var msg = "";

                //campos livres
                var cl1 = ((EditText)oForm.Items.Item("txcl1").Specific).Value;
                var cl2 = ((EditText)oForm.Items.Item("txcl2").Specific).Value;
                var cl8 = ((EditText)oForm.Items.Item("txcl8").Specific).Value;
                var cl3 = ((EditText)oForm.Items.Item("txcl3").Specific).Value;
                var cl4 = ((EditText)oForm.Items.Item("txcl4").Specific).Value;
                var cl5 = ((EditText)oForm.Items.Item("txcl5").Specific).Value;
                var cl6 = ((EditText)oForm.Items.Item("txcl6").Specific).Value;
                var cl7 = ((EditText)oForm.Items.Item("txcl7").Specific).Value;
                var DocNum = string.Empty;

                try
                {
                    oForm.Freeze(true);

                    var layout = ((ComboBox)oForm.Items.Item("layout").Specific).Selected.Value;

                    if (string.IsNullOrEmpty(layout))
                    {
                        msg = "Por favor, selecione qual o layout de etiquetas deseja utilizar primeiro.";
                        application.MessageBox(msg);
                        return;
                    }

                    for (int i = 0; i < oGrid.Rows.Count; i++)
                    {

                        if (oGrid.DataTable.GetValue(0, i).ToString() == "Y")
                        {


                            var tagParams = new TagParameterModel();

                            tagParams.ObjType = oGrid.DataTable.GetValue("ObjType", i).ToString();
                            tagParams.Key = oGrid.DataTable.GetValue("Key", i).ToString();
                            tagParams.cl1 = cl1;
                            tagParams.cl2 = cl2;
                            tagParams.cl3 = cl3;
                            tagParams.cl4 = cl4;
                            tagParams.cl5 = cl5;
                            tagParams.cl6 = cl6;
                            tagParams.cl7 = cl7;
                            tagParams.cl8 = cl8;     
                            lista.Add(tagParams);

                            DocNum = oGrid.DataTable.GetValue("Key", i).ToString();
                        }
                    }

                    if (lista.Count == 0)
                    {
                        msg = "Nenhum registro foi selecionado.";
                        application.MessageBox(msg);

                        return;
                    }

                    var quantidade = Convert.ToInt32(txCopias.Value);

                    foreach (var item in lista)
                    {

                        ProcessaImpressaoEtiqueta(layout, item, quantidade);                        
                    }
                }
                catch (Exception ex)
                {
                    SBOApp.Application.SetStatusBarMessage(ex.Message);
                    return;
                }
                finally
                {
                    oForm.Freeze(false);
                }
                x = false;
            }


        }

        private void AtualizaPesquisa()
        {
            var layout = "";
            string docini = "";
            string docfim = "";
            string dtini;
            string dtfim;
            string termo = "";
            var msg = "";

            if (SBOApp.Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                var form = SBOApp.Application.Forms.ActiveForm;
                if (x)
                {
                    try
                    {
                        form.Freeze(true);

                        layout = ((ComboBox)form.Items.Item("layout").Specific).Selected.Value;

                        if (string.IsNullOrEmpty(layout))
                        {
                            msg = "Por favor, selecione qual o layout de etiquetas deseja utilizar primeiro.";
                            application.MessageBox(msg);
                            return;
                        }

                        docini = string.IsNullOrEmpty(((EditText)oForm.Items.Item("docini").Specific).Value.Replace("'", "''")) ? "NULL" : "'" + ((EditText)oForm.Items.Item("docini").Specific).Value.Replace("'", "''") + "'";


                        docfim = string.IsNullOrEmpty(((EditText)oForm.Items.Item("docfim").Specific).Value.Replace("'", "''")) ? "NULL" : "'" + ((EditText)oForm.Items.Item("docfim").Specific).Value.Replace("'", "''") + "'";

                        dtini = (string.IsNullOrEmpty(txDtIni.String) ? "NULL" : "'" + DateTime.Parse(txDtIni.String).ToString("yyyy-MM-dd") +"'");
                        dtfim = (string.IsNullOrEmpty(txDtFim.String) ? "NULL" : "'" + DateTime.Parse(txDtFim.String).ToString("yyyy-MM-dd") + "'");
                        //dtini = (string.IsNullOrEmpty(txDtIni.String) ? "NULL" : "'" + DateTime.ParseExact(oUDS.Item("dtini").ValueEx, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'");
                        //dtfim = (string.IsNullOrEmpty(txDtFim.String) ? "NULL" : "'" + DateTime.ParseExact(oUDS.Item("dtfim").ValueEx, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'");

                        termo = string.IsNullOrEmpty(((EditText)oForm.Items.Item("termo").Specific).Value.Replace("'", "''")) ? "NULL" : "'" + ((EditText)oForm.Items.Item("termo").Specific).Value.Replace("'", "''") + "'";

                        var tag = new BLL.TagServices();

                        DataTable dt;
                        //var sql = string.Format("CALL" + tag.GetTagHANA(layout).U_procpesq.Replace("'", "") + " {0}, {1}, {2}, {3}, {4}", docini, docfim, dtini, dtfim, termo);

                        SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordSet.DoQuery(string.Format("select \"U_procpesq\" from \"@CVA_ETIQUETA\" where \"Code\" = {0}", layout));
                        var proc = "";
                        var p = "";
                        if (!oRecordSet.EoF)
                        {
                            proc = oRecordSet.Fields.Item(0).Value;



                            oRecordSet.MoveFirst();
                        }

                        try
                        {
                            dt = oForm.DataSources.DataTables.Item("dt");
                        }
                        catch
                        {
                            dt = oForm.DataSources.DataTables.Add("dt");
                        }

                    
                        var sql = string.Format("CALL " + "{0}" + " ({1},{2},{3},{4},{5},'{6}')", proc.ToUpper(), docini, docfim, dtini, dtfim, termo,layout);
                        dt.ExecuteQuery(sql);
                        oGrid.DataTable = dt;
                        oGrid.Columns.Item("Sel").Type = BoGridColumnType.gct_CheckBox;


                        for (int i = 1; i <= oGrid.Columns.Count - 1; i++)
                        {
                            oGrid.Columns.Item(i).Editable = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.MessageBox("Erro ao efetuar a pesquisa.. \n Possivel causa (Parametros da consulta Invalidos)\n");
                        x = false;
                        //SBOApp.Application.SetStatusBarMessage(ex.Message);
                        return;
                    }
                    finally
                    {
                        oForm.Freeze(false);
                        x = false;
                    }
                }
            }

            else
            {
                if (x)
                {
                    try
                    {

                        oForm.Freeze(true);
                        var form = SBOApp.Application.Forms.ActiveForm;

                        layout = ((ComboBox)form.Items.Item("layout").Specific).Selected.Value;

                        if (string.IsNullOrEmpty(layout))
                        {
                            msg = "Por favor, selecione qual o layout de etiquetas deseja utilizar primeiro.";
                            application.MessageBox(msg);
                            return;
                        }

                        docini = string.IsNullOrEmpty(((EditText)oForm.Items.Item("docini").Specific).Value.Replace("'", "''")) ? "NULL" : ((EditText)oForm.Items.Item("docini").Specific).Value.Replace("'", "''");


                        docfim = string.IsNullOrEmpty(((EditText)oForm.Items.Item("docfim").Specific).Value.Replace("'", "''")) ? "NULL" : ((EditText)oForm.Items.Item("docfim").Specific).Value.Replace("'", "''");

                        dtini = (string.IsNullOrEmpty(txDtIni.String) ? "NULL" : "'" + DateTime.ParseExact(oUDS.Item("dtini").ValueEx, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'");
                        dtfim = (string.IsNullOrEmpty(txDtFim.String) ? "NULL" : "'" + DateTime.ParseExact(oUDS.Item("dtfim").ValueEx, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'");

                        termo = string.IsNullOrEmpty(((EditText)oForm.Items.Item("termo").Specific).Value.Replace("'", "''")) ? "NULL" : "'" + ((EditText)oForm.Items.Item("termo").Specific).Value.Replace("'", "''") + "'";

                        var tag = new BLL.TagServices();

                        DataTable dt;

                        SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordSet.DoQuery(string.Format("select U_procpesq from [@CVA_ETIQUETA] where Code = {0}", layout));
                        var proc = "";
                        var p = "";
                        if (!oRecordSet.EoF)
                        {
                            proc = oRecordSet.Fields.Item(0).Value;



                            oRecordSet.MoveFirst();
                        }

                        try
                        {
                            dt = oForm.DataSources.DataTables.Item("dt");
                        }
                        catch
                        {
                            dt = oForm.DataSources.DataTables.Add("dt");
                        }

                        var sql = string.Format("CALL " + "{0}" + " ({1},{2},{3},{4},{5})", proc, docini, docfim, dtini, dtfim, termo);
                        dt.ExecuteQuery(sql);
                        oGrid.DataTable = dt;
                        oGrid.Columns.Item("Sel").Type = BoGridColumnType.gct_CheckBox;
                        //var sql = string.Format("exec " + tag.GetTagSQL(layout).U_procpesq.Replace("'", "") + " {0}, {1}, {2}, {3}, {4}", docini, docfim, dtini, dtfim, termo);

                        //try
                        //{
                        //    dt = oForm.DataSources.DataTables.Item("dt");
                        //}
                        //catch
                        //{
                        //    dt = oForm.DataSources.DataTables.Add("dt");
                        //}
                        //dt.ExecuteQuery(sql);
                        //oGrid.DataTable = dt;
                        //oGrid.Columns.Item("Sel").Type = BoGridColumnType.gct_CheckBox;

                        for (int i = 1; i <= oGrid.Columns.Count - 1; i++)
                        {
                            oGrid.Columns.Item(i).Editable = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.MessageBox("Erro ao efetuar a pesquisa.. \n Possivel causa (Parametros da consulta Invalidos)\n");
                        x = false;
                        //SBOApp.Application.SetStatusBarMessage(ex.Message);
                        return;
                    }
                    finally
                    {
                        oForm.Freeze(false);
                        x = false;
                    }


                }

            }
        }

        private void ProcessaImpressaoEtiqueta(string layout, Model.TagParameterModel tagParams, int quantidade = 1)
        {
            if (x)
            {
                x = false;
                if (SBOApp.Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                {
                    try
                    {


                        var tag = new BLL.TagServices();

                        var tags = tag.GetTagScriptHANA(tag.GetTagHANA(layout), tagParams, quantidade);

                        var etiqueta = tag.GetTagHANA(layout);
                        var local =  etiqueta.U_local;
                        if (local.Substring(local.Length - 1) != "\\")
                        {
                            local = local.Trim() + "\\";
                        }

                        var c = 1;
                        foreach (var item in tags)
                        {
                            

                            var num = ("000000" + item.Id);
                            num = num.Substring(num.Length - 6);

                            var file = local + "TAG." + num + "_" + tagParams.Key+  "." + Guid.NewGuid() + ".prn"; //  /*+ tagParams.ObjType.Replace("|", "") + "." + tagParams.Key + "."*/ + Guid.NewGuid() +"_"+ c + ".prn";

                            try
                            {
                                if (!string.IsNullOrEmpty(etiqueta.U_caminhoscript))
                                {
                                    var ScriptOriginal = System.IO.File.ReadAllText(etiqueta.U_caminhoscript, Encoding.GetEncoding("ISO-8859-1"));

                                    //int count = Regex.Matches(ScriptOriginal, "<#").Count;
                                    var quantidadevariaveis = Regex.Matches(ScriptOriginal, "<#").Count;

                                    List<(int, int, int, string)> variaveis = new List<(int, int, int, string)>();
                                    List<(int, int, int, string)> variaveisOriginais = new List<(int, int, int, string)>();

                                    var indexes = 0;
                                    var total = item.Tag.Count();

                                    for (var i = 0; i < quantidadevariaveis; i++)
                                    {
                                       
                                        var index = item.Tag.IndexOf("<#", indexes + 2);
                                        var indexfim = item.Tag.IndexOf("#>", index + 2);

                                        indexes = indexfim;

                                        var tamanho = indexfim - index;

                                        var valor = item.Tag.Substring(index+ 2, tamanho - 2);
                                        variaveis.Add((i, index, indexfim, valor));
                                    }

                                    var indexes2 = 0;

                                    for (var i = 0; i < quantidadevariaveis; i++)
                                    {
                                        var index = ScriptOriginal.IndexOf("<#", indexes2 + 2);
                                        var indexfim = ScriptOriginal.IndexOf("#>", index + 2);

                                        indexes2 = indexfim;

                                        var aStringBuilder = new StringBuilder(ScriptOriginal);
                                        var tamanho = indexfim - index;

                                        //var valor = ScriptOriginal.Substring(index + 1, tamanho - 1);
                                        //variaveisOriginais.Add((i, index, indexfim, valor));


                                        aStringBuilder.Remove(index, tamanho + 2);
                                        aStringBuilder.Insert(index, variaveis.FirstOrDefault(f => f.Item1 == i).Item4);

                                        ScriptOriginal = aStringBuilder.ToString();
                                    }

                                    File.WriteAllText(file, ScriptOriginal, Encoding.GetEncoding("ISO-8859-1"));
                                }
                                else
                                {

                                    File.WriteAllText(file, item.Tag.Replace("\r\n", "\r").Replace("\r", "\r\n") + "\r\n", Encoding.GetEncoding(1252));
                                   
                                }
                              
                            }
                            catch (Exception ex)
                            {
                                SBOApp.Application.SetStatusBarMessage(ex.Message);
                                return;
                            }

                            SBOApp.Application.SetStatusBarMessage("Arquivo " + file + " enviado com sucesso.", BoMessageTime.bmt_Short, false);
                            // Apenas para Escoteiros, para novos Clientes gerar Nova Versão e Comentar as Linhas do IF abaixo.
                            //if (layout!="004" && layout != "005")
                            //{
                            //    AtualizaImpressao(tagParams.Key);
                            //}
                            
                        }

                    }
                    catch (Exception ex)
                    {

                        SBOApp.Application.SetStatusBarMessage(ex.Message.ToString());
                    }

                }
                else
                {
                    try
                    {
                        var tag = new BLL.TagServices();

                        var tags = tag.GetTagScriptSQL(tag.GetTagSQL(layout), tagParams, quantidade);

                        var local = tag.GetTagSQL(layout).U_local;

                        if (local.Substring(local.Length - 1) != "\\")
                        {
                            local = local.Trim() + "\\";
                        }

                        foreach (var item in tags)
                        {

                            var num = ("000000" + item.Id);
                            num = num.Substring(num.Length - 6);

                            var file = local + "TAG."/* + tagParams.ObjType.Replace("|", "") + "."/* + tagParams.Key.Replace("'\'", "'-'") + "."*/ + num + "." + Guid.NewGuid() + ".prn";

                            try
                            {

                                File.WriteAllText(file, item.Tag.Replace("\r\n", "\r").Replace("\r", "\r\n") + "\r\n", Encoding.GetEncoding(1252));
                                //File.WriteAllText(file, item.Tag.Replace("\r\n", "\r").Replace("\r", "\r\n") + "\r\n", Encoding.GetEncoding(850));

                            }
                            catch (Exception ex)
                            {
                                SBOApp.Application.SetStatusBarMessage(ex.Message);
                                return;
                            }

                            SBOApp.Application.SetStatusBarMessage("Arquivo " + file + " enviado com sucesso.", BoMessageTime.bmt_Short, false);
                            //AtualizaImpressao(tagParams.Key);
                        }
                    }
                    catch (Exception ex)
                    {

                        SBOApp.Application.SetStatusBarMessage(ex.Message);
                    }

                }
                x = true;


            }


        }

        private void CheckAll()
        {
            Form.Freeze(true);
            for (int i = 0; i < oGrid.Rows.Count; i++)
            {
                oGrid.DataTable.SetValue(0, i, "Y");
            }
            Form.Freeze(false);
        }

        private void UnCheckAll()
        {
            Form.Freeze(true);
            for (int i = 0; i < oGrid.Rows.Count; i++)
            {
                oGrid.DataTable.SetValue(0, i, "N");
            }
            Form.Freeze(false);
        }

        private void AtualizaImpressao(string DocNum)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            string sql = string.Format(@" select T3.""DocNum"" from ORDR T0
                             inner join RDR1 T1 on T0.""DocEntry"" = T1.""DocEntry""
                             inner join INV1  T2 on T2.""BaseType""  = T0.""ObjType""
   					                            and T2.""BaseEntry"" = T0.""DocEntry""
   					                            and T2.""BaseLine""  = T1.""LineNum""
                             inner join OINV T3 On T3.""DocEntry"" = T2.""DocEntry""
                              where T0.""DocNum"" = {0}",DocNum);


            oRecordSet.DoQuery(sql);
            var DocNum_OINV = 0;
            if (!oRecordSet.EoF)
            {
                DocNum_OINV = oRecordSet.Fields.Item(0).Value;
                oRecordSet.MoveFirst();
            }


            oRecordSet.DoQuery(string.Format(@"update OINV set ""U_CVA_EtiquetaImpressa"" = 'Y' where ""DocNum"" = '{0}'", DocNum_OINV));
            oRecordSet.DoQuery(string.Format(@"update ORDR set ""U_CVA_EtiquetaImpressa"" = 'Y' where ""DocNum"" = '{0}'", DocNum));
        }
    }
}
