#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor.Callbacks;

public class ManagerCodeGenerator
{
    // 이 메서드는 스크립트가 컴파일될 때마다 자동으로 실행됩니다
    [DidReloadScripts]
    public static void OnScriptsReloaded()
    {
        GenerateManagerProperties();
    }

    // 수동으로도 실행 가능하도록 메뉴 항목 추가
    [MenuItem("Tools/Generate Manager Properties")]
    public static void GenerateManagerPropertiesMenu()
    {
        GenerateManagerProperties();
    }

    public static void GenerateManagerProperties()
    {
        // 모든 어셈블리에서 ManagerAttribute가 적용된 타입 찾기
        var managerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !assembly.IsDynamic)
            .SelectMany(assembly => {
                try {
                    return assembly.GetTypes();
                } catch {
                    return Type.EmptyTypes;
                }
            })
            .Where(t => t.GetCustomAttribute<ManagerAttribute>() != null)
            .ToList();

        if (managerTypes.Count == 0) 
        {
            Debug.Log("No manager types found with ManagerAttribute.");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("// 자동 생성된 코드 - 수동으로 수정하지 마세요");
        sb.AppendLine("using System;");
        sb.AppendLine("using Cysharp.Threading.Tasks;");
        sb.AppendLine("namespace Core.Manager");
        sb.AppendLine("{");
        
        sb.AppendLine();
        sb.AppendLine("    public partial class Managers");
        sb.AppendLine("    {");

        // 리전별로 그룹화
        var groupedManagers = managerTypes
            .Select(t => new {
                Type = t,
                Attr = t.GetCustomAttribute<ManagerAttribute>(),
                Name = t.GetCustomAttribute<ManagerAttribute>().AccessName ?? t.Name
            })
            .GroupBy(x => x.Attr.Region)
            .OrderBy(g => g.Key);
        foreach (var group in groupedManagers)
        {
            sb.AppendLine($"        #region {group.Key}");
            foreach (var manager in group)
            {
                string propertyName = manager.Name;
                string typeName = manager.Type.FullName;
                string fieldName = $"_{char.ToLowerInvariant(propertyName[0])}{propertyName.Substring(1)}";
                sb.AppendLine($"        private readonly {typeName} {fieldName} = new();");
                sb.AppendLine($"        public static {typeName} {propertyName} => Instance.{fieldName};");
                sb.AppendLine();
            }
            sb.AppendLine("        #endregion");
            sb.AppendLine();
        }
        sb.AppendLine("        private static async UniTask InitAsyncContents()");
        sb.AppendLine("        {");
        foreach (var group in groupedManagers)
        {
            foreach (var manager in group)
            {
                string propertyName = manager.Name;
                sb.AppendLine($"            await {propertyName}.InitAsync();");
            }
        }
        sb.AppendLine("        }");
        sb.AppendLine("        private static void InitContents()");
        sb.AppendLine("        {");
        foreach (var group in groupedManagers)
        {
            foreach (var manager in group)
            {
                string propertyName = manager.Name;
                sb.AppendLine($"            {propertyName}.Init();");
            }
        }
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        // 파일에 저장
        string directoryPath = "Assets/@GGDOK/Scripts/Generated";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        string filePath = Path.Combine(directoryPath, "ManagersGenerated.cs");
        
        // 파일이 이미 있는지 확인하고, 변경사항이 있을 때만 쓰기
        bool shouldWrite = true;
        if (File.Exists(filePath))
        {
            string existingContent = File.ReadAllText(filePath);
            if (existingContent.Trim() == sb.ToString().Trim())
            {
                shouldWrite = false;
            }
        }
        
        if (shouldWrite)
        {
            File.WriteAllText(filePath, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log($"Manager properties generated at: {filePath}");
        }
    }
}
#endif
