using System;
using System.Collections.Generic;
using System.Linq;
using PLNCompiler.Compile.Errors;
using PLNCompiler.Syntax.SyntaxTree;
using System.Reflection;
using PLNCompiler.Syntax;
using PLNCompiler.Compile.PLNReflection.TypeFinder;
using PLNCompiler.Compile.PLNReflection.AssemblyLoader;
using PLNCompiler.Compile.PLNReflection;

namespace PLNCompiler.Syntax.TreeConverter
{
    //public enum GenericContext {Type,Method }

    //public class TypeGeneratorResult:Result<TypeNode>
    //{
    //    public IReadOnlyCollection<Type> GenericTypes { get; private set; }
    //    public NameManagerReadOnly Member { get; private set; }
    //    public GenericContext Context { get; private set; }

    //    public TypeGeneratorResult(TypeNode initiator,NameManagerReadOnly member, GenericContext context, IReadOnlyCollection<Type> genericTypes):base(initiator)
    //    {
    //        if (member.IsNullOrEmpty()) member = null;
    //        if (genericTypes.IsNullOrEmpty()) genericTypes = null;

    //        Member = member;
    //        GenericTypes = genericTypes;
    //        Context = context;

    //        switch (context)
    //        {
    //            case GenericContext.Type: IsSuccessful = (member == null && genericTypes.Count == 1);break;
    //            case GenericContext.Method: IsSuccessful = (member != null && genericTypes.Count>0); break;
    //        }
    //    }
    //}

    public sealed class PLNTypeGenerator : ITypeGenerator,ITypeVisitor
    {
        public static readonly AssemblyName MSCORLIB = new AssemblyName("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
        public static readonly AssemblyName SYSTEM = new AssemblyName("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
        public static readonly AssemblyName SYSTEM_NUMERICS = new AssemblyName("System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
        public const string PLNDefaultLib = "PLNLib.dll";
        public const string PLNDefaultLibNS = "PLNLib";

        //private void MakeAllGeneric(TypeNameNode node)
        //{
        //    int genericCount = node.GetGenerisCount();
        //    if (genericCount > 0)
        //    {
        //        Type[] genericArguments = new Type[genericCount];
        //        for (int i = genericCount - 1; i >= 0; i--)
        //            genericArguments[i] = Stack.Pop();

        //        var target = Stack.Pop();
        //        var verifyResult = GenericVerify.Verify(target, genericArguments);

        //        if (!verifyResult.IsSuccessful)
        //        {
        //            throw SemanticErrors.Get(node.Location, verifyResult);
        //        }
        //        Stack.Push(target.MakeGenericType(genericArguments));
        //        NameManager = new NameManager();
        //    }
        //}


        //private void VisitAllGenerics(TypeNameNode node)
        //{
        //    if (node.Generics.IsNullOrEmpty()) return;
        //    foreach (var generic in node.Generics)
        //        generic.Visit(this);
        //}


        //private void MakeType(TypeNameNode node)
        //{
        //    if (!NameManager.IsNullOrEmpty())
        //    {
        //        SearchingTypeResult searchingTypeResult = TypeFinder.FindType(new TypeName(NameManager, node.GetGenerisCount())).ExclusiveWithNetComparer();
        //        Type type = null;

        //        if (searchingTypeResult.IsSuccessful)
        //            type = searchingTypeResult.FoundResult.Value;
        //        else
        //            if (searchingTypeResult.Result == Compiler.SearchingResults.OK && searchingTypeResult.WithMember)
        //        {
        //            var findNestedResult = NestedTypeFinder.FindNestedType(
        //                searchingTypeResult.Initiator,
        //                searchingTypeResult.Candidates.First().Value,
        //                new TypeName(searchingTypeResult.Member, node.GetGenerisCount())
        //                ).ExclusiveWithNetComparer();

        //            if (findNestedResult.IsSuccessful)
        //                type = findNestedResult.FoundResult.Value;
        //            else
        //                throw SemanticErrors.Get(node.Location, findNestedResult);
        //        }

        //        else
        //            throw SemanticErrors.Get(node.Location, searchingTypeResult);

        //        Stack.Push(type);
        //        NameManager = new NameManager();
        //    }
        //}



        private Type FindType(TypeName typeName,TypeNameNode node)
        {
            SearchingTypeResult searchingTypeResult = TypeFinder.FindType(new TypeName(NameManager, node.GetGenerisCount())).ExclusiveWithNetComparer();
            Type type = null;

            if (searchingTypeResult.IsSuccessful)
                type = searchingTypeResult.FoundResult.Type;
            else
                if (searchingTypeResult.Result == Compile.SearchingResults.OK && searchingTypeResult.WithMember)
            {
                var findNestedResult = NestedTypeFinder.FindNestedType(
                    new TypeAssociation(
                        searchingTypeResult.Candidates.First().Type, 
                        searchingTypeResult.Initiator),
                    
                    new TypeName(searchingTypeResult.Member, 
                        node.GetGenerisCount())
                    ).ExclusiveWithNetComparer();

                if (findNestedResult.IsSuccessful)
                    type = findNestedResult.FoundResult.Type;
                else
                    throw SemanticErrors.Get(node.Location, findNestedResult);
            }

            else
                throw SemanticErrors.Get(node.Location, searchingTypeResult);
            return type;
        }


        public void VisitTypeNameNode(TypeNameNode node)
        {
            NameManager.Add(node.Identifer.Name);

            if (node.Child != null && node.GetGenerisCount() == 0)
            {
                node.Child.Visit(this);
            }
            else
            {
                nestedDeep++;
                Type type = null;
                TypeName typeName = null;

                if (findInNested)
                {
                    var parentType = stack.Pop();
                    var genericsCount = node.GetGenerisCount();
                    typeName = new TypeName(parentType.Name.Concat(NameManager), genericsCount);
                    var findNestedResult = NestedTypeFinder.FindNestedType(parentType, new TypeName(NameManager, genericsCount)).ExclusiveWithNetComparer();
                    if (findNestedResult.IsSuccessful)
                        type = findNestedResult.FoundResult.Type;
                    else
                        throw SemanticErrors.Get(node.Location, findNestedResult);
                    findInNested = false;
                }
                else
                {
                    typeName = new TypeName(NameManager, node.GetGenerisCount());
                    type = FindType(typeName, node);
                }
                stack.Push(new TypeAssociation(type, typeName));
                NameManager.Clear();

                if (node.Child != null)
                {
                    findInNested = true;
                    node.Child.Visit(this);
                    findInNested = false;

                }
                nestedDeep--;
            }

            int genericCount = node.GetGenerisCount();

            if (genericCount > 0)
            {
                for (int i = genericCount - 1; i >= 0; i--)
                {
                    node.Generics[i].Visit(this);
                }


                if (nestedDeep == 0)
                {
                    List<Type> genericArguments = new List<Type>(genericCount);
                    lab:
                    genericArguments.Add(stack.Pop().Type);

                    if (!stack.Peek().Type.IsGenericTypeDefinition)
                        goto lab;

                    var target = stack.Pop().Type;
                    var verifyResult = GenericVerify.Verify(target, genericArguments);

                    if (!verifyResult.IsSuccessful)
                    {
                        throw SemanticErrors.Get(node.Location, verifyResult);
                    }
                    stack.Push(new TypeAssociation(target.MakeGenericType(genericArguments.ToArray()),null));
                    // Stack.Push(new Pair<Type, TypeName>(target.MakeGenericType(genericArguments.ToArray()),new TypeName(NameManager,genericCount)));
                    //NameManager = new NameManager();
                }
            }


        




            ////========== (Add new identifer)==========
            // NameManager.Add(node.Identifer.Name);
            ////========================================

            //if (node.Child != null && node.GetGenerisCount() == 0)
            //{
            //    node.Child.Visit(this);
            //}

            ////==========(Find and make current type from "NameManager")==========
            //MakeType(node);
            ////===================================================================


            ////==========(Visit All Generics)==========
            //VisitAllGenerics(node);
            ////========================================


            ////==========(Verify and make all generics)==========
            //MakeAllGeneric(node);
            ////==================================================

        }


        public void VisitArrayTypeNode(ArrayTypeNode node)
        {
            node.ArrayChild.Visit(this);

            Type arrayType = stack.Pop().Type;

            //if (node.DimensionCount != 1)
            //   arrayType = arrayType.MakeArrayType(node.DimensionCount);
            //else
            //    arrayType = arrayType.MakeArrayType();
            arrayType = arrayType.MakePLNArrayType(node.DimensionCount);

            stack.Push(new TypeAssociation(arrayType, null));
        }

        public Type GenerateType(TypeNode typeNode)
        {
            Reset();
            typeNode.Visit(this);
            return stack.Pop().Type;
        }

        public PLNTypeGenerator(DirectiveInfo directiveInfo,ImportsNode importSection, UsingsNode usingSection, GenericVerify genericVerify, IAssemblyLoader assemblyLoader)
        {
            DirectiveInfo = directiveInfo;

            AssemblyLoader = assemblyLoader;

            TypeFinder = new TypeFinder(StringHelper.PLNDefualtTypes);

            TypeFinder.ErrorNamespace += TypeFinder_ErrorNamespace;
            TypeFinder.NamespaceArleadyUsing += TypeFinder_NamespaceArleadyUsing;
            GenericVerify = genericVerify;
            stack = new Stack<TypeAssociation>();
            NameManager = new NameManager();
            Reset();

            if (importSection != null)
                importSection.Visit(this);

            TypeFinder.Assemblies.Add(Assembly.Load(MSCORLIB));
            TypeFinder.Assemblies.Add(Assembly.Load(SYSTEM));
            TypeFinder.Assemblies.Add(Assembly.Load(SYSTEM_NUMERICS));

            if (directiveInfo != null)
            {
                if (directiveInfo.UseSystemLibrary)
                {
                    TypeFinder.Assemblies.Add(Assembly.LoadFrom(PLNDefaultLib));
                    TypeFinder.Namespaces.Add(new NameManagerReadOnly(new string[1] { PLNDefaultLibNS }), new Location(0, 0, 0, 0));
                }
            }

            if (usingSection != null)
                usingSection.Visit(this);

        }



        public PLNTypeGenerator(ProgramNode programNode,DirectiveInfo directiveInfo) : this(directiveInfo,programNode.ImportSection, programNode.UsingSection, new GenericVerify(), new PLNAssemblyLoader())
        {

        }

        private void Reset()
        {
            findInNested = false;
            nestedDeep = 0;
            stack.Clear();
            NameManager.Clear();
        }




        public void VisitImportsNode(ImportsNode node)
        {

            foreach (var importNode in node.Imports)
                importNode.Visit(this);
        }

        public void VisitImportNode(ImportNode node)
        {
            var name = node.Name.TrimFLChar();
#if DEBUG
            if (name == "this")
            {
                TypeFinder.Assemblies.Add(Assembly.GetExecutingAssembly());
                return;
            }
#endif
            var assemblyLoadResult = AssemblyLoader.Load(new AssemblyLoadHeader(name,DirectiveInfo.CompilationPath));

            if (!assemblyLoadResult.IsSuccessful)
            {
                throw SemanticErrors.Get(node.Location, assemblyLoadResult);
            }

            var assembly = assemblyLoadResult.Assembly;
            if (!TypeFinder.Assemblies.Add(assembly))
            {
                throw new SemanticErrors.ReferenceArleadyImport(node.Location, assembly);
            }
        }

        public void VisitUsingsNode(UsingsNode node)
        {
            foreach (var usingNode in node.Usings)
                usingNode.Visit(this);
        }

        public void VisitUsingNode(UsingNode node)
        {
            TypeFinder.Namespaces.Add(node.Namespace.GetNameManager(), node.Location);
        }

        private int TypeFinder_ErrorNamespace(object sender, ErrorNamespaceEventArgs args)
        {
            if (args.Result.Result == Compile.SearchingResults.Suspense && !args.Result.WithMember)
            {
                var result = args.Result.ExclusiveWithNetComparer();
                if (result >= 0) return result;
            }
            throw SemanticErrors.Get(args.Location, args.Result);
        }

        private void TypeFinder_NamespaceArleadyUsing(object sender, NamespaceArleadyUsingEventArgs e)
        {
          
            throw SemanticErrors.Get(e);
        }

        ITypeFinder ITypeGenerator.TypeFinder { get { return TypeFinder; } }
        public DirectiveInfo DirectiveInfo { get; private set; }
        public GenericVerify GenericVerify { get; private set; }
        private TypeFinder TypeFinder;
        private IAssemblyLoader AssemblyLoader;
        private NameManager NameManager;
        private Stack<TypeAssociation> stack;
        private bool findInNested;
        private int nestedDeep;

    }
}
