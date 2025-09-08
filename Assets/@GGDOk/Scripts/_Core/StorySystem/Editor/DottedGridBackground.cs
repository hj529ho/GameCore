using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DottedGridBackground : VisualElement
{
    private readonly GraphView _graphView;

    // 스타일 파라미터
    public float majorSpacing = 120f;
    public float dotLength    = 2f;
    public float gap          = 6f;
    public float thickness    = 1f;
    public Color majorColor   = new Color(1, 1, 1, 0.15f);

    public DottedGridBackground(GraphView gv)
    {
        _graphView = gv;
        pickingMode = PickingMode.Ignore;           // 클릭 통과
        generateVisualContent += OnGenerateVisualContent;

        // 뷰 변할 때마다 리페인트
        RegisterCallback<GeometryChangedEvent>(_ => MarkDirtyRepaint());
        // 일부 버전에서는 아래 이벤트가 없을 수 있음 → 그래도 contentViewContainer transform 변화로 충분
        gv.viewTransformChanged += _ => MarkDirtyRepaint();
    }

    private void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var r = contentRect;
        if (r.width <= 0 || r.height <= 0) return;

        // GraphView의 뷰 변환 읽기
        var t    = _graphView.contentViewContainer.transform;
        float sx = t.scale.x;       // 균일 스케일 가정 (GraphView는 x=y)
        float sy = t.scale.y;
        Vector3 pan = t.position;   // 화면 좌표계에서의 평행이동 (px)

        // 1) 화면상 그리드 간격 = 콘텐츠 간격 * 스케일
        float stepX = Mathf.Max(1f, majorSpacing * sx);
        float stepY = Mathf.Max(1f, majorSpacing * sy);

        // 2) 화면상 오프셋 = pan 그대로 (※ scale 곱하지 말 것)
        float offX = Mathf.Repeat(pan.x, stepX);
        float offY = Mathf.Repeat(pan.y, stepY);

        var p = mgc.painter2D;
        p.lineCap   = LineCap.Butt;
        p.lineJoin  = LineJoin.Miter;
        p.lineWidth = thickness; // 화면 픽셀 기준 두께(고정)

        // 점 크기/간격은 줌에 따라 키우면 시각적으로 자연스러움
        float dot = Mathf.Max(1f, dotLength * (sx + sy) * 0.5f);
        float gap = Mathf.Max(1f, this.gap * (sx + sy) * 0.5f);

        DrawDottedGrid(p, r, stepX, stepY, offX, offY, dot, gap, majorColor);
    }

    private void DrawDottedGrid(
        Painter2D p, Rect r,
        float stepX, float stepY,
        float offX, float offY,
        float dot, float gap,
        Color color)
    {
        // 수직 점선
        for (float x = r.x + offX; x <= r.xMax; x += stepX)
        {
            for (float y = r.y; y <= r.yMax; y += dot + gap)
            {
                p.BeginPath();
                p.strokeColor = color;
                p.MoveTo(new Vector2(x, y));
                p.LineTo(new Vector2(x, Mathf.Min(y + dot, r.yMax)));
                p.Stroke();
            }
        }

        // 수평 점선
        for (float y = r.y + offY; y <= r.yMax; y += stepY)
        {
            for (float x = r.x; x <= r.xMax; x += dot + gap)
            {
                p.BeginPath();
                p.strokeColor = color;
                p.MoveTo(new Vector2(x, y));
                p.LineTo(new Vector2(Mathf.Min(x + dot, r.xMax), y));
                p.Stroke();
            }
        }
    }
}