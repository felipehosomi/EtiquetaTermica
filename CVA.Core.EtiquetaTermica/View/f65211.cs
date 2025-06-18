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
    /// Ordem de produção
    /// </summary>
    class f65211 : BaseForm
    {
        public f65211()
        {
            FormCount++;
        }
        public f65211(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f65211(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f65211(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    CriaCamposEtiqueta();
                }
                else if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "6" || ItemEventInfo.ItemUID == "32")
                    {
                        CarregaCombosEtiqueta(true);
                    }
                }
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                if (!BusinessObjectInfo.BeforeAction)
                {
                    this.CarregaCombosEtiqueta(false);
                }
            }

            return true;
        }

        private void CarregaCombosEtiqueta(bool atualizar)
        {
            try
            {
                string itemCode = Form.DataSources.DBDataSources.Item("OWOR").GetValue("ItemCode", 0);
                string cardCode = Form.DataSources.DBDataSources.Item("OWOR").GetValue("CardCode", 0);


                ComboBox cb_Pad1 = Form.Items.Item("cb_Pad1").Specific;
                ComboBox cb_Pad2 = Form.Items.Item("cb_Pad2").Specific;
                int index = 0;

                while (cb_Pad1.ValidValues.Count > 0)
                {
                    try
                    {
                        cb_Pad1.ValidValues.Remove(0, BoSearchKey.psk_Index);
                    }
                    catch (Exception ex)
                    {
                        index++;
                        if (index == 10)
                        {
                            break;
                        }
                    }
                }

                while (cb_Pad2.ValidValues.Count > 0)
                {
                    try
                    {
                        cb_Pad2.ValidValues.Remove(0, BoSearchKey.psk_Index);
                    }
                    catch (Exception ex)
                    {
                        index++;
                        if (index == 10)
                        {
                            break;
                        }
                    }
                }


                string sql;
                if (!String.IsNullOrEmpty(cardCode))
                {
                    sql = String.Format(Query.Etiqueta_GetByPN, cardCode, itemCode);
                }
                else
                {
                    sql = String.Format(Query.EtiquetaValida_GetByItem, itemCode);
                }

                List<TagModel> list = new CrudController().FillModelListAccordingToSql<TagModel>(sql);

                try
                {
                    cb_Pad1.ValidValues.Add("", "");
                    cb_Pad2.ValidValues.Add("", "");
                }
                catch { }

                foreach (var item in list)
                {
                    try
                    {
                        cb_Pad1.ValidValues.Add(item.Code, item.Name);
                        cb_Pad2.ValidValues.Add(item.Code, item.Name);
                    }
                    catch { }
                }

                if (atualizar)
                {
                    if (!String.IsNullOrEmpty(cardCode))
                    {
                        if (list.Count == 1)
                        {
                            cb_Pad1.Select(list[0].Code);
                        }
                    }
                    else
                    {
                        sql = String.Format(Query.EtiquetaPadrao_GetByItem, itemCode);
                        list = new CrudController().FillModelListAccordingToSql<TagModel>(sql);
                        if (list.Count > 0)
                        {
                            cb_Pad1.Select(list[0].Code);
                            if (list.Count == 2)
                            {
                                cb_Pad2.Select(list[1].Code);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void CriaCamposEtiqueta()
        {
            Item itemRef = Form.Items.Item("255000116");

            Item it_Pad1 = Form.Items.Add("st_Pad1", BoFormItemTypes.it_STATIC);
            it_Pad1.Top = itemRef.Top + 17;
            it_Pad1.Height = itemRef.Height;
            it_Pad1.Left = itemRef.Left;
            it_Pad1.Width = itemRef.Width;

            ((StaticText)it_Pad1.Specific).Caption = "Etiqueta Padrão 1";

            Item it_CbPad1 = Form.Items.Add("cb_Pad1", BoFormItemTypes.it_COMBO_BOX);
            it_CbPad1.Top = itemRef.Top + 17;
            it_CbPad1.Height = itemRef.Height;
            it_CbPad1.Left = itemRef.Left + itemRef.Width + 2;
            it_CbPad1.Width = 240;
            it_CbPad1.DisplayDesc = true;
            ((ComboBox)it_CbPad1.Specific).DataBind.SetBound(true, "OWOR", "U_SO_EtiqPadrao1");

            itemRef = Form.Items.Item("st_Pad1");

            Item it_Pad2 = Form.Items.Add("st_Pad2", BoFormItemTypes.it_STATIC);
            it_Pad2.Top = itemRef.Top + 17;
            it_Pad2.Height = itemRef.Height;
            it_Pad2.Left = itemRef.Left;
            it_Pad2.Width = itemRef.Width;

            ((StaticText)it_Pad2.Specific).Caption = "Etiqueta Padrão 2";

            Item it_CbPad2 = Form.Items.Add("cb_Pad2", BoFormItemTypes.it_COMBO_BOX);
            it_CbPad2.Top = itemRef.Top + 17;
            it_CbPad2.Height = itemRef.Height;
            it_CbPad2.Left = itemRef.Left + itemRef.Width + 2;
            it_CbPad2.Width = 240;
            it_CbPad2.DisplayDesc = true;
            ((ComboBox)it_CbPad2.Specific).DataBind.SetBound(true, "OWOR", "U_SO_EtiqPadrao2");
        }
    }
}
