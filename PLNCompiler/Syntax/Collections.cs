using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Semantic;

namespace PLNCompiler.Syntax
{
    public class UsingCollection : List<SyntaxTree.UsingNode>
    {
        public UsingCollection() { }
    }
    public class ImportCollection : List<ImportNode>
    {
        public ImportCollection() { }
    }

    public class StatementCollection : List<StatementNode>
    {
        public StatementCollection() { }
    }

    public class TypeCollection : List<TypeNode>
    {
        public TypeCollection() { }
    }

    public class DirectiveCollection : List<DirectiveNode>
    {

    }

    public class IdentiferCollection : List<IdentiferNode>
    {
        public IdentiferCollection() { }
        public NameManagerReadOnly GetNameManager()
        {
            var manager = new NameManager();
            foreach (var identifer in this)
                manager.Add(identifer.Name);
            return manager;
        }
    }

    public class ExpressionCollection : List<ExpressionNode>
    {
        public ExpressionCollection() { }
    }


    public class MethodArgumentsCollection : List<FactArgumentNode>
    {
        public MethodArgumentsCollection() { }
        public MethodArgumentsCollection(FactArgumentNode first) { Add(first); }
    }

    public class IndexerArgumentsCollection : List<ExpressionArgumentNode>
    {
        public IndexerArgumentsCollection() { }
        public IndexerArgumentsCollection(ExpressionArgumentNode first) { Add(first); }
    }

    public class WaitLabelsDictionary : Dictionary<string, WaitLabelsDictionary.LabelInfo>
    {

        public WaitLabelsDictionary():base(StringHelper.PLNComparer)
        {

        }

        public void AddUniqueRange(WaitLabelsDictionary items)
        {
            foreach (var item in items)
            {
                if (TryGetValue(item.Key, out LabelInfo labelInfo))
                    labelInfo.Location.AddRange(item.Value.Location);
                else
                    Add(item.Key, item.Value);
            }
        }

        public LabelWrapper TryAdd(string name, Location location)
        {
            if (TryGetValue(name, out LabelInfo labelInfo))
            {
                labelInfo.Location.Add(location);
                return labelInfo.Label;
            }
            else
            {
                LabelWrapper label = new LabelWrapper(name);
                Add(name, new LabelInfo(label, location));
                return label;
            }
        }

        public bool TryRemove(string name,out LabelWrapper result)
        {
            if (TryGetValue(name,out LabelInfo labelInfo))
            {
                Remove(name);
                result = labelInfo.Label;
                return true;
            }
            result = default(LabelWrapper);
            return false;
        }


        public struct LabelInfo
        {
            public readonly LabelWrapper Label;
            public readonly List<Location> Location;

            public LabelInfo(LabelWrapper label, Location location)
            {
                Label = label;
                Location = new List<Location>
                {
                    location
                };
            }
        }
    }

}
