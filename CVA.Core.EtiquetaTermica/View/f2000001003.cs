using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.EtiquetaTermica.DAO;
using CVA.Core.EtiquetaTermica.Model;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CVA.Core.EtiquetaTermica.View
{
    /// <summary>
    /// Gerador de Etiquetas por OP
    /// </summary>
    public class f2000001003 : BaseForm
    {
        public f2000001003()
        {
            FormCount++;
        }

        public f2000001003(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001003(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001003(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Pesq")
                    {
                        this.Pesquisa();
                    }
                    else if (ItemEventInfo.ItemUID == "bt_Imp")
                    {
                        this.Imprime();
                    }
                    else if (ItemEventInfo.ItemUID == "bt_Vis")
                    {
                        this.Visualizar();
                    }
                }
                //else if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                //{
                //    Form.DataSources.UserDataSources.Item("ud_Cop").Value = "1";
                //}
            }
            return true;
        }

        private void Pesquisa()
        {
            try
            {
                Form.Freeze(true);
                string op = Form.DataSources.UserDataSources.Item("ud_OP").Value;

                if (!string.IsNullOrEmpty(op))
                {
                    DataTable dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
                    dt_Etiq.ExecuteQuery(string.Format(Query.Etiqueta_GetByOrdemProducao, op));

                    Grid gr_Etiq = Form.Items.Item("gr_Etiq").Specific;
                    gr_Etiq.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;
                    gr_Etiq.CollapseLevel = 1;

                    gr_Etiq.Columns.Item("Item").Editable = false;
                    gr_Etiq.Columns.Item("Etiqueta").Editable = false;
                    gr_Etiq.Columns.Item("DocEntry").Visible = false;
                    gr_Etiq.Columns.Item("CodEtiq").Visible = false;

                    //gr_Etiq.Rows.SelectedRows.Add(1);
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        private void Visualizar()
        {
            DataTable dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
            List<TagParameterModel> tagList = new List<TagParameterModel>();

            Grid gr_Etiq = Form.Items.Item("gr_Etiq").Specific;
            int row = gr_Etiq.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder) - 1;
            TagParameterModel tagParams = new TagParameterModel();

            tagParams.ObjType = "202";
            tagParams.Key = dt_Etiq.GetValue("DocEntry", row).ToString();
            tagParams.cl1 = Form.DataSources.UserDataSources.Item("ud_UN").Value;
            tagParams.cl5 = Form.DataSources.UserDataSources.Item("ud_Corte").Value;
            tagParams.cl8 = dt_Etiq.GetValue("CodEtiq", row);
            tagList.Add(tagParams);

            if (tagList.Count == 0)
            {
                SBOApp.Application.MessageBox("Nenhum registro foi selecionado");
                return;
            }

            var quantidade = Convert.ToInt32(Form.DataSources.UserDataSources.Item("ud_Cop").Value);

            foreach (var item in tagList)
            {
                ProcessaImpressaoEtiqueta(item.cl8, item, quantidade, true);
            }
        }

        private void Imprime()
        {
            DataTable dt_Etiq = Form.DataSources.DataTables.Item("dt_Etiq");
            List<TagParameterModel> tagList = new List<TagParameterModel>();

            for (int i = 0; i < dt_Etiq.Rows.Count; i++)
            {
                if (dt_Etiq.GetValue("#", i) == "Y")
                {
                    TagParameterModel tagParams = new TagParameterModel();

                    tagParams.ObjType = "202";
                    tagParams.Key = dt_Etiq.GetValue("DocEntry", i).ToString();
                    tagParams.cl1 = Form.DataSources.UserDataSources.Item("ud_UN").Value;
                    tagParams.cl5 = Form.DataSources.UserDataSources.Item("ud_Corte").Value;
                    tagParams.cl8 = dt_Etiq.GetValue("CodEtiq", i);
                    tagList.Add(tagParams);
                }
            }
            if (tagList.Count == 0)
            {
                SBOApp.Application.MessageBox("Nenhum registro foi selecionado");
                return;
            }

            var quantidade = Convert.ToInt32(Form.DataSources.UserDataSources.Item("ud_Cop").Value);

            foreach (var item in tagList)
            {
                ProcessaImpressaoEtiqueta(item.cl8, item, quantidade, false);
            }
        }

        private void ProcessaImpressaoEtiqueta(string layout, TagParameterModel tagParams, int quantidade = 1, bool visualizar = false)
        {
            try
            {
                var tag = new BLL.TagServices();

                var tags = tag.GetTagScriptHANA(tag.GetTagHANA(layout), tagParams, quantidade);

                var etiqueta = tag.GetTagHANA(layout);
                var local = etiqueta.U_local;
                if (local.Substring(local.Length - 1) != "\\")
                {
                    local = local.Trim() + "\\";
                }

                if (tags.Count == 0)
                {
                    SBOApp.Application.SetStatusBarMessage("Etiqueta não encontrada para os dados informados");
                }
                
                foreach (var item in tags)
                {
                    var num = ("000000" + item.Id);
                    num = num.Substring(num.Length - 6);

                    var file = local + "TAG." + num + "_" + tagParams.Key + "." + Guid.NewGuid() + ".prn"; //  /*+ tagParams.ObjType.Replace("|", "") + "." + tagParams.Key + "."*/ + Guid.NewGuid() +"_"+ c + ".prn";

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

                                var valor = item.Tag.Substring(index + 2, tamanho - 2);
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

                                aStringBuilder.Remove(index, tamanho + 2);
                                aStringBuilder.Insert(index, variaveis.FirstOrDefault(f => f.Item1 == i).Item4);

                                ScriptOriginal = aStringBuilder.ToString();
                            }

                            File.WriteAllText(file, ScriptOriginal, Encoding.GetEncoding("ISO-8859-1"));
                        }
                        else
                        {
                            if (visualizar)
                            {
                                tag.VisualizarEtiqueta(item.Tag, file);
                            }
                            else
                            {
                                File.WriteAllText(file, item.Tag.Replace("\r\n", "\r").Replace("\r", "\r\n") + "\r\n", Encoding.GetEncoding(1252));
                                SBOApp.Application.SetStatusBarMessage("Arquivo " + file + " enviado com sucesso.", BoMessageTime.bmt_Short, false);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.SetStatusBarMessage(ex.Message);
                        return;
                    }  
                }

            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message.ToString());
            }
        }
    }
}
