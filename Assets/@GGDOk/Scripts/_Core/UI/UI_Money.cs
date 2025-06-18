// using Core.Manager;
// using Core.UI;
// using TMPro;
// using DataTypes;
//
// public class UI_Money : UI_Scene
// {
//     public Currency testCurrency;
//     
//     enum Texts
//     {
//         GoldText,
//         GemText,
//         COUNT
//     }
//     public override void Init()
//     {
//         base.Init();
//         Bind<TMP_Text>(typeof(Texts));
//     }
//
//     public void OnNotify(Currency value)
//     {
//         Get<TMP_Text>((int)Texts.GoldText).text = $"{value.gold}";
//         Get<TMP_Text>((int)Texts.GemText).text =$"{value.gem}";
//     }
// }