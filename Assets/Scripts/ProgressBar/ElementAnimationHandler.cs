using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.ProgressBar
{
    [RequireComponent(typeof(Animation))]
    public class ElementAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        private TaskCompletionSource<bool> _completionSource;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public Task<bool> Play()
        {
            if (_completionSource != null && !_completionSource.Task.IsCanceled)
            {
                return _completionSource.Task;
            }

            _completionSource = new TaskCompletionSource<bool>();
            _animation.Stop();
            _animation.Play();
            return _completionSource.Task;
        }

        public void OnAnimationEnd()
        {
            _completionSource?.TrySetResult(true);
        }

        private void OnDestroy()
        {
            _completionSource?.TrySetResult(false);
        }
    }
}