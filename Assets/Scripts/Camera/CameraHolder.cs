using UnityEngine;
using Utils.Singleton;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [SerializeField] private UnityEngine.Camera mainCamera;

        [Range(0.1f,1f)]
        [SerializeField] private float _dragSmooth = 1f;
        
        public UnityEngine.Camera MainCamera => mainCamera;

        public float DragSmooth => _dragSmooth;

        private Vector3 _initPos;

        protected override void Init()
        {
            base.Init();
            _initPos = transform.position;
        }

        
        public void ResetPosition()
        {
            transform.position = _initPos;
        }

        public void GetOrthoWidthAndHeight(out float cameraWidth, out float cameraHeight)
        {
            cameraHeight = mainCamera.orthographicSize * 2f;
            cameraWidth = cameraHeight * mainCamera.aspect;
        }

        public Bounds OrthographicBounds(Vector3 pos)
        {
            GetOrthoWidthAndHeight(out var width, out var height);
            var bounds = new Bounds(pos,
                new Vector3(width, height, mainCamera.nearClipPlane));
            return bounds;
        }
        
        public Bounds OrthographicBounds()
        {
            GetOrthoWidthAndHeight(out var width, out var height);
            var bounds = new Bounds(transform.position,
                new Vector3(width, height, mainCamera.nearClipPlane));
            return bounds;
        }
    }
}