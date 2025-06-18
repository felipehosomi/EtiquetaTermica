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
    /// Cadastro de Item
    /// </summary>
    public class f150 : BaseForm
    {
        public f150()
        {
            FormCount++;
        }
        public f150(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f150(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f150(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        //public override object Show()
        //{
        //    Form form = (Form)base.Show();
        //    this.CriaAbaEtiqueta(form);
        //    return form;
        //}

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    this.CriaAbaEtiqueta(Form);
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
                if (BusinessObjectInfo.BeforeAction)
                {
                    return ValidaEtiqueta();
                }
                else
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

        private bool ValidaEtiqueta()
        {
            DataTable dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
            int count = 0;
            for (int i = 0; i < dt_Etiq.Rows.Count; i++)
            {
                if (dt_Etiq.GetValue("Etiqueta Padrão", i) == "Y")
                {
                    count++;
                }
                if (count > 2)
                {
                    SBOApp.Application.MessageBox("Quantidade máxima de etiquetas padrão é 2");
                    return false;
                }
            }
            return true;
        }

        private void GravaEtiquetas()
        {
            GridController gridController = new GridController();
            List<EtiquetaItemModel> list = gridController.FillModelFromTable<EtiquetaItemModel>(Form.DataSources.DataTables.Item("dt_Etiq"));

            string itemCode = Form.DataSources.DBDataSources.Item("OITM").GetValue("ItemCode", 0);

            foreach (var item in list)
            {
                string sql;
                if (item.Code == 0)
                {
                    sql = String.Format(Query.EtiquetaItem_Insert, item.CodigoEtiqueta, itemCode, item.EtiquetaPadrao, item.ModeloValido);
                }
                else
                {
                    sql = String.Format(Query.EtiquetaItem_Update, item.Code, item.EtiquetaPadrao, item.ModeloValido);
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
                CriaAbaEtiqueta(Form);
                dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
            }

            Grid gr_Etiq = (Grid)Form.Items.Item("gr_Etiq").Specific;
            string itemCode = Form.DataSources.DBDataSources.Item("OITM").GetValue("ItemCode", 0);
            dt_Etiq.ExecuteQuery(String.Format(Query.EtiquetaItem_Get, itemCode));

            gr_Etiq.Columns.Item("Code").Visible = false;
            gr_Etiq.Columns.Item("Código").Editable = false;
            gr_Etiq.Columns.Item("Descrição").Editable = false;

            gr_Etiq.Columns.Item("Etiqueta Padrão").Type = BoGridColumnType.gct_CheckBox;
            gr_Etiq.Columns.Item("Modelo Válido").Type = BoGridColumnType.gct_CheckBox;
        }

        private void CriaAbaEtiqueta(Form form)
        {
            try
            {
                form.DataSources.UserDataSources.Add("ds_Etiq", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);

                Item itmFolder = form.Items.Add("fl_Etiq", BoFormItemTypes.it_FOLDER);
                Item itemRef = form.Items.Item("9");

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

                itemRef = form.Items.Item("129");
                Item it_GrEtiq = form.Items.Add("gr_Etiq", BoFormItemTypes.it_GRID);
                it_GrEtiq.FromPane = 99;
                it_GrEtiq.ToPane = 99;
                it_GrEtiq.Top = itemRef.Top;
                it_GrEtiq.Height = itemRef.Height;
                it_GrEtiq.Left = itemRef.Left;
                it_GrEtiq.Width = itemRef.Width;
                it_GrEtiq.Visible = true;

                Grid gr_Etiq = (Grid)it_GrEtiq.Specific;
                gr_Etiq.DataTable = form.DataSources.DataTables.Add("dt_Etiq");

                form.PaneLevel = 1;
            }
            catch { }
        }
    }
}
