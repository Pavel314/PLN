//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PLN.Compile.TypeManager.NativeConversion
//{
//    public class PLNNativeTypeComparer: NativeTypeComparer //: IComparer<Type>
//    {


//        public int? Compare(Type left, Type right)//left>right +| left<right-
//        {
//            Int64 a = Int64.MaxValue;
//            float b = a;
//            if (NativeTypeCodes.Get(left, out NativeTypeInfo l))
//            {
//                if (NativeTypeCodes.Get(left, out NativeTypeInfo r))
//                {
//                    if (l.Kind == NativeTypeKind.Sign)
//                    {
//                        switch (r.Kind)
//                        {
//                            case NativeTypeKind.Sign:return l.Size.CompareTo(r.Size);
//                            case NativeTypeKind.UnSign:return null;
//                            case NativeTypeKind.Float:return
//                        }
//                    }
//                    if (l.Kind==NativeTypeKind.Sign)
//                    {
//                        if (r.Kind == NativeTypeKind.Sign)
//                        {
//                            return 
//                        }
//                        if (r.Kind == NativeTypeKind.UnSign)
//                        {
//                            return null;
//                        }
//                        }
//                }
//                return null;
//            }
//            return null;
//        }
//    }
//}
