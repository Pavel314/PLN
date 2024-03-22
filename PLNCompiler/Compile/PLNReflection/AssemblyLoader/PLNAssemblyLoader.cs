using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace PLNCompiler.Compile.PLNReflection.AssemblyLoader
{
   public class PLNAssemblyLoader:IAssemblyLoader
    {
        public PLNAssemblyLoader()
        {

        }

        public AssemblyLoadResult Load(AssemblyLoadHeader header)
        {
            AssemblyLoadResult result = null;
            if (header.LoadFrom.HasFlag(AssemblyLoadFrom.File))
                result = LoadFromFile(header);
            if (result.Result == AssemblyResults.FileNotFoundException && header.LoadFrom.HasFlag(AssemblyLoadFrom.GAC))
                result = LoadFromGAC(header);
            if (result.Result == AssemblyResults.FileNotFoundException && header.LoadFrom.HasFlag(AssemblyLoadFrom.StrongName))
                result = LoadFromStringName(header);

            if (result == null) throw new PresentVariantNotImplementedException(typeof(AssemblyLoadFrom));
            return result;
        }


        protected virtual AssemblyLoadResult LoadFromFile(AssemblyLoadHeader header)
        {
            //return LoadProcessException(header, header.Name, f => Assembly.LoadFrom(header.Name));
            var path = header.Name;
            if (path == Path.GetFileName(path))
                path = Path.Combine(header.CompilationPath, header.Name);
            return LoadProcessException(header, path, f => Assembly.LoadFrom(path));
        }

        protected virtual AssemblyLoadResult LoadFromFileNear(AssemblyLoadHeader header)
        {
            var path = header.Name;
            if (path == Path.GetFileName(path))
                path = Path.Combine(header.CompilationPath, header.Name);
            return LoadProcessException(header, path, f => Assembly.LoadFrom(path));
        }

        protected virtual AssemblyLoadResult LoadFromStringName(AssemblyLoadHeader header)
        {
            return LoadProcessException(header, header.Name, f => Assembly.Load(header.Name));
        }


        protected virtual AssemblyLoadResult LoadFromGAC(AssemblyLoadHeader header)
        {
            if (!AttemptToLoadGAC)
            {
                InitGAC();
                AttemptToLoadGAC = true;
            }

            if (GAC.IsNullOrEmpty())
                return new AssemblyLoadResult(header, AssemblyResults.GACNotExists,null);

            var name = header.Name;

            if (GAC.TryGetValue(name, out string longName))
            {
                return LoadProcessException(header, longName, f => Assembly.Load(longName));
            }

            name = Path.GetFileNameWithoutExtension(name);

            if (GAC.TryGetValue(name, out longName))
            {
                return LoadProcessException(header, longName, f => Assembly.Load(longName));
            }
            return new AssemblyLoadResult(header,AssemblyResults.InGacNotFound);
        }

        private AssemblyLoadResult LoadProcessException(AssemblyLoadHeader header,string path,Func<string, Assembly> loader)
        {
            try
            {
                return new AssemblyLoadResult(header,loader(path));
            }
            catch (FileLoadException e)
            {
                return new AssemblyLoadResult(header, AssemblyResults.FileLoadException, e);
            }
            catch (FileNotFoundException e)
            {
                return new AssemblyLoadResult(header, AssemblyResults.FileNotFoundException, e);
            }
            catch (BadImageFormatException e)
            {
                return new AssemblyLoadResult(header, AssemblyResults.BadImageFormatException, e);
            }
            catch (ArgumentException e)
            {
                return new AssemblyLoadResult(header, AssemblyResults.ArgumentException, e);
            }
            catch (PathTooLongException e)
            {
                return new AssemblyLoadResult(header, AssemblyResults.PathTooLongException, e);
            }
        }

        protected virtual void InitGAC()
        {
            try
            {
                GAC = XElement.Load(GACGenerator.FILE_NAME).Descendants(GACGenerator.PAIR).ToDictionary(
                    f => (string)f.Attribute(GACGenerator.SHORT_NAME),
                    f => (string)f.Attribute(GACGenerator.LONG_NAME),
                    GACGenerator.FileSystemComparer);
            }
            catch (Exception)
            {
                GAC = null;
            }


        }

        protected Dictionary<string, string> GAC;
        private bool AttemptToLoadGAC = false;
    }
}
