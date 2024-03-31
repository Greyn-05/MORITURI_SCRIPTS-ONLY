using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        
        protected void Start()
        {
            _root = SetupTree();
        }

        
        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }
        //업데이트 마다, 노드에 대한 평가를 진행

        protected abstract Node SetupTree();
        //BH Tree를 사용할 곳에서 Override하기 위해 추상화

    }

}
