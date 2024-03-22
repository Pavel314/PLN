using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree
{
    public sealed class NamespaceResult
    {
        public NameManagerReadOnly Namespace { get; private set; }
        public NamespaceNode Tree { get; private set; }
        public NamespaceResult(NameManagerReadOnly namespace_, NamespaceNode tree)
        {
            Namespace = namespace_;
            Tree = tree;
        }
    }

    public class NamespaceVisitor : ITypeTreeVisitor
    {
        public const bool NAMESPACE_SUSPENSE_IS_ENABLED = true;

        public NamespaceVisitor() { }

        public IEnumerable<NamespaceResult> CheckNamespace(NamespaceNode tree, NameManagerReadOnly findNamespace, out int matches)
        {
            _ResetFields(findNamespace);
            tree.Visit(this);
            matches = _MatchesMax;
            if (_Matches.Count == 0) return null;

            return _Matches;
        }

        public void VisitNamespaceNode(NamespaceNode node)
        {
            if (_IsEndOfSearech) return;
            if (_deph > 0) _CurrentNamespace.Add(node.Name);

            if (_deph < _FindNamespace.Count)
            {
                foreach (var child in node.Childs)
                {
                    _deph++;
                    if (StringHelper.PLNComparer.Equals(child.Name, _FindNamespace[_index]))
                    {
                        _index++;
                        child.Visit(this);
                        _index--;
                    }
                    _deph--;
                }
            }
            if (_CurrentNamespace.Count > _MatchesMax)
            {
                _MatchesMax = _CurrentNamespace.Count;
                _Matches.Clear();
                _Matches.Add(new NamespaceResult(_CurrentNamespace.CloneAs(), node));
                if (!NAMESPACE_SUSPENSE_IS_ENABLED)
                    _IsEndOfSearech = true;
            }
            else
                if (_MatchesMax > 0 && _CurrentNamespace.Count == _MatchesMax)
            {
                _Matches.Add(new NamespaceResult(_CurrentNamespace.CloneAs(), node));

            }

            if (_deph > 0) _CurrentNamespace.RemoveLast();

        }

        public void VisitTypeNode(TypeNode node)
        {

        }

        protected virtual void _ResetFields(NameManagerReadOnly findNamespace)
        {
            _deph = 0;
            _index = 0;
            _FindNamespace = findNamespace;
            _IsEndOfSearech = false;
            _Matches.Clear();
            _CurrentNamespace.Clear();
            _MatchesMax = 0;
        }



        protected int _deph = 0;
        protected int _index = 0;
        protected NameManagerReadOnly _FindNamespace = null;
        protected bool _IsEndOfSearech = false;
        protected List<NamespaceResult> _Matches = new List<NamespaceResult>();
        protected NameManager _CurrentNamespace = new NameManager();
        protected int _MatchesMax = 0;
    }
}
