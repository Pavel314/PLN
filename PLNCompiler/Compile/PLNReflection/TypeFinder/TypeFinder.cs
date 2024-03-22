using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{


    public sealed class TypeFinder : ITypeFinder
    {
        public const int CASH_SIZE = 100;

        public TypeFinder(IReadOnlyDictionary<TypeName, Type> typeSynonyms=null) : this(new TypeVisitor(), new NamespaceVisitor(), typeSynonyms)
        {

        }

        public TypeFinder(TypeVisitor typeVisitor, NamespaceVisitor namespaceVisitor, IReadOnlyDictionary<TypeName,Type> typeSynonyms)
        {
            //typeSynonyms = new Dictionary<TypeName, Type>
            //{
            //    {  new TypeName(new string[1]{"Целое" },0),typeof(Int32) }
            //};

            TypeTree = NamespaceNode.CreateRoot();
            Assemblies = new AssemblyCollection(this);
            Namespaces = new NamespaceCollection(this);
            NamespaceDictionary = new Dictionary<NameManagerReadOnly, NamespaceNode>(Namespaces.Comarer);
            TypeVisitor = typeVisitor;
            NamespaceVisitor = namespaceVisitor;

            if (typeSynonyms.IsNullOrEmpty())
                typeSynonyms = null;
            TypeSynonyms = typeSynonyms;
        }

        public SearchingTypeResult FindType(TypeName typeName)
        {
            //   var result = FindInCash(typeName);
            var result = FindTypeInSynonyms(typeName);

            if (result == null) result = FindInCash(typeName);
            if (result == null) result = FindFromFullName(typeName);
            if (result == null) result = FindFromNamespace(typeName);
            if (result == null) return new SearchingTypeResult(typeName, null,null);
            return result;
        }

        private SearchingTypeResult FindTypeInSynonyms(TypeName typeName)
        {
            if (TypeSynonyms != null)
            {
                if (typeName.GenericParametersCount > 0)
                {
                    if (TypeSynonyms.TryGetValue(typeName, out Type type))
                        return new SearchingTypeResult(typeName, null, new TypeAssociation[1] { new TypeAssociation(type, typeName) });
                    return null;
                }
                TypeAssociation[] typeAssociationArr = new TypeAssociation[1];
                for (int i = typeName.Count; i > 0; i--)
                {
                    TypeName newTypeName = new TypeName(typeName.Take(i),0);
                  if  (TypeSynonyms.TryGetValue(newTypeName, out Type typee))
                    {
                        typeAssociationArr[0] = new TypeAssociation(typee, newTypeName);
                        return new SearchingTypeResult(
                             typeName, TypeName.UsedGenericsIfNotEmpty(typeName.Where((f, f1) => f1 > i),0),
                             typeAssociationArr);
                    }
                }
                return null;
            }
            return null;
        }

        private SearchingTypeResult FindFromFullName(TypeName typeName)
        {
            int matches = 0;
            var types = TypeVisitor.FindType(TypeTree, typeName, out matches);
            if (types == null) return null;

            

            var result = new SearchingTypeResult(
                typeName,TypeName.UsedGenericsIfNotEmpty(typeName.Where((f, f1) => f1 >= matches),
                typeName.GenericParametersCount), types);

            AddCash(typeName, result);
            return result;
        }

        private SearchingTypeResult FindFromNamespace(TypeName typeName)
        {
            if (NamespaceDictionary.Count == 0) return null;

            int matches = 0;

            var types = new List<TypeAssociation>();

            foreach (var namepsaces in NamespaceDictionary.Values)
            {
                var findTypes = TypeVisitor.FindType(namepsaces, typeName, out int _matches);
                if (_matches >= matches)
                {
                    if (_matches > matches)
                    {
                        matches = _matches;
                        types.Clear();
                    }
                    if (!findTypes.IsNullOrEmpty())
                        types.AddRange(findTypes);
                }
            }
            if (types.IsNullOrEmpty()) return null;
            var result = new SearchingTypeResult(
                typeName, TypeName.UsedGenericsIfNotEmpty(typeName.Where((f, f1) => f1 >= matches),
                typeName.GenericParametersCount), types);

            AddCash(typeName, result);
            return result;
        }


        private SearchingTypeResult FindInCash(TypeName typeName)
        {
            SearchingTypeResult result = null;
            _Cash.TryGetValue(typeName, out result);
            return result;
        }





        internal void OnChangedAssembly(Assembly newitem, ChangedType changedType)
        {

            switch (changedType)
            {
                case ChangedType.Add:
                    _Cash.Clear();
                    TypeTreeBuilder.AddAssembly(TypeTree, newitem);
                    break;
                case ChangedType.Remove:
                    _Cash.Clear();
                    NamespaceDictionary.Clear();
                    //TODO Написать хорошую реализацию
                    TypeTree = TypeTreeBuilder.BuildTree(Assemblies);
                    foreach (var namespace_ in Namespaces)
                        AddNamespace(namespace_, true,null);
                    break;
                case ChangedType.Clear:
                    _Cash.Clear();
                    NamespaceDictionary.Clear();
                    TypeTree.Childs.Clear();
                    break;
            }
        }


        private bool AddNamespace(NameManagerReadOnly candidate, bool calledFromAssembly,Syntax.Location location)
        {
            int matches = 0;
            var candidates = NamespaceVisitor.CheckNamespace(TypeTree, candidate, out matches);

            var members = new NameManagerReadOnly(candidate.Where((f, f1) => f1 >= matches));

            if (candidates.IsNullOrEmpty())
            {
                OnErrorNamespace(new ErrorNamespaceEventArgs(new SearechingNamespaceResult(candidate, members, null), calledFromAssembly,location));
                return false;
            }
            var candidates_count = candidates.Count();
            var namespaceTree = new NamespaceNode[candidates_count];
            var namespaceManager = new NameManagerReadOnly[candidates_count];
            int i = 0;
            foreach (var namespace_ in candidates)
            {
                namespaceTree[i] = namespace_.Tree;
                namespaceManager[i] = namespace_.Namespace;
                i++;
            }

            if (candidates_count != 1 || matches != candidate.Count)
            {
                var searechingNamespaceResult = new SearechingNamespaceResult(candidate, members, namespaceManager);

                var index = OnErrorNamespace(new ErrorNamespaceEventArgs(searechingNamespaceResult, calledFromAssembly,location));
                if (index >= 0)
                {
                    NamespaceDictionary.Add(candidate, namespaceTree[index]);
                    return true;
                }
                else return false;
            }
            else
                NamespaceDictionary.Add(candidate,  namespaceTree[0] );
            return true;
        }


        internal bool OnChangeNamespace(NameManagerReadOnly item, ChangedType changedType, Syntax.Location location)
        {
            switch (changedType)
            {
                case ChangedType.Add:
                    _Cash.Clear();
                    return AddNamespace(item, false,location);
                case ChangedType.Remove:
                    _Cash.Clear();
                    NamespaceDictionary.Remove(item);
                    break;
                case ChangedType.Clear:
                    _Cash.Clear();
                    NamespaceDictionary.Clear();
                    break;
            }
            return true;
        }

        internal void OnNamespaceArleadyUsing(NamespaceArleadyUsingEventArgs namespaceArleadyUsingEventArgs)
        {
            NamespaceArleadyUsing?.Invoke(this, namespaceArleadyUsingEventArgs);
        }

        private void AddCash(TypeName key, SearchingTypeResult result)
        {
            if (_Cash.Count >= CASH_SIZE)
                _Cash.Clear();

            _Cash.Add(key, result);
        }

        private int OnErrorNamespace(ErrorNamespaceEventArgs args)
        {
            if (ErrorNamespace != null)
                return  ErrorNamespace(this, args);
            return -1;
        }


        public AssemblyCollection Assemblies { get; private set; }
        public NamespaceCollection Namespaces { get; private set; }
        public TypeVisitor TypeVisitor { get; private set; }
        public NamespaceVisitor NamespaceVisitor { get; private set; }
        public IReadOnlyDictionary<TypeName,Type> TypeSynonyms { get; private set; }

        IReadOnlyCollection<Assembly> ITypeFinder.Assemblies { get { return Assemblies; } }
        IReadOnlyCollection<NameManagerReadOnly> ITypeFinder.Namespaces { get { return Namespaces; } }

        public event ErrorNamespaceHandler ErrorNamespace;
        public event EventHandler<NamespaceArleadyUsingEventArgs> NamespaceArleadyUsing;

        private Dictionary<TypeName, SearchingTypeResult> _Cash = new Dictionary<TypeName, SearchingTypeResult>(CASH_SIZE);



        private NamespaceNode TypeTree;
        private Dictionary<NameManagerReadOnly, NamespaceNode> NamespaceDictionary;

        internal enum ChangedType { Add, Remove, Clear };
        public delegate int ErrorNamespaceHandler(object sender, ErrorNamespaceEventArgs args);

    }

    public class NamespaceArleadyUsingEventArgs:EventArgs
    {
        public NameManagerReadOnly ArleadyUsingName { get; private set; }
        public Syntax.Location Location { get; private set; }

        public NamespaceArleadyUsingEventArgs(NameManagerReadOnly arleadyUsingName, Syntax.Location location)
        {
            ArleadyUsingName = arleadyUsingName;
            Location = location;
        }
    }

    public class ErrorNamespaceEventArgs
    {
        public bool CalledBecouseAssemblyRemoved { get; private set; }
        public SearechingNamespaceResult Result { get; private set; }
        public Syntax.Location Location { get; private set; }

        public ErrorNamespaceEventArgs(SearechingNamespaceResult result, bool calledBecouseAssemblyRemoved, Syntax.Location location)
        {
            Result = result;
            CalledBecouseAssemblyRemoved = calledBecouseAssemblyRemoved;
            Location = location;
        }
    }
}
