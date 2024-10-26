using System;
using Camera;
using Connection;
using Player;
using Player.ActionHandlers;
using UnityEngine;
using Utils.MonoBehaviourUtils;

namespace Levels
{
    public class GameLevel : GameMonoBehaviour
    {
     

        private ClickHandler _clickHandler;
        private CameraHolder _cameraHolder;

        private Bounds _levelBounds;
        private Bounds _cameraBounds;
        private bool _isCameraScrolling;
        private Vector3 _startDragPosition;

        private void Awake()
        {
            _clickHandler = ClickHandler.Instance;
            _cameraHolder = CameraHolder.Instance;
        }

        private void Start()
        {
            _cameraHolder.ResetPosition();

            var nodes = GetComponentsInChildren<ColorNode>();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (i == 0)
                {
                    _levelBounds = nodes[i].Bounds;
                }
                else
                {
                    _levelBounds.Encapsulate(nodes[i].Bounds);
                }
            }

            _cameraBounds = _cameraHolder.OrthographicBounds();

            if (_levelBounds.size.x > _cameraBounds.size.x || _levelBounds.size.y > _cameraBounds.size.y)
            {
                _clickHandler.DragStartEvent += OnDragStartEventHandler;
                _clickHandler.DragEndEvent += OnDragEndEventHandler;
            }
        }

        private void OnDestroy()
        {
            _clickHandler.DragStartEvent -= OnDragStartEventHandler;
            _clickHandler.DragEndEvent -= OnDragEndEventHandler;
        }

        private void OnDragEndEventHandler(Vector3 endPos)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling) return;
            _isCameraScrolling = false;
        }

        private void OnDragStartEventHandler(Vector3 startPos)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling) return;
            _isCameraScrolling = true;
            _startDragPosition = startPos;
        }

        private void Update()
        {
            if (!_isCameraScrolling) return;

            var currentPos = _cameraHolder.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            var delta = ClampDelta((_startDragPosition - currentPos) * _cameraHolder.DragSmooth);

            _cameraHolder.transform.Translate(delta);
        }

        private Vector3 ClampDelta(Vector3 delta)
        {
            var currentPos = _cameraHolder.transform.position;
            delta.z = 0f;

            Vector3 targetPos = currentPos + delta;

            _cameraBounds = _cameraHolder.OrthographicBounds();

            if (targetPos.x < _levelBounds.min.x)
                delta.x = _levelBounds.min.x - currentPos.x;
            if (targetPos.x > _levelBounds.max.x)
                delta.x = _levelBounds.max.x - currentPos.x;

            if (targetPos.y < _levelBounds.min.y)
                delta.y = _levelBounds.min.y - currentPos.y;
            if (targetPos.y > _levelBounds.max.y)
                delta.y = _levelBounds.max.y - currentPos.y;


            return delta;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_levelBounds.center, _levelBounds.size);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_cameraBounds.center, _cameraBounds.size);
        }
    }
}