using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using PLNCompiler.Syntax.Analysis;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Compile.Errors;
using PLNCompiler.Semantic;
using PLNCompiler.Syntax;
using PLNCompiler.Compile.PLNReflection.TypeFinder;
using PLNCompiler.Compile.PLNReflection;
using PLNCompiler.Syntax.TreeConverter;
using PLNCompiler.Syntax.Optimization;
using PLNCompiler.Compile;

namespace PLNCompiler
{

    //
    //public class TreeConverter : IStatementVisitor, IExpressionVisitor
    //{

    //    public TreeConverter(PLNTypeGenerator typeGenerator)
    //    {
    //        TypeGenerator = typeGenerator;
    //    }


    //    public void VisitBlockNode(BlockNode node)
    //    {
    //        foreach (var statement in node.Statements)
    //            statement.Visit(this);
    //    }

    //    public void VisitConstantNode(ConstNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitIdentiferNode(IdentiferNode node)
    //    {
    //        throw new NotImplementedException();
    //    }



    //    public void VisitParsableConstantNode(ParsableConstNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitMethodNode(MethodNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitVarAssignNode(VarAssignNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitVarDefineNode(VarDefineNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitBinaryNode(BinaryNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitUnaryNode(UnaryNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitMemberAccessNode(MemberAccessNode node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public PLNTypeGenerator TypeGenerator { get; private set; }

    //}



    public class Program
    {
        private static bool Scanner_ScanError(object sender, ScanErrorEventArgs e)
        {
            //  var a = new tst<int>();
            // mem();
            Console.WriteLine(e.Text.ToString());
            return false;
        }




        static void Main(string[] args)
        {
            if (args.IsNullOrEmpty())
                args = new string[1] { "PLNProgram.txt" };

            CompileWithConsoleIO(args[0]);
            ////   Int32.MaxValue();
            ////.MakeGenericType(typeof(int))
            ////    Console.WriteLine(typeof(Tests.NesType<int>).Name);
            ////NestedTypeFinder.FindNestedType(typeof(PLN.Tests.NesType<int>), new TypeName(new string[] {"NesType2", "NesType22", "NesType221", "asd" },0));
            ////var a = NestedTypeFinder.Matches;
            ////Console.ReadLine();
            ////System.Environment.Exit(0);
            //string fileName = "PLNProgram.txt";

            //Console.OutputEncoding = StringHelper.Encoding;
            //var compiler = new PLNCompiler(false);
            //var result = compiler.CompileFromFile(fileName);

            //if (!result.IsSuccessful)
            //{
            //    Console.WriteLine(File.ReadAllText(fileName));

            //    if (!result.Errors.LexicalErrors.IsNullOrEmpty())
            //    {
            //        Console.WriteLine("\nБыли обнаружены лексические ошибки:");
            //        foreach (var error in result.Errors.LexicalErrors)
            //            AddError(error.Message, error.Location);
            //    }

            //    if (!result.Errors.SyntaxErrors.IsNullOrEmpty())
            //    {
            //        Console.WriteLine("\nБыли обнаружены синтаксическме ошибки:");
            //        foreach (var error in result.Errors.SyntaxErrors)
            //            AddError(error.Message, error.Location);
            //    }

            //    if (!result.Errors.SemanticErrors.IsNullOrEmpty())
            //    {
            //        Console.WriteLine("\nБыли обнаружены семантические ошибки:");
            //        foreach (var error in result.Errors.SemanticErrors)
            //            AddError(error.Message, error.Location);
            //    }
            //}


            ////PLNScanner scanner = new PLNScanner();
            ////scanner.ScanError += Scanner_ScanError;
            ////using (var sourceCodeStream = new StreamReader("Program.txt", StringHelper.Encoding))
            ////{
            ////    scanner.SetSource(sourceCodeStream.BaseStream, sourceCodeStream.CurrentEncoding.CodePage);
            ////    var parser = new PLNParser(scanner);
            ////    PLNTypeGenerator typeVisitor = null;
            ////    try
            ////    {
            ////       //System.CodeDom.Compiler.CodeCompiler
            ////        var isParsable = parser.Parse();
            ////        //         var a = parser.Root;

            ////        //typeVisitor = new PLNTypeGenerator(parser.Root);
            ////        // typeVisitor.GenerateType(((VarDefineNode)parser.Root.MainBlock.Statements[0]).TypeNode);
            ////        ITreeConverter treeConverter = new PLNTreeConverter(parser.Root);
            ////       var semanticTree= treeConverter.ConvertToSemanticTree(parser.Root.MainBlock);
            ////        var a = semanticTree;
            ////        // 
            ////        //var a = NestedTypeFinder.FindNestedType(typeof(PLN.Tests.NesType<int>), new TypeName(new string[] { "NesType2", "NesType21", "aa" }, 1));
            ////        //var b = a;
            ////        //  parser.Root.MainBlock.Visit(new TreeConverter(new TypeGeneratorVisitor(parser.Root)));
            ////        //typeVisitor.GenerateType(((VarArrayDefineNode)parser.Root.MainBlock.Statements[0]).TypeNode, GenericContext.Type);
            ////        // Console.WriteLine(typeVisitor.GenerateType(((VarDefineNode) parser.Root.MainBlock.Statements[0]).TypeNode,TypeGenerateMode.FindTypeOnly));
            ////    }
            ////    catch (SourceCodeException e)
            ////    {
            ////        Console.WriteLine(e.ErrorCode.ToString());
            ////        Console.WriteLine(File.ReadAllText("Program.txt"));
            ////        var pos = Console.CursorTop;

            ////        Console.SetCursorPosition(e.Location.StartColumn, e.Location.StartLine + 1);
            ////        Console.ForegroundColor = ConsoleColor.Red;
            ////        Console.WriteLine("━");
            ////        Console.ForegroundColor = ConsoleColor.Gray;
            ////        Console.SetCursorPosition(0, pos + 1);
            ////        Console.WriteLine(e.Message);
            ////    }
            ////}
            ////var v = new VisText();
            ////var finder = new TypeFinder();// TypeTreeBuilder.BuildTree(new Assembly[] { Assembly.Load("mscorlib.dll") });
            ////finder.ErrorNamespace += T_ErrorNamespace;
            ////finder.Assemblies.Add(Assembly.Load("mscorlib.dll"));
            ////finder.Assemblies.Add(Assembly.LoadFile(@"C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll"));
            ////finder.Namespaces.Add(new NameManagerReadOnly("System.Collections.Generic", StringHelper.NetTypeDelimiter));
            ////finder.Namespaces.Add(new NameManagerReadOnly("System", StringHelper.NetTypeDelimiter));

            ////v.Finder = finder;
            ////v.Verify = new GenericVerify();

            ////v.FindType(a);
            ////a.Visit(v);
            //// var b = a;
            //// Tokens tok = 0;
            ////do
            ////{
            ////    tok = (Tokens) scanner.yylex();

            ////        if (tok == Tokens.EOF) break;
            ////    //    Console.WriteLine(scanner.TokenToString(tok));
            ////    Console.WriteLine(tok);
            ////} while (true);

            //// }
            //Console.ReadLine();
        }

        public static void CompileWithConsoleIO(string fileName)
        {
            var compiler = new Compile.PLNCompiler();
            OutputResult(compiler.CompileFromFile(new ComilerSettings(false, Path.ChangeExtension(fileName, "exe")), fileName));
        }

        //public static void CompileWithConsoleIOMemory(string soureCode,string programName="PLNProgram")
        //{
        //    var compiler = new PLNCompiler();
        //    using (var stream = new MemoryStream(StringHelper.Encoding.GetBytes(soureCode)))
        //    {
        //        OutputResult(compiler.CompileFromStream(new ComilerSettings(false, programName fileName) stream, StringHelper.Encoding.CodePage));
        //    }
        //}

        private static void OutputResult(CompilationResult result)
        {
            Console.OutputEncoding = StringHelper.Encoding;

            if (!result.IsSuccessful)
            {
                if (!result.Errors.LexicalErrors.IsNullOrEmpty())
                {
                    Console.WriteLine("\nБыли обнаружены лексические ошибки:");
                    foreach (var error in result.Errors.LexicalErrors)
                        AddError(error.Message, error.Location);
                }

                if (!result.Errors.SyntaxErrors.IsNullOrEmpty())
                {
                    Console.WriteLine("\nБыли обнаружены синтаксическме ошибки:");
                    foreach (var error in result.Errors.SyntaxErrors)
                        AddError(error.Message, error.Location);
                }

                if (!result.Errors.SemanticErrors.IsNullOrEmpty())
                {
                    Console.WriteLine("\nБыли обнаружены семантические ошибки:");
                    foreach (var error in result.Errors.SemanticErrors)
                        AddError(error.Message, error.Location);
                }
                Console.ReadKey();
            }
        }

        private static void AddError(string error, Location location)
        {
            //var posLeft = Console.CursorLeft;
            //var posTop = Console.CursorTop;

            Console.WriteLine(string.Format("[{0},{1}] {2}", location.StartColumn + 1, location.StartLine, error));

            //Console.SetCursorPosition(error.Location.StartColumn, error.Location.StartLine-1);
            //   Console.Write('—');
        }

    }
}
