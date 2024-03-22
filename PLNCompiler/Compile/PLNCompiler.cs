using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic;
using PLNCompiler.Semantic.SemanticTree;
using PLNCompiler.Syntax;
using PLNCompiler.Syntax.Analysis;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Syntax.TreeConverter;
using PLNCompiler.Compile.CodeGeneration;
using QUT.Gppg;
using System.Reflection;
using System.Reflection.Emit;

namespace PLNCompiler.Compile
{
    public class PLNCompiler : ICompiler
    {
        public CompilationResult CompileFromFile(ComilerSettings settings,string soureceCodePath)
        {
            //Settings.ProgramPath = Path.GetFileNameWithoutExtension(path);
            using (var sourceCodeStream = new StreamReader(soureceCodePath, StringHelper.Encoding))
            {
                return CompileFromStream(settings,sourceCodeStream.BaseStream, sourceCodeStream.CurrentEncoding.CodePage);
            }
        }

        public CompilationResult CompileFromStream(ComilerSettings settings,Stream stream,int fallbackCodePage)
        {
            scanner.SetSource(stream, fallbackCodePage);
            try
            {
                parser.Parse();
            }
            catch (Errors.SyntaxError error)
            {
                errorsCreator.SyntaxErrors.Add(error);
            }

            if (!errorsCreator.HasErrors())
                return CompileFromSyntaxTree(settings,parser.Root);

            return new CompilationResult(1, errorsCreator.Export(), null, null);
        }

        public CompilationResult CompileFromSyntaxTree(ComilerSettings settings,ProgramNode programNode)
        {
            SemanticMain semanticMain = null;
            PLNTreeConverter treeConverter = null;
            try
            {
                //   PLNTreeConverter treeConverter = new PLNTreeConverter(programNode);
                treeConverter = PLNTreeConverter.FromProgramNode(programNode,settings.DirectoryPath); 
                semanticMain= treeConverter.GenerateSemanticTree(programNode.MainBlock);
            }
            catch (Errors.SemanticError error)
            {
                errorsCreator.SemanticErrors.Add(error);
            }

            if (!errorsCreator.HasErrors())
            {
                // var domian = AppDomain.CreateDomain("PLNDomian",new System.Security.Policy.Evidence(),,,);
                // var assembly = domian.DefineDynamicAssembly(new AssemblyName("PLNAssembly"), AssemblyBuilderAccess.Save);
                var assemblyName = new AssemblyName(settings.ShortNameWithoutExtension);
                var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, settings.AssemblyAccess,settings.DirectoryPath);
                var moduleSettings = new ModuleSetting(settings.ShortName, settings.ShortName, settings.DebugMode, settings.MainClassName, settings.MainMethodName);
                var moduleResult = codeGenerator.GenerateModule(assembly, moduleSettings, semanticMain.Main);
                assembly.SetEntryPoint(moduleResult.MainMethod, DirectiveInfo.ToPEFileKinds( semanticMain.DirectiveInfo.ApplicationKind));
                assembly.Save(settings.ShortName);

               if (treeConverter.DirectiveInfo.UseSystemLibrary)
                {
                    var currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    //Check Assembly version if not overrite
                    if (!settings.DirectoryPath.IsNullOrEmpty() && currentPath != settings.DirectoryPath)
                        File.Copy(PLNTypeGenerator.PLNDefaultLib, Path.Combine(settings.DirectoryPath, PLNTypeGenerator.PLNDefaultLib), true);
                }
             
                return CompilationResult.CreateSuccesful(assembly);
            }
            return new CompilationResult(2, errorsCreator.Export(), null, null);
        }

        public PLNCompiler()
        {
            codeGenerator = new PLNCodeGenerator();
            errorsCreator = new CompilationErrorsCreator();
            scanner = new PLNScanner();
            scanner.ScanError += Scanner_ScanError;
            parser = new PLNParser(scanner);
          // Settings = new ComilerSettings(debugMode,programName);
        }

        private bool Scanner_ScanError(object sender, ScanErrorEventArgs e)
        {
            errorsCreator.LexicalErrors.Add(new Errors.LexicalErrors.IllicitChar(e.Text,e.Location));
            return true;//TODO return true or false не оставливает лексический разбор
        }

       // public ComilerSettings Settings { get; private set; }

        private PLNCodeGenerator codeGenerator;
        private CompilationErrorsCreator errorsCreator;
        private PLNScanner scanner;
        private PLNParser parser;
    }
}
