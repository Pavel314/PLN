#define ENABLE_ENGLISH
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Analysis
{
 public  class PLNKeywords
    {
        public static IReadOnlyDictionary<string,int> Keywords { get; private set; }

        static PLNKeywords()
        {
            var keywords = new Dictionary<string, int>(StringHelper.PLNComparer);
            keywords.Add("авто", (int)Tokens.AUTO);
            keywords.Add("Блок", (int)Tokens.PROCEDURE);
            keywords.Add("Функция", (int)Tokens.FUNCTION);
            keywords.Add("НМассив", (int)Tokens.ARRAY);

            keywords.Add("цикл", (int)Tokens.LOOP);
            keywords.Add("пока", (int)Tokens.WHILE);
            keywords.Add("делать", (int)Tokens.DO);

            keywords.Add("прервать", (int)Tokens.BREAK);
            keywords.Add("всё", (int)Tokens.ALL);
            keywords.Add("пропустить", (int)Tokens.CONTINUE);
            keywords.Add("перезапустить", (int)Tokens.CONTINUE);

            keywords.Add("Вернуть", (int)Tokens.RETURNN);
            keywords.Add("шаг", (int)Tokens.STEP);
            keywords.Add("если", (int)Tokens.IF);
            keywords.Add("иначе", (int)Tokens.ELSE);
            keywords.Add("и", (int)Tokens.AND);
            keywords.Add("или", (int)Tokens.OR);
            keywords.Add("иИли", (int)Tokens.XOR);
            keywords.Add("истина", (int)Tokens.TRUE);
            keywords.Add("ложь", (int)Tokens.FALSE);
            keywords.Add("да", (int)Tokens.TRUE);
            keywords.Add("нет", (int)Tokens.FALSE);
            keywords.Add("нуль", (int)Tokens.NULL);
            keywords.Add("импорт", (int)Tokens.IMPORT);
            keywords.Add("Подключить", (int)Tokens.USING);
            keywords.Add("новый", (int)Tokens.NEW);
            keywords.Add("не", (int)Tokens.INVERSE);
            keywords.Add("идти", (int)Tokens.GOTO);
            keywords.Add("ссылка", (int)Tokens.REF);
            keywords.Add("тип", (int)Tokens.TYPEOF);
            keywords.Add("к", (int)Tokens.AS);
            keywords.Add("является", (int)Tokens.IS);

            #region directives
            keywords.Add("ОТКЛЮЧИТЬ_СИСТЕМНУЮ_БИБЛИОТЕКУ",(int)Tokens.DISABLE_SYSTEM_LIBRARY);
            keywords.Add("ОКОННОЕ_ПРИЛОЖЕНИЕ", (int)Tokens.WINDOWS_APPLICATION);
            keywords.Add("КОНСОЛЬНОЕ_ПРИЛОЖЕНИЕ", (int)Tokens.CONSOLE_APPLICATION);
            #endregion

#if ENABLE_ENGLISH
            keywords.Add("NArray", (int)Tokens.ARRAY);
            keywords.Add("Import", (int)Tokens.IMPORT);
            keywords.Add("Using", (int)Tokens.USING);
            keywords.Add("New", (int)Tokens.NEW);
            keywords.Add("true", (int)Tokens.TRUE);
            keywords.Add("false", (int)Tokens.FALSE);
            keywords.Add("null", (int)Tokens.NULL);
            keywords.Add("not", (int)Tokens.INVERSE);
            keywords.Add("and", (int)Tokens.AND);
            keywords.Add("or", (int)Tokens.OR);
            keywords.Add("xor", (int)Tokens.XOR);
            keywords.Add("if", (int)Tokens.IF);
            keywords.Add("else", (int)Tokens.ELSE);
            keywords.Add("goto", (int)Tokens.GOTO);
            keywords.Add("while", (int)Tokens.WHILE);
            keywords.Add("loop", (int)Tokens.LOOP);
            keywords.Add("do", (int)Tokens.DO);
            keywords.Add("break", (int)Tokens.BREAK);
            keywords.Add("continue", (int)Tokens.CONTINUE);
            keywords.Add("ref", (int)Tokens.REF);
            keywords.Add("all", (int)Tokens.ALL);
            keywords.Add("typeof", (int)Tokens.TYPEOF);
            keywords.Add("auto", (int)Tokens.AUTO);
            keywords.Add("as", (int)Tokens.AS);
            keywords.Add("is", (int)Tokens.IS);
#endif
            Keywords = keywords;
        }
        public static int GetToken(string str)
        {
            if (Keywords.TryGetValue(str, out int token))
                return token;
            return (int)Tokens.ID;
        }

    }
}
