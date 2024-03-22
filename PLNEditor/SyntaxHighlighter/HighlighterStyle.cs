using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;
using FastColoredTextBoxNS;

namespace PLNEditor.SyntaxHighlighter
{
    [DataContract]
    [KnownType(typeof(List<Brackets>))]
    [KnownType(typeof(List<ColorStyle>))]
    [KnownType(typeof(List<HighlighterElement>))]
    [KnownType(typeof(List<FoldingMarker>))]
    [KnownType(typeof(RegexOptions))]
    [KnownType(typeof(FontStyle))]
    [KnownType(typeof(GraphicsUnit))]
    public class HighlighterStyle
    {
        [DataMember]
        public GlobalHighlighterSettings GlobalSettings { get; private set; }
        [DataMember]
        public string CommentPrefix { get; private set; }
        [DataMember]
        public IReadOnlyList<Brackets> Brackets { get; private set; }
        [DataMember]
        public IReadOnlyList<ColorStyle> ColorStyles { get; private set; }
        [DataMember]
        public IReadOnlyList<HighlighterElement> HighlighterElements { get; private set; }
        [DataMember]
        public IReadOnlyList<FoldingMarker> FoldingMarkers { get; private set; }

        public HighlighterStyle(GlobalHighlighterSettings globalSettings, string commentPrefix, IReadOnlyList<Brackets> brackets, IReadOnlyList<ColorStyle> colorStyles, IReadOnlyList<HighlighterElement> highlighterElements, IReadOnlyList<FoldingMarker> foldingMarkers)
        {
            GlobalSettings = globalSettings;
            CommentPrefix = commentPrefix;
            Brackets = brackets;
            ColorStyles = colorStyles;
            HighlighterElements = highlighterElements;
            FoldingMarkers = foldingMarkers;
        }

        public static void Save(HighlighterStyle obj, string path)
        {
            using (var writer = XmlWriter.Create(path,settings))
                Serializer.WriteObject(writer, obj);
        }

        public  static HighlighterStyle Load(string path)
        {
            using (var reader = XmlReader.Create(path))
                return (HighlighterStyle)Serializer.ReadObject(reader);
        }

        private static DataContractSerializer Serializer = new DataContractSerializer(typeof(HighlighterStyle), null, int.MaxValue, false, false, new HighlighterSurrogate());
        private static XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

    }
   


    [Serializable]
    [DataContract]
    public struct GlobalHighlighterSettings
    {
        [DataMember]
        public Font Font { get; private set; }
        [DataMember]
        public Color BackgroundColor { get; private set; }
        [DataMember]
        public Color ForeColor { get; private set; }

        public GlobalHighlighterSettings(Font font, Color backgroundColor, Color foreColor) : this()
        {
            Font = font;
            BackgroundColor = backgroundColor;
            ForeColor = foreColor;
        }
    }

    [Serializable]
    [DataContract]
    public struct Brackets
    {
        [DataMember]
        public char Open { get; private set; }
        [DataMember]
        public char Close { get; private set; }
        [DataMember]
        public Color Color { get; private set; }

        public Brackets(char open, char close, Color color) : this()
        {
            Open = open;
            Close = close;
            Color = color;
        }
    }

    [Serializable]
    [DataContract]
    public struct HighlighterElement
    {
        [DataMember]
        public int StyleIndex { get; private set; }
        [DataMember]
        public Regex Regex { get; private set; }

        public HighlighterElement(int styleIndex, Regex regex) : this()
        {
            StyleIndex = styleIndex;
            Regex = regex;
        }
    }

    [Serializable]
    [DataContract]
    public struct FoldingMarker
    {
        [DataMember]
        public string StartFoldingMarker { get; private set; }
        [DataMember]
        public string EndFoldingMarker { get; private set; }
        [DataMember]
        public RegexOptions RegexOptions { get; private set; }

        public FoldingMarker(string startFoldingMarker, string endFoldingMarker, RegexOptions regexOptions)
        {
            StartFoldingMarker = startFoldingMarker;
            EndFoldingMarker = endFoldingMarker;
            RegexOptions = regexOptions;
        }

        public FoldingMarker(string startFoldingMarker, string endFoldingMarker) : this(startFoldingMarker, endFoldingMarker, RegexOptions.None)
        {
        }

    }


    [Serializable]
    [DataContract]
    public struct ColorStyle
    {
        [DataMember]
        public Color ForeColor { get; private set; }
        [DataMember]
        public Color BackgroundColor { get; private set; }
        [DataMember]
        public FontStyle FontStyle { get; private set; }
        [DataMember]
        public StringFormat StringFormat { get; private set; }

        public ColorStyle(Color foreColor, Color backgroundColor, FontStyle fontStyle, StringFormat stringFormat)
        {
            ForeColor = foreColor;
            BackgroundColor = backgroundColor;
            FontStyle = fontStyle;
            StringFormat = stringFormat;
        }

        public ColorStyle(Color foreColor, Color backgroundColor, FontStyle fontStyle) : this(foreColor, backgroundColor, fontStyle, null)
        {
        }

        public ColorStyle(Color foreColor, FontStyle fontStyle) : this(foreColor, Color.Transparent, fontStyle, null)
        {
        }

        public ColorStyle(Color foreColor) : this(foreColor, Color.Transparent, FontStyle.Regular, null)
        {
        }
    }

}
