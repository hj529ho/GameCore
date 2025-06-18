// using System.Collections;
// using Spine.Unity;
// #if UNITY_EDITOR
// using Spine.Unity.Editor;
// #endif
// using UnityEngine;
//
// namespace Core.Utils
// {
//     public class SpineAnimationHandler : MonoBehaviour
//     {
//         public SkeletonGraphic skeletonGraphic;
//         
//         private Coroutine _playAnimationOnceCoroutine;
//         private void Awake()
//         {
//
//         }
//         public void Init(SkeletonDataAsset skeletonDataAsset)
//         {
//             skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
// #if UNITY_EDITOR
//             SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(skeletonGraphic);
// #endif
//             skeletonGraphic.MatchRectTransformWithBounds();
//             skeletonGraphic.rectTransform.anchoredPosition = Vector2.zero;
//         }
//
//         public void SetState(string animationName, bool loop)
//         {
//             if(skeletonGraphic == null)
//                 return;
//             if(skeletonGraphic.AnimationState.GetCurrent(0).Animation.Name !=animationName )
//                 skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
//         }
//
//         //Play Spine Animation once than return to previous animation
//         //1. Save current animation
//         //2. Play new animation
//         //3. Wait for new animation to finish
//         //4. Play saved animation
//         public void PlayAnimationOnce(string animationName)
//         {
//             if(skeletonGraphic == null)
//                 return;
//             if (_playAnimationOnceCoroutine != null)
//                 return;
//             var currentAnimation = skeletonGraphic.AnimationState.GetCurrent(0).Animation.Name;
//             skeletonGraphic.AnimationState.SetAnimation(0, animationName, false);
//             _playAnimationOnceCoroutine = StartCoroutine(PlayAnimationOnceCoroutine(currentAnimation));
//         }
//
//         private IEnumerator PlayAnimationOnceCoroutine(string animationName)
//         {
//             yield return new WaitForSeconds(skeletonGraphic.AnimationState.GetCurrent(0).Animation.Duration);
//             SetState(animationName, true);
//             _playAnimationOnceCoroutine = null;
//         }
//     }
// }