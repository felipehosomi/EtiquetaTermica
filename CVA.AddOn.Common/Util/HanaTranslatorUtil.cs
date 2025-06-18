using Translator;

namespace CVA.AddOn.Common.Util
{
    class HanaTranslatorUtil
    {
        public static TranslatorTool Translator { get; private set; }

        public static string Translate(string sql)
        {
            int count;
            int errCount;

            if (Translator == null)
            {
                Translator = new TranslatorTool();
            }
            sql = Translator.TranslateQuery(sql, out count, out errCount);
            sql = sql.Substring(0, sql.Length - 3);
            return sql;

        }
    }
}
