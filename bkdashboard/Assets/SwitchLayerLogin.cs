using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


    public class SwitchLayerLogin : MonoBehaviour
    {
        [SerializeField]
        public CanvasGroup _canvasLayerLogin;
        [SerializeField]
        public CanvasGroup _canvasWorking;
        private Tween twenFade;
        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }

            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }
        public IEnumerator _IESwitchLayer()
        {
            if (_canvasLayerLogin.interactable == true)
            {
                FadeOut(_canvasLayerLogin, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasWorking, 0.25f);
            }
            else
            {
                FadeOut(_canvasWorking, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayerLogin, 0.25f);
            }
        }
        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }
    }
