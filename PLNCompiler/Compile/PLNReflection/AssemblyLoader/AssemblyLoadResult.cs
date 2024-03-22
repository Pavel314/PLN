using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.AssemblyLoader
{
    public enum AssemblyResults { OK, FileNotFoundException, FileLoadException, ArgumentException, BadImageFormatException,PathTooLongException,GACNotExists,InGacNotFound }
    public class AssemblyLoadResult:Result<AssemblyLoadHeader>
    {
        public AssemblyResults Result { get; private set; }
        private readonly Assembly _Assembly;
        private readonly Exception _Exception;

        public AssemblyLoadResult(AssemblyLoadHeader initiator, AssemblyResults results,Assembly assembly, Exception _exception) : base(initiator)
        {
            Result = results;
            _Assembly = assembly;
            _Exception = _exception;
            IsSuccessful = results == AssemblyResults.OK;
        }

        public AssemblyLoadResult(AssemblyLoadHeader initiator, AssemblyResults results, Exception exception) : this(initiator,results,null,exception)
        {
            if (results == AssemblyResults.OK)
                throw new ArgumentException("this is constructor for failed loading");
        }

        public AssemblyLoadResult(AssemblyLoadHeader initiator, AssemblyResults results) : this(initiator, results, null)
        {

        }

        public AssemblyLoadResult(AssemblyLoadHeader initiator, Assembly assembly) : this(initiator, AssemblyResults.OK, assembly,null)
        {
        }



        public Assembly Assembly
        {
            get
            {
                if (IsSuccessful)
                    return _Assembly;
                throw new InvalidOperationException("IsSuccessfully != true");
            }
        }

        public Exception Exception
        {
            get
            {
                if (!IsSuccessful)
                    return _Exception;
                throw new InvalidOperationException("IsSuccessfully == true");
            }
        }
    }
}
