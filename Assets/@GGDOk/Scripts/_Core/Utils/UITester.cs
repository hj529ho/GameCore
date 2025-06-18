// using System.Collections;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// /// <summary>
// /// TODO 원래 오딘 시리얼라이저에서 돌던건데 CustomWindow로 만들예정
// /// </summary>
// public class UITester : MonoBehaviour
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void Initializer()
//     {
//         Debug.Log("UI Tester init");
//         GameObject gameObject = new GameObject() { name = $"@UITester" };
//         gameObject.AddComponent<UITester>();
//         DontDestroyOnLoad(gameObject);
//     }
//
//     public UISequence UISequence;
//     
//     
//     
//     public void Test()
//     {
//         StartCoroutine(TestCoroutine());
//     }
//
//     IEnumerator TestCoroutine()
//     {
//         GameObject go = GameObject.Find("@UI_Root");
//         foreach (UISequence.UIComponent component in UISequence.List)
//         {
//             switch (component.type)
//             {
//                 case UISequence.UIComponent.Type.Button:
//                     Transform buttonTransform = go.transform.Find(component.Atrribute1).Find(component.Atrribute2);
//                     Button.ButtonClickedEvent buttonClickedEvent = buttonTransform.GetComponent<Button>().onClick;
//                     buttonClickedEvent.Invoke();
//                     yield return null;
//                     break;
//                 case UISequence.UIComponent.Type.InputField:
//                     Transform inputFieldTransform = go.transform.Find(component.Atrribute1).Find(component.Atrribute2);
//                     TMP_InputField inputField = inputFieldTransform.GetComponent<TMP_InputField>();
//                     inputField.text = component.Atrribute2;
//                     yield return null;
//                     break;
//                 case UISequence.UIComponent.Type.Slider:
//                     Transform sliderTranform = go.transform.Find(component.Atrribute1).Find(component.Atrribute2);
//                     Slider slider = sliderTranform.GetComponent<Slider>();
//                     slider.value = float.Parse(component.Atrribute2);
//                     yield return null;
//                     break;
//                 case UISequence.UIComponent.Type.Delay:
//                     yield return new WaitForSeconds(float.Parse(component.Atrribute1));
//                     break;
//             }
//         }
//     }
//
//
// }