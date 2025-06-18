using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVA.AddOn.Common.Enums;

namespace CVA.AddOn.Common.Controllers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ModelControllerAttribute : Attribute
    {
        public int Index { get; set; }

        /// <summary>
        /// Nome da Coluna na tabela
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// É chave primária
        /// </summary>
        public bool IsPK { get; set; }

        /// <summary>
        /// Name da Tabela
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Nome do campo na tela
        /// </summary>
        public string UIFieldName { get; set; }

        /// <summary>
        /// Descricao do campo
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Campo obrigatório
        /// </summary>
        public bool MandatoryYN { get; set; }

        /// <summary>
        /// Nome do campo no DataSource
        /// </summary>
        public string DataSourceFieldName { get; set; }


        /// <summary>
        /// Preenche campo (Banco)?
        /// </summary>
        public bool FillOnSelect { get; set; }

        /// <summary>
        /// Preenche campo (Tela)?
        /// </summary>
        public bool AutoFill { get; set; }

        /// <summary>
        /// Campo para ser tratado no banco de dados?
        /// </summary>
        public bool DataBaseFieldYN { get; set; }

        /// <summary>
        /// Campo para ser tratado no banco de dados?
        /// </summary>
        public bool SBOReadOnly { get; set; }

        public UIFieldTypeEnum UIFieldType { get; set; }

        public ModelControllerAttribute()
        {
            DataBaseFieldYN = true;
            AutoFill = true;
            FillOnSelect = true;
        }
    }
}
