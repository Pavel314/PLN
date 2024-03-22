using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic;
using PLNCompiler.Syntax;

namespace PLNCompiler.Compile.TypeManager
{
    //public class AssignConstantResult : Result<ParsableConstant>
    //{
    //    public Type TargetType { get; private set; }
    //    private TypedConstant result;

    //    public AssignConstantResult(ParsableConstant initiator, Type targetType, TypedConstant result) : base(initiator, result != null)
    //    {
    //        TargetType = targetType;
    //        this.result = result;
    //    }

    //    public TypedConstant Result
    //    {
    //        get
    //        {
    //            if (!IsSuccessful)
    //                throw new InvalidOperationException("IsSuccessfully != true");
    //            return result;
    //        }
    //    }
    //}
}
