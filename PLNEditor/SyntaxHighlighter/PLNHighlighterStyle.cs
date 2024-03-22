using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace PLNEditor.SyntaxHighlighter
{
    public static class PLNHighlighterStyle
    {
        public static HighlighterStyle Get()
        {
            var globalSettings = new GlobalHighlighterSettings(new Font("Consolas", 11), Color.White, Color.Black);

            var brackets = new List<Brackets>()
            {
                new Brackets('(', ')',Color.FromArgb(150,30,30,170)), new Brackets('[', ']', Color.FromArgb(150,30,30,170))
            };

            var colors = new List<ColorStyle>()
            {
                new ColorStyle(Color.Green),
                new ColorStyle(Color.Blue, FontStyle.Italic),
                new ColorStyle(Color.Magenta),
                new ColorStyle(Color.Red),
                new ColorStyle(Color.Brown,FontStyle.Italic),
                new ColorStyle(Color.SaddleBrown,FontStyle.Italic),
                new ColorStyle(Color.Black,FontStyle.Bold),
                new ColorStyle(Color.Navy,FontStyle.Bold),
                new ColorStyle(Color.Navy,FontStyle.Bold),//8,
                new ColorStyle(Color.Navy,FontStyle.Bold),
                 new ColorStyle(Color.Navy),
                 new ColorStyle(Color.ForestGreen,FontStyle.Bold),
                 new ColorStyle(Color.DimGray)
            };
            var elements = new List<HighlighterElement>()
            {
                new HighlighterElement(0, new Regex(@"//.*", RegexOptions.Multiline)),
                new HighlighterElement(4,new Regex(@"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'",RegexOptions.Multiline)),
                new HighlighterElement(3, new Regex(@"\b(\d+\.\d+)\b")),
                new HighlighterElement(2, new Regex(@"\b\d+\b")),
                new HighlighterElement(6,new Regex(@"\b(Блок|Функция|Импорт|Подключить)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(7,new Regex(@"\b(цикл|пока|делать|если|иначе|идти|Вернуть|шаг|прервать|всё|пропустить|перезапустить)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(8,new Regex(@"\b(новый|ссылка|тип|к|является)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(9,new Regex(@"\b(и|или|иИли|не)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(10,new Regex(@"\b(авто|нмассив)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(1,new Regex(@"\b(истина|ложь|да|нет|нуль)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(10,new Regex(@"\b(БЦелое8|Байт|Целое8|Целое16|БЦелое16|Целое|БЦелое|Целое64|БЦелое64|Вещественное|Дробное|Логическое|Булевый|Булево|Символ|Строка|Объект|Стек|Массив|Список|Очередь)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(11,new Regex(@"\b(Консоль|Математика|Конвертер)\b",RegexOptions.IgnoreCase)),
                new HighlighterElement(12,new Regex(@"\#\[[^\]\n]*\]",RegexOptions.IgnoreCase))
            };

            var markers = new List<FoldingMarker>()
            {
                new FoldingMarker("{","}")
            };

            return new HighlighterStyle(globalSettings, "//", brackets, colors, elements, markers);
        }

    }
}
