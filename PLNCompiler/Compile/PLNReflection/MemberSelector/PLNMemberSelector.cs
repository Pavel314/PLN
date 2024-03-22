using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.MemberSelector
{
    public class PLNMemberSelector:IMemberSelector
    {
        public SelectFieldResult SelectField(Type type, FieldHeader header)
        {
            return new SelectFieldResult(new HeaderInitiator<FieldHeader>(type, header), type.GetFields(header.GetBindingsFlags()).
            Where(f => FieldFilter(f, header, GetRealName(type, header))).ToArray());
        }
        protected virtual bool FieldFilter(FieldInfo member, FieldHeader header, string realName)
        {
            return NameFilter(member.Name, realName);
        }








        public SelectPropertyResult SelectProperty(Type type, PropertyHeader header)
        {
            var candidates = type.GetProperties(header.GetBindingsFlags());
            return new SelectPropertyResult(new HeaderInitiator<PropertyHeader>(type, header), FindMin(PropertyFilter, header, candidates, GetRealName(type, header)));
        }
        protected virtual int PropertyFilter(PropertyInfo member, PropertyHeader header, string realName)
        {
            if (!NameFilter(member.Name, realName)) return -1;

            var getMethod = member.GetGetMethod(true);
            if (!PropertyHeader.CompareAcces(getMethod, header.Getter)) return -1;
            var setMethod = member.GetSetMethod(true);
            if (!PropertyHeader.CompareAcces(setMethod, header.Setter)) return -1;

            var indexers = member.GetIndexParameters();
            var infoIsIndexer = !indexers.IsNullOrEmpty();
            if (infoIsIndexer != header.IsIndexer) return -1;
            if (infoIsIndexer) return GetParametersMass(indexers, header);
            return 0;
        }








        public SelectMethodResult SelectMethod(Type type, MethodHeader header)
        {
            var candidates = type.GetMethods(header.GetBindingsFlags());
            return new SelectMethodResult(new HeaderInitiator<MethodHeader>(type, header), FindMin(MethodFilter, header, candidates, GetRealName(type, header)));

        }
        protected virtual int MethodFilter(MethodInfo member, MethodHeader header, string realName)
        {
            if (!NameFilter(member.Name, realName)) return -1;
            return GetParametersMass(member.GetParameters(), header);
        }








        public SelectConstructorResult SelectConstructor(Type type, ConstructorHeader header)
        {
            var candidates = type.GetConstructors(header.GetBindingsFlags());
            return new SelectConstructorResult(new HeaderInitiator<ConstructorHeader>(type, header), FindMin(ConstructorFilter, header, candidates, GetRealName(type, header)));
        }

        protected virtual int ConstructorFilter(ConstructorInfo member, ConstructorHeader header, string realName)
        {
            if (header.UseNameComparer && !NameFilter(member.Name, realName)) return -1;
            return GetParametersMass(member.GetParameters(), header);
        }








        public SelectStringMemberResult SelectStringMember(Type type, StringMemberHeader header)
        {
            return new SelectStringMemberResult(new HeaderInitiator<IMemberHeader>(type,header), StringMemberFilter(type,header, GetRealName(type, header)));
        }

        protected virtual IReadOnlyList<MemberInfo> StringMemberFilter(Type type, StringMemberHeader header,string realName)
        {
            //if (type.IsConstructedGenericType)
            //{
            //    throw new ArgumentException("Type is constructed generic type", "type");
            //}
            var count = type.GetGenericArguments().Length;
            
            MemberInfo[] candidates = type.FindMembers(header.IncludingMembers, header.GetBindingsFlags(),
                   (MemberInfo member, object criteria) =>
                   {
                       if (member.MemberType == MemberTypes.Property)
                       {
                           PropertyInfo property = (PropertyInfo)member;
                           if (!property.GetIndexParameters().IsNullOrEmpty())
                               return false;
                       }
                       else
                       {
                           if (member.MemberType == MemberTypes.NestedType)
                           {
                               Type memberType = (TypeInfo)member;
                               if (memberType.IsGenericTypeDefinition && memberType.GetGenericArguments().Length-count!=0)
                                   return false;
                           }
                       }
                       return NameFilter(realName, member.Name);
                      // if (!NameFilter(realName, member.Name)) return false;
                       // return true;
                   }, null);
            return candidates;
        }








        public PLNMemberSelector(IMassCalculator massCalculator)
        {
            MassCalculator = massCalculator;
            Comparer = StringHelper.PLNComparer;
        }




        protected IReadOnlyList<TInfo> FindMin<THeader, TInfo>(MemberFilter<THeader, TInfo> filter, THeader header, IEnumerable<TInfo> candidates, string name) where THeader : IParameterMemberHeader where TInfo : MemberInfo
        {
            var lst = new List<TInfo>();
            int min = Int32.MaxValue;

            foreach (var candidate in candidates)
            {
                var m = filter(candidate, header, name);
                if (m == -1) continue;
                if (m < min)
                {
                    lst.Clear();
                    lst.Add(candidate);
                    min = m;
                }
                else
                    if (m == min)
                {
                    lst.Add(candidate);
                }
            }
            return lst;
        }



        protected virtual int GetParametersMass(ParameterInfo[] target, IParameterMemberHeader header)
        {
            if (header.Parameters.Count > target.Length) return -1;
            int totalMass = 0;
            int headerParamCount = header.Parameters.Count;
            for (int i = 0; i < headerParamCount; i++)
            {
                if (header.Parameters[i].IsOut && !target[i].IsOut) return -1;
                var mass = MassCalculator.CalculateMass(header.Parameters[i].Type, target[i].ParameterType);
                if (mass == -1) return -1;
                totalMass += mass;
            }
            for (int i = headerParamCount; i < target.Length; i++)
            {
                if (!target[i].IsOptional) return -1;
            }
            return totalMass;
        }


        private bool NameFilter(string name1, string name2)
        {
            return Comparer.Equals(name1, name2);
        }



        private string GetRealName(Type type, IMemberHeader header)
        {
            if (header.UseDefaultName)
            {
                Comparer = StringHelper.NetComparer;
                return ReflectionUtils.GetDefaultName(type);
            };
            Comparer = StringHelper.PLNComparer;
            return header.Name;
        }

  

        public IMassCalculator MassCalculator { get; private set; }
        private IEqualityComparer<string> Comparer;
        protected delegate int MemberFilter<THeader, TInfo>(TInfo Info, THeader Header, string Name) where THeader : IParameterMemberHeader where TInfo : MemberInfo;

    }
}
