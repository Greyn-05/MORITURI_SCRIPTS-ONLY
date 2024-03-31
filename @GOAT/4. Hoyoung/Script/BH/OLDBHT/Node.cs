using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }
        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;
        //node를 상속받은 클래스에서 Override해서 사용할 수 있도록
        //가상 함수로 만들고 만약 Override안할시 기본적으로 FAILURE 반환
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }

}
