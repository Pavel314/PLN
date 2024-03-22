using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;
using System.Drawing;
using System.Collections.ObjectModel;
using System.CodeDom;
using FastColoredTextBoxNS;


namespace PLNEditor.SyntaxHighlighter
{

    public class HighlighterSurrogate : IDataContractSurrogate
    {

        public HighlighterSurrogate()
        {
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public Type GetDataContractType(Type type)
        {
            if (type == typeof(Color))
                return typeof(ViewColor);
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            if (targetType == typeof(Color))
            {
                return ((ViewColor)obj).ToColor();
            }
            return obj;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (targetType == typeof(ViewColor))
            {
                return new ViewColor(((Color)obj));
            }
            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
        [Serializable]
        [DataContract]
        public struct ViewColor
        {
            public ViewColor(Color c)
            {
                Name = c.Name;
            }

            public Color ToColor()
            {
                return Color.FromName(Name);
            }
            [DataMember]
            public string Name { get; private set; }
        }
    }
}
