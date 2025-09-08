using System;
using System.Collections.Generic;
using System.Linq;
using GGDOk.Scripts.Content.MapSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordMapCreator : MonoBehaviour
{
    private MapNode MapTree = null;
    private MapNode BossNode = new BossNode();
    Dictionary<int,List<MapNode>> nodes = new Dictionary<int, List<MapNode>>();
    private int maxLevel = 17;
    private void Start()
    {
        MapTree = new First();
        BossNode.level = maxLevel;
        CreateMap(MapTree,maxLevel);
        nodes.Add(BossNode.level, new List<MapNode>());
        nodes[BossNode.level].Add(BossNode);
    }
    
    
    public void CreateMap(MapNode root, int maxLevel)
    {
        int nextLevel = 1;
        int maxAtNext;
        nodes.Add(0, new List<MapNode>());
        nodes[0].Add(root);
        for (int i = 1; i < maxLevel; i++)
        {
            maxAtNext = Math.Min(nextLevel, maxLevel - nextLevel + 1);
            var currentLevel = i;
            nodes.Add(currentLevel, new List<MapNode>());
            for (int j = 0; j < maxAtNext; j++)
            {
                var node = MapNode.CreateRandomNode();
                node.level = currentLevel;
                nodes[currentLevel].Add(node);
            }
            nextLevel = currentLevel + 1;
        }
        var levelKeys = nodes.Keys.OrderBy(k => k).ToList();
        for (int li = 0; li < levelKeys.Count - 1; li++) // 마지막 레벨은 Boss로만 연결
        {
            var curr = nodes[levelKeys[li]];
            var next = nodes[levelKeys[li + 1]];

            for (int j = 0; j < curr.Count; j++)
            {
                var (a, b) = GetNearestTwoIndices(j, curr.Count, next.Count);
                var from = curr[j];
                var first  = next[a];
                var second = next[b];

                if (curr.Count == 1)
                {
                    Link(from, first);
                    if (b != a) Link(from, second);
                    continue;
                }
                int r = Random.Range(0, 3); // 0,1,2
                if (r == 0) Link(from, first);
                else if (r == 1) Link(from, second);
                else { Link(from, first); if (b != a) Link(from, second); }
            }
        }
        var lastLevel = nodes[levelKeys[^1]];
        foreach (var node in lastLevel) Link(node, BossNode);


        foreach (var node in root.TraverseBFS())
        {
            node.check = true;
        }
        
        
        void Link(MapNode from, MapNode to)
        {
            if (!from.nextNodes.Contains(to)) from.nextNodes.Add(to);
            if (!to.prevNodes.Contains(from)) to.prevNodes.Add(from);
        }
    }

    /// <summary>
    /// 현재 레벨의 노드 개수 n, 다음 레벨의 노드 개수 m,
    /// 현재 노드 인덱스 j가 주어졌을 때
    /// 다음 레벨에서 가장 가까운 두 개 노드 인덱스를 반환.
    /// (반드시 하나 이상은 보장됨)
    /// </summary>
    public static (int first, int second) GetNearestTwoIndices(int currentIndex, int currentMax, int nextMax)
    {
        if (nextMax <= 0) throw new ArgumentException("다음 레벨 노드 수는 1 이상이어야 합니다.");

        // 허용 구간 [L, R] 계산
        // j가 커질수록 L, R은 절대 감소하지 않으므로(단조 증가) 전체 매핑이 안정적
        int j = currentIndex;
        int n = currentMax;
        int m = nextMax;

        int L = Mathf.FloorToInt((float)j     * m / n);
        int R = Mathf.CeilToInt ((float)(j+1) * m / n) - 1;
        L = Mathf.Clamp(L, 0, m - 1);
        R = Mathf.Clamp(R, L, m - 1); // 최소한 L ≤ R

        if (L == R)
        {
            // 이 부모가 담당할 수 있는 다음 레벨 인덱스가 단 하나일 때
            return (L, L);
        }

        // 목표 위치(비율 기반 중앙) -> 허용 구간 내에서 가장 가까운 정수 인덱스로 스냅
        float target = ((j + 0.5f) * m / n); // 0 ~ m
        int t0 = Mathf.Clamp(Mathf.RoundToInt(target - 0.5f), L, R);

        // 두 번째 후보: t0의 이웃 중에서 [L, R]에 들어오는 더 가까운 쪽
        int left  = Mathf.Max(L, t0 - 1);
        int right = Mathf.Min(R, t0 + 1);

        int t1;
        if (left == t0 && right == t0)
        {
            t1 = t0;
        }
        else if (left == t0)
        {
            t1 = right;
        }
        else if (right == t0)
        {
            t1 = left;
        }
        else
        {
            // 거리 비교(중앙 정렬 보정은 동일하게 -0.5f 사용)
            float dl = Mathf.Abs((left  + 0.5f) - (target));
            float dr = Mathf.Abs((right + 0.5f) - (target));
            t1 = (dl <= dr) ? left : right;
        }

        return (t0, t1);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 0.2f);

        if(MapTree == null)
            return;

        foreach (var list in nodes.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var x = list.Count % 2 == 1 ? i - (float)list.Count / 2 + 0.5f :  i - ((float)list.Count / 2 - 0.5f) ;
                var node = list[i];
                var pos = new Vector3(x,node.level);
                node.position = pos;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(pos, 0.2f);
                
                Gizmos.color = Color.green;
                foreach (var prev in node.prevNodes)
                {
                    if(prev.check == false)
                        continue;
                    Gizmos.DrawLine(node.position, prev.position);
                }
            }
        }
    }
}
