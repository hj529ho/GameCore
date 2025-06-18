using System;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Utils
{
    public class Statics
    {
        public static int GetIndex(Vector2Int position, int column)
        {
            return (position.y - 1) * column + (position.x - 1);
        }

        public static Vector2 SizeFix(float x, float y, float max_x, float max_y)
        {
            Vector2 size = new Vector2(x, y);
            if (size.x < size.y)
            {
                size = new Vector2(size.x * max_y / size.y, max_y);
            }
            else if (size.x > size.y)
            {
                size = new Vector2(max_x, size.y * max_x / size.x);
            }
            else if (size.x == size.y)
            {
                if (max_x < max_y)
                {
                    size = new Vector2(max_x, max_x);
                }
                else if (max_x > max_y)
                {
                    size = new Vector2(max_y, max_y);
                }
                else if (max_x == max_y)
                {
                    size = new Vector2(max_x, max_x);
                }
            }

            return size;
        }

        public static Vector2 Bezier2(Vector2 p1, Vector2 p2, Vector2 p3, float value)
        {
            Vector2 A = Vector2.Lerp(p1, p2, value);
            Vector2 B = Vector2.Lerp(p2, p3, value);
            return Vector2.Lerp(A, B, value);
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }
        
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null)
                return null;
            else
                return transform.gameObject;
        }

        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false)
            where T : UnityEngine.Object
        {
            if (go == null)
                return null;
            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }

            return null;
        }

        public static bool IsValidUrl(string url)
        {
            Uri uriResult;
            bool isValidUrl =
                Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return isValidUrl;
        }

        public static bool HasHtmlTags(string text)
        {
            // 정규식을 이용하여 HTML 태그를 찾습니다.
            string pattern = @"<[^>]*>";
            return Regex.IsMatch(text, pattern);
        }

        public static Define.SkillType GetSkillType(int value)
        {
            foreach (Define.SkillType skillType in Enum.GetValues(typeof(Define.SkillType)))
            {
                int minValue = (int)skillType;
                int maxValue = minValue + 5; // 100501~ 100506 사이 값이면 100501값 리턴

                if (value >= minValue && value <= maxValue)
                {
                    return skillType;
                }
            }

            Debug.LogError($" Faild add skill : {value}");
            return Define.SkillType.None;
        }

        public static Define.SkillType GetSkillType(string value)
        {
            foreach (Define.SkillType skillType in Enum.GetValues(typeof(Define.SkillType)))
            {
                if (value == skillType.ToString())
                    return skillType;
            }

            Debug.LogError($" Faild add skill : {value}");
            return Define.SkillType.None;
        }

        public static void DelayInvoke(Action action, float delay)
        {
            DelayActionAsync().Forget();
            return;
            
            
            async UniTask DelayActionAsync()
            {
                await UniTask.WaitForSeconds(delay);;
                action?.Invoke();
            }
        }
        
        /// <summary>
        /// bytes를 받아서 GB, MB, KB, Bytes 단위로 변환하여 반환
        /// </summary>
        /// <param name="bytes">바이트 크기</param>
        /// <returns></returns>
        public static string FormatBytes(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }

            return "0 Bytes";
        }

    }
}