using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public struct GridCoord { public int col, row; }
public interface IGridNode { GridCoord Coord { get; set; } }

public class VNNode : Node, IGridNode
{
    public GridCoord Coord { get; set; }

    // 300×120 셀로 스냅 (시각 크기도 동일하게 유지)
    protected virtual Vector2 GridCellSize => new Vector2(300f, 240);

    // 포트를 안 쓰는 노드는 공백을 없애기 위해 숨김 (파생에서 필요하면 true로)
    protected virtual bool UsePorts => false;

    // 좌/상단 제한(필요하면 0으로 변경)
    protected virtual int MinCol => int.MinValue;
    protected virtual int MinRow => int.MinValue;

    public VNNode()
    {
        // ── 공통 스타일: 타이틀-본문 간격 제거 ───────────────────────────────
        titleContainer.style.marginBottom  = 0;
        titleContainer.style.paddingBottom = 0;
        mainContainer.style.marginTop      = 0;
        mainContainer.style.paddingTop     = 0;

        var contents = this.Q("contents");
        if (contents != null)
        {
            contents.style.paddingTop    = 0;
            contents.style.paddingBottom = 0;
            contents.style.marginTop     = 0;
            contents.style.marginBottom  = 0;
            contents.style.flexGrow      = 1;
            contents.style.flexDirection = FlexDirection.Column;
            contents.style.justifyContent = Justify.FlexStart; // ★ 핵심: 위에서부터 채우기
            contents.style.alignItems     = Align.Stretch;
        }

        mainContainer.style.marginTop      = 0;
        mainContainer.style.paddingTop     = 0;
        mainContainer.style.marginBottom   = 0;
        mainContainer.style.paddingBottom  = 0;
        mainContainer.style.flexDirection  = FlexDirection.Column;
        mainContainer.style.justifyContent = Justify.FlexStart; // ★
        mainContainer.style.alignItems     = Align.Stretch;     // ★
        mainContainer.style.flexGrow       = 1;

        // (포트 안 쓰면 topContainer / input / output 숨김 — 필요 없으면 빼도 됨)
        // topContainer가 남아 있어도 contents 위 패딩을 제거했기 때문에 띠가 사라집니다.
        inputContainer.style.display  = DisplayStyle.None;
        outputContainer.style.display = DisplayStyle.None;
        topContainer.style.display    = DisplayStyle.None; 
        titleContainer.style.display = DisplayStyle.None;
        
        mainContainer.style.backgroundColor = new Color(0.5f, 0.5f, 0.5f,0.3f);
    
        // 노드 자체 패딩 최소화(선택)
        style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = 0;
        titleContainer.style.flexDirection = FlexDirection.Row;
        titleContainer.contentContainer.style.flexDirection = FlexDirection.Row;
        titleContainer.style.width = 0;
        titleContainer.style.height = 240;
        topContainer.style.height = 0;
        
        mainContainer.style.alignContent = Align.FlexStart;
        mainContainer.style.flexDirection = FlexDirection.Row;
        
    }

    public override void SetPosition(Rect newPos)
    {
        style.width = 300;
        style.height = 240;
        
        
        float cellW = GridCellSize.x;
        float cellH = GridCellSize.y;

        int col = Mathf.RoundToInt(newPos.x / cellW);
        int row = Mathf.RoundToInt(newPos.y / cellH);
        if (col < MinCol) col = MinCol;
        if (row < MinRow) row = MinRow;

        // 스냅 + 크기 강제 (초기 생성/드래그 시 모두 동일하게 유지)
        var snapped = new Rect(col * cellW, row * cellH, cellW, cellH);
        base.SetPosition(snapped);

        Coord = new GridCoord { col = col, row = row };
    }

    // 초기 배치 헬퍼
    public void PlaceAt(int col, int row)
    {
        var r = new Rect(col * GridCellSize.x, row * GridCellSize.y,
                         GridCellSize.x, GridCellSize.y);
        base.SetPosition(r);
        Coord = new GridCoord { col = col, row = row };
    }
}

public class StartNode : VNNode
{
    public StartNode()
    {
        var label = new Label("스토리는 여기서 시작됩니다.\n TODO 여기에 간단한 사용 방법을 적어놓을 것.");
        // 라벨 위쪽 붙이기
        label.style.alignSelf = new StyleEnum<Align>(Align.Center);
        label.style.display = DisplayStyle.Flex;
        label.style.marginTop = 4;
        mainContainer.Add(label);
        var spacer = new VisualElement { style = { flexGrow = 1 } };
        mainContainer.Add(spacer);
        RefreshExpandedState();
        RefreshPorts();
    }
}

public sealed class SayNode : VNNode
{
    public SayNode()
    {
        title = "";
        // 1) USS 추가 (한 번만 추가해도 됨)
        var ss = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/@GGDOk/Scripts/_Core/StorySystem/aa.uss");
        if (ss != null && !styleSheets.Contains(ss)) styleSheets.Add(ss);

        // 2) UXML 로드 & 인스턴스화
        var vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/@GGDOk/Scripts/_Core/StorySystem/SayNode.uxml");
        var content = vta?.CloneTree();
        if (content != null)
        {
            // 3) Node의 mainContainer에 붙이기
            mainContainer.Add(content);

            // 4) 필요한 내부 요소 참조(선택)
            var speaker = content.Q<TextField>("speaker");
            var dialog  = content.Q<TextField>("dialog");

            // 필요 시 런타임 값/옵션 셋업
            // speaker.value = "speaker";
            // dialog.value  = "대사 입력";
        }
        
        RefreshExpandedState();
        
    }
}

public sealed class SFXNode : VNNode
{
    public SFXNode()
    {
        var ss = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/@GGDOk/Scripts/_Core/StorySystem/aa.uss");
        if (ss != null && !styleSheets.Contains(ss)) styleSheets.Add(ss);

        // 2) UXML 로드 & 인스턴스화
        var vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/@GGDOk/Scripts/_Core/StorySystem/SFXNode.uxml");
        var content = vta?.CloneTree();
        if (content != null)
        {
            // 3) Node의 mainContainer에 붙이기
            mainContainer.Add(content);

            // 4) 필요한 내부 요소 참조(선택)
            var speaker = content.Q<TextField>("speaker");
            var dialog  = content.Q<TextField>("dialog");

            // 필요 시 런타임 값/옵션 셋업
            // speaker.value = "speaker";
            // dialog.value  = "대사 입력";
        }
        
        RefreshExpandedState();
    }
}


public sealed class ClickNode : VNNode
{
    public ClickNode()
    {
        var label = new Label("클릭 구분 노드");
        // 라벨 위쪽 붙이기
        label.style.alignSelf = new StyleEnum<Align>(Align.Center);
        label.style.display = DisplayStyle.Flex;
        label.style.marginTop = 4;
        mainContainer.Add(label);
        var spacer = new VisualElement { style = { flexGrow = 1 } };
        mainContainer.Add(spacer);
        RefreshExpandedState();
        RefreshPorts();
    }
}