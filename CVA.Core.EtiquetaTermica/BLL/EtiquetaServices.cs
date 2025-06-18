using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.EtiquetaTermica.DAO;
using CVA.Core.EtiquetaTermica.Model;
using CVA.Core.EtiquetaTermica.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.EtiquetaTermica.BLL
{
    public class TagServices
    {
        SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

        #region SQL

        public List<TagModel> GetTagListSQL()
        {
            var lista = new List<TagModel>();

            oRecordSet.DoQuery(Query.EtiquetaServicesGetTagListSql);

            while(!oRecordSet.EoF)
            {
                var model = new TagModel();

                model.Code = oRecordSet.Fields.Item(0).Value;
                model.Name = oRecordSet.Fields.Item(1).Value;

                lista.Add(model);

                oRecordSet.MoveNext();
            }
            return lista;
        }

        public TagFullModel GetTagSQL(string tagCode)
        {
            var tag = new TagFullModel();

            //var sql = string.Format(this.GetSQL("EtiquetaServices.GetTag.sql"), tagCode);

            //tag = B1DAO.ExecuteSqlForObject<TagFullModel>(sql);

            oRecordSet.DoQuery(string.Format(Query.EtiquetaServicesGetTagSql,tagCode));

            if (!oRecordSet.EoF)
            {
                tag.Code = oRecordSet.Fields.Item(0).Value;
                tag.Name = oRecordSet.Fields.Item(1).Value;
                tag.U_descricao = oRecordSet.Fields.Item(2).Value;
                tag.U_script = oRecordSet.Fields.Item(3).Value;
                tag.U_linguagem = oRecordSet.Fields.Item(4).Value;
                tag.U_procedure = oRecordSet.Fields.Item(5).Value;
                tag.U_local = oRecordSet.Fields.Item(6).Value;
                tag.U_procpesq = oRecordSet.Fields.Item(7).Value;
            }
            return tag;
        }

        public List<TagResult> GetTagScriptSQL(TagFullModel tag, TagParameterModel tagParams, int quantidade)
        {
            var sql = "exec " + tag.U_procedure
                + " '" + tag.Code.Replace("'", "")
                + "', '" + tagParams.ObjType.Replace("'", "")
                + "', '" + tagParams.Key.Replace("'\'", "-")
                + "', " + quantidade.ToString() + ", " +
                "'" + (tagParams.cl1 ?? "") + "', " +
                "'" + (tagParams.cl2 ?? "") + "', " +
                "'" + (tagParams.cl3 ?? "") + "', " +
                "'" + (tagParams.cl4 ?? "") + "', " +
                "'" + (tagParams.cl5 ?? "") + "', " +
                "'" + (tagParams.cl6 ?? "") + "', " +
                "'" + (tagParams.cl7 ?? "") + "', " +
                "'" + (tagParams.cl8 ?? "") + "'";



            var lista = new List<TagResult>();     //--result = B1DAO.ExecuteSqlForList<TagResult>(sql);

            oRecordSet.DoQuery(sql);

            while(!oRecordSet.EoF)
            {
                var model = new TagResult();

                model.Id = oRecordSet.Fields.Item(0).Value;
                model.Tag = oRecordSet.Fields.Item(1).Value;

                lista.Add(model);
                oRecordSet.MoveNext();
            }
            return lista;
        }

        #endregion

        #region HANA

        public List<TagModel> GetTagListHANA()
        {
            var lista = new List<TagModel>();

            oRecordSet.DoQuery(Query.EtiquetaServicesGetTagListHana);

            oRecordSet.MoveFirst();

            while(!oRecordSet.EoF)
            {
                var model = new TagModel();

                model.Code = oRecordSet.Fields.Item(0).Value;
                model.Name = oRecordSet.Fields.Item(1).Value;

                lista.Add(model);

                oRecordSet.MoveNext();
            }
            return lista;
        }

        public TagFullModel GetTagHANA(string tagCode)
        {
            var tag = new TagFullModel();

            var hana = string.Format(Query.EtiquetaServicesGetTagHana,tagCode);

            oRecordSet.DoQuery(hana);

            if (!oRecordSet.EoF)
            {
                tag.Code = oRecordSet.Fields.Item(0).Value;
                tag.Name = oRecordSet.Fields.Item(1).Value;
                tag.U_descricao = oRecordSet.Fields.Item(2).Value;
                tag.U_script = oRecordSet.Fields.Item(3).Value;
                tag.U_linguagem = oRecordSet.Fields.Item(4).Value;
                tag.U_procedure = oRecordSet.Fields.Item(5).Value;
                tag.U_local = oRecordSet.Fields.Item(6).Value;
                tag.U_procpesq = oRecordSet.Fields.Item(7).Value;
                tag.U_caminhoscript = oRecordSet.Fields.Item(8).Value;

                oRecordSet.MoveFirst();
            }

            var result = tag;
            return result;
        }

        public List<TagResult> GetTagScriptHANA(TagFullModel tag, TagParameterModel tagParams, int quantidade)
        {
            var sql = "CALL " +'"'+tag.U_procedure+'"'
                +"("
                + " '" + tag.Code.Replace("'", "")
                + "', '" + tagParams.ObjType.Replace("'", "")
                + "', '" + tagParams.Key.Replace("'", "")
                + "', " + quantidade.ToString() + ", " +
                "'" + (tagParams.cl1 ?? "") + "', " +
                "'" + (tagParams.cl2 ?? "") + "', " +
                "'" + (tagParams.cl3 ?? "") + "', " +
                "'" + (tagParams.cl4 ?? "") + "', " +
                "'" + (tagParams.cl5 ?? "") + "', " +
                "'" + (tagParams.cl6 ?? "") + "', " +
                "'" + (tagParams.cl7 ?? "") + "', " +
                "'" + (tagParams.cl8 ?? "") +  "')";

            var lista = new List<TagResult>();

            oRecordSet.DoQuery(sql);

            while(!oRecordSet.EoF)
            {
                var model = new TagResult();

                model.Id = Convert.ToInt32(oRecordSet.Fields.Item(0).Value);
                model.Tag = oRecordSet.Fields.Item(1).Value;

                lista.Add(model);
                oRecordSet.MoveNext();
            }
            return lista;
        }

        public void VisualizarEtiqueta(string body, string path)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.ExpectContinue = false;

            var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = client.PostAsync("http://api.labelary.com/v1/printers/8dpmm/labels/3x2/0/", content).Result;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            if (response.IsSuccessStatusCode)
            {
                var responseStream = response.Content.ReadAsStreamAsync().Result;
                var fileStream = File.Create(path.Replace("prn", "png")); // change file name for PNG images
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();

                f2000001004 form = new f2000001004();
                form.FormID = 1004;
                form.SrfFolder = "srfFiles";
                form.Show(path.Replace("prn", "png"));
                //System.Diagnostics.Process.Start(path.Replace("prn", "png"));

            }
        }

        #endregion
    }
}
