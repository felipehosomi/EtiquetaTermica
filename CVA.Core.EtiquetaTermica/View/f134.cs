using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.Core.EtiquetaTermica.DAO;
using CVA.Core.EtiquetaTermica.Model;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.Core.EtiquetaTermica.View
{
    /// <summary>
    /// Cadastro de PN
    /// </summary>
    class f134 : BaseForm
    {
        public f134()
        {
            FormCount++;
        }
        public f134(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f134(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f134(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    this.CriaAbaEtiqueta();
                }
                else if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "fl_Etiq")
                    {
                        Form.PaneLevel = 99;
                    }
                }
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
            {
                if (!BusinessObjectInfo.BeforeAction)
                {
                    this.GravaEtiquetas();
                }
            }
            else if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                if (!BusinessObjectInfo.BeforeAction)
                {
                    this.CarregaGrid();
                }
            }

            return true;
        }

        private void GravaEtiquetas()
        {
            GridController gridController = new GridController();
            List<EtiquetaPNModel> list = gridController.FillModelFromTable<EtiquetaPNModel>(Form.DataSources.DataTables.Item("dt_Etiq"));

            string cardCode = Form.DataSources.DBDataSources.Item("OCRD").GetValue("CardCode", 0);

            foreach (var item in list)
            {
                string sql;
                if (item.Code == 0)
                {
                    sql = String.Format(Query.EtiquetaPN_Insert, item.CodigoEtiqueta, cardCode, item.EtiquetaPadrao);
                }
                else
                {
                    sql = String.Format(Query.EtiquetaPN_Update, item.Code, item.EtiquetaPadrao);
                }
                CrudController.ExecuteNonQuery(sql);
            }

        }

        private void CarregaGrid()
        {
            DataTable dt_Etiq;
            try
            {
                dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
            }
            catch
            {
                CriaAbaEtiqueta();
                dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
            }

            string cardCode = Form.DataSources.DBDataSources.Item("OCRD").GetValue("CardCode", 0);
            dt_Etiq.ExecuteQuery(String.Format(Query.EtiquetaPN_Get, cardCode));

            Grid gr_Etiq = (Grid)Form.Items.Item("gr_Etiq").Specific;

            gr_Etiq.Columns.Item("Code").Visible = false;
            gr_Etiq.Columns.Item("Código").Editable = false;
            gr_Etiq.Columns.Item("Descrição").Editable = false;

            gr_Etiq.Columns.Item("Etiqueta Padrão").Type = BoGridColumnType.gct_CheckBox;
        }

        private void CriaAbaEtiqueta()
        {
            try
            {
                Form.DataSources.UserDataSources.Add("ds_Etiq", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);

                Item itmFolder = Form.Items.Add("fl_Etiq", BoFormItemTypes.it_FOLDER);
                Item itemRef = Form.Items.Item("9");

                itmFolder.Top = itemRef.Top;
                itmFolder.Height = itemRef.Height;
                itmFolder.Left = itemRef.Left + itemRef.Width + 100;
                itmFolder.Width = itemRef.Width;
                itmFolder.AffectsFormMode = false;
                itmFolder.Visible = true;
                Folder folder = (Folder)itmFolder.Specific;
                folder.Caption = "Etiqueta";
                folder.DataBind.SetBound(true, "", "ds_Etiq");
                folder.Pane = 99;
                folder.GroupWith("9");

                itemRef = Form.Items.Item("21");
                Item it_GrEtiq = Form.Items.Add("gr_Etiq", BoFormItemTypes.it_GRID);
                it_GrEtiq.FromPane = 99;
                it_GrEtiq.ToPane = 99;
                it_GrEtiq.Top = itemRef.Top;
                it_GrEtiq.Height = itemRef.Height;
                it_GrEtiq.Left = itemRef.Left;
                it_GrEtiq.Width = itemRef.Width;
                it_GrEtiq.Visible = true;

                Grid gr_Etiq = (Grid)it_GrEtiq.Specific;
                gr_Etiq.DataTable = Form.DataSources.DataTables.Add("dt_Etiq");

                Form.PaneLevel = 1;
            }
            catch { }
        }
    }
}
