using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ProgressBar
{
    public class BarScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _duration = 1f;

        private RectTransform _currentTarget;
        private Coroutine _coroutine;
        private float _time = 0;

        public void StartSmoothScrollTo(RectTransform child)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _time = 0;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _currentTarget = child;
            _coroutine = StartCoroutine(SmoothScroll(child));
        }

        private IEnumerator SmoothScroll(RectTransform child)
        {
            Vector2 startPoint = _scrollRect.content.localPosition;
            Vector2 endPoint = GetSnapToPositionToBringChildIntoView(child);

            while (_time < _duration)
            {
                var newPosition = Vector2.Lerp(startPoint, endPoint, _time / _duration);
                _scrollRect.content.localPosition = newPosition;
                _time += Time.deltaTime;
                yield return null;
            }
        }

        private Vector2 GetSnapToPositionToBringChildIntoView(RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = _scrollRect.viewport.localPosition;
            Vector2 childLocalPosition   = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );

            return result;
        }

        private void OnEnable()
        {
            if (_currentTarget == null)
            {
                return;
            }

            StartSmoothScrollTo(_currentTarget);
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}