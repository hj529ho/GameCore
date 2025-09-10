using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
public class VNGraphWindow : EditorWindow
{
     [MenuItem("Tools/Story Graph Editor")]
     public static void Open()
     {
         var window = GetWindow<VNGraphWindow>();
         window.titleContent = new GUIContent("Scenario Graph Editor");
         window.minSize = new Vector2(800,500);
     }
     // private TutorialGraphAsset _graphAsset;
     // private TutorialGraphAsset _asset;
     private VNGraphView _graphView;
     private Toolbar _toolbar;
     void OnEnable()
     {
        ConstructGraphView();
        GenerateToolBar();
     }
     private void OnDisable()
     {
         rootVisualElement.Remove(_graphView);
     }
     void ConstructGraphView()
     {
         _graphView = new VNGraphView(this)
         {
             name = "TutorialGraphView"
         };
         _graphView.StretchToParentSize();
         rootVisualElement.Add(_graphView);
     }
    void GenerateToolBar()
    {
        var newButton = new Button(OnClickNewButton) { text = "New" };
        var loadButton = new Button(OnClickLoadButton){text = "Open"};
        var saveButton = new Button(OnClickSaveButton) { text = "Save" };
        _toolbar = new Toolbar();
        _toolbar.Add(newButton);
        _toolbar.Add(loadButton);
        _toolbar.Add(saveButton);
        rootVisualElement.Add(_toolbar);
    }
    void OnClickLoadButton()
    {
        var path = EditorUtility.OpenFilePanel("Open Tutorial Graph", "Assets", "asset");
        if(!string.IsNullOrEmpty(path))
        {
            path = FileUtil.GetProjectRelativePath(path);

        }
    }
    void OnClickNewButton()
    {

    }
    void OnClickSaveButton()
    {

        AssetDatabase.SaveAssets();
    }
}



public class VNGraphView : GraphView
{
    private readonly EditorWindow _window;
    private readonly MiniMap _miniMap;
    private const float CellWidth = 300;
    private const float CellHeight = 240;
    private StartNode _startNode;
    public VNGraphView(EditorWindow window)
    {
        _window = window;
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        graphViewChanged = OnGraphViewChanged;

        // (2) 점선 배경만 깔기
        var dotted = new DottedGridBackground(this)
        {
            majorSpacingW = CellWidth,
            majorSpacingH = CellHeight,
            dotLength    = 3f,
            gap          = 5f,
            thickness    = 1.5f,
            majorColor   = new Color(1,1,1,0.45f),
        };
        Insert(0, dotted);
        dotted.StretchToParentSize();
        this.viewTransformChanged += _ => dotted.MarkDirtyRepaint();
        this.schedule.Execute(() => dotted.MarkDirtyRepaint()).Every(16);
        _startNode = CreateNode<StartNode>(new Vector2(340,240));
        CreateNode<SayNode>(new Vector2(340,480));
        CreateNode<SayNode>(new Vector2(340,480));
        CreateNode<SayNode>(new Vector2(340,480));
        CreateNode<SayNode>(new Vector2(340,480));
        CreateNode<SayNode>(new Vector2(340,480));
        CreateNode<SayNode>(new Vector2(340,480));
        CreateNode<ClickNode>(new Vector2(340,480));
        CreateNode<ClickNode>(new Vector2(340,480));
        CreateNode<ClickNode>(new Vector2(340,480));
        CreateNode<SFXNode>(new Vector2(340,480));
        CreateNode<SFXNode>(new Vector2(340,480));
        CreateNode<SFXNode>(new Vector2(340,480));
        CreateNode<SFXNode>(new Vector2(340,480));
        CreateNode<SFXNode>(new Vector2(340,480));
    }
    StartNode FindStart()
    {
        if (_startNode != null) return _startNode;
        _startNode = this.nodes.OfType<StartNode>().FirstOrDefault();
        return _startNode;
    }
    (int col, int row) GetStartGrid()
    {
        var s = FindStart();
        if (s == null) return (0, 0); // 없으면 (0,0)을 기준
        var r = s.GetPosition();
        int scol = Mathf.RoundToInt(r.x / CellWidth);
        int srow = Mathf.RoundToInt(r.y / CellHeight) +1;
        return (scol, srow);
    }
    void SnapNodeToGrid(Node n)
    {
        // 1) 현재 위치 → 그리드 좌표
        var pos = n.GetPosition();
        int col = Mathf.RoundToInt(pos.x / CellWidth);
        int row = Mathf.RoundToInt(pos.y / CellHeight);

        // 2) Start 기준 좌표
        var (scol, srow) = GetStartGrid();
        // 3) StartNode 자신은 제외, 나머지는 왼쪽/위쪽 금지
        if (n is not StartNode)
        {
            col = Mathf.Max(col, scol);
            row = Mathf.Max(row, srow);
        }
        // 4) 위치 스냅 + 크기 고정(300x240)
        n.SetPosition(new Rect(col * CellWidth, row * CellHeight, 0, 0));
        n.style.width  = CellWidth;
        n.style.height = CellHeight;
        if (n is IGridNode gn)
            gn.Coord = new GridCoord { col = col, row = row };
    }
    private GraphViewChange OnGraphViewChanged(GraphViewChange change)
    {
        if (change.movedElements != null)
        {
            foreach (var el in change.movedElements)
            {
                if (el is Node node)
                {
                    SnapNodeToGrid(node);
                }
            }
        }
        return change;
    }
    private T CreateNode<T>(Vector2 position) where T : VNNode, new()
    {
        var node = new T();
        node.SetPosition(new UnityEngine.Rect(position.x,position.y, CellWidth, CellHeight));
        AddElement(node);
        return node;
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var result = new List<Port>();
        ports.ForEach(port =>
        {
            if (port == startPort) return;
            if (port.node == startPort.node) return;            // 같은 노드는 금지
            if (port.direction == startPort.direction) return;  // 같은 방향 금지
            // 타입은 신경쓰지 않고 허용 (원하면 여기서 타입 체크 추가)
            result.Add(port);
        });
        return result;
    }
}
