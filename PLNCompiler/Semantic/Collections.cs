#define StrongTypeControl

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic.SemanticTree;
using PLNCompiler.Semantic;
using PLNCompiler.Compile.TypeManager;

namespace PLNCompiler.Semantic
{
    public class StatementCollection: AddOnlyList<StatementNode>
    {
       public StatementCollection()
        {
        }

        public void Add(VarDefineNode item)
        {
            base.Add(item);
            VariablesCount += item.Variables.Count;
        }

        public void Add(LabelDefineNode item)
        {
            base.Add(item);
            LabelsCount++;
        }

#if (StrongTypeControl || DEBUG)
        public override void Add(StatementNode item)
        {
            base.Add(item);
            var varDefNode = item as VarDefineNode;
            if (!ReferenceEquals(varDefNode, null))
                VariablesCount += varDefNode.Variables.Count;
            else
            {
                if (item is LabelDefineNode)
                    LabelsCount++;
            }
        }
#endif



        public int VariablesCount { get; private set; }
        public int LabelsCount { get; private set; }

        //public override void Clear()
        //{
        //    base.Clear();
        //    VariablesCount = 0;
        //}

        //public void Add(VarDefineNode varDefine)
        //{
        //    base.Add(varDefine);
        //    VariablesCount += varDefine.Variables.Count;
        //}

        //public void Insert(int index, VarDefineNode item)
        //{
        //    base.Insert(index, item);
        //    VariablesCount += item.Variables.Count;
        //}

        //public bool Remove(VarDefineNode item)
        //{
        //    VariablesCount -= item.Variables.Count;
        //    return base.Remove(item);
        //}


        //#if (VariablesCountStrongControl || DEBUG)
        //public override void Add(SemanticNode item)
        //{
        //    base.Add(item);
        //    var varDefNode = item as VarDefineNode;
        //    if (!ReferenceEquals(varDefNode, null))
        //        VariablesCount += varDefNode.Variables.Count;
        //}

        //public override void Insert(int index, SemanticNode item)
        //{
        //    base.Insert(index, item);
        //    var varDefNode = item as VarDefineNode;
        //    if (!ReferenceEquals(varDefNode, null))
        //        VariablesCount += varDefNode.Variables.Count;
        //}

        //public override bool Remove(SemanticNode item)
        //{
        //    var varDefNode = item as VarDefineNode;
        //    if (!ReferenceEquals(varDefNode, null))
        //        VariablesCount -= varDefNode.Variables.Count;
        //    return base.Remove(item);
        //}

        //public override void RemoveAt(int index)
        //{
        //    base.RemoveAt(index);
        //    var varDefNode =this[index] as VarDefineNode;
        //    if (!ReferenceEquals(varDefNode, null))
        //        VariablesCount -= varDefNode.Variables.Count;
        //}
        //#endif
    }



    public class VariableCollection : AddOnlyList<LocalWrapper>
    {
     //   public VarDefineNode Parent { get; private set; }
        //VarDefineNode parent

        public VariableCollection()
        {
       //     Parent = parent;
        }

        //public new int Add(IdentiferNode identifer)
        //{
        //    base.Add(identifer);
        //    return Count - 1;
        //}
    }



    public class ExpressionCollection:AddOnlyList<TypedExpression>
    {
        public ExpressionCollection()
        {
        }

        public ExpressionCollection(IEnumerable<TypedExpression> items):base(items)
        {

        }
    }

}
