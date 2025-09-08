using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GGDOk.Scripts.Content.MapSystem
{
    public class MapNode
    {
        public int level;
        public Vector2 position;
        public List<MapNode> prevNodes = new List<MapNode>();
        public List<MapNode> nextNodes = new List<MapNode>();
        public bool check;
        public virtual string AccessName {get; set; }

        public static MapNode CreateRandomNode()
        {
            var random = Random.Range(0, 4);
            switch (random)
            {
                case 0 :
                    return new CampFire();
                case 1 : 
                    return new EliteStage();
                case 2 : 
                    return new NormalStage();
                case 3 : 
                    return new TraderStage();
            }
            return new NormalStage();
        }
        
        
        public IEnumerable<MapNode> TraverseBFS()
        {
            var visited = new HashSet<MapNode>();
            var queue = new Queue<MapNode>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (visited.Contains(node)) continue;

                visited.Add(node);
                yield return node;

                foreach (var next in node.nextNodes)
                    queue.Enqueue(next);
            }
        }
    }

    public class First : MapNode
    {
        public override string AccessName => "CampFire";
    }

    public class CampFire : MapNode
    {
        public override string AccessName => "CampFire";
    }

    public class EliteStage : MapNode
    {
        public override string AccessName =>"EliteStage" ;
    }

    public class NormalStage : MapNode
    {
        public override string AccessName =>"NormalStage" ;   
    }

    public class TraderStage : MapNode
    {
        public override string AccessName =>"NormalStage" ;
    }

    public class BossNode : MapNode
    {
        public override string AccessName => "BossNode" ;
    }
}