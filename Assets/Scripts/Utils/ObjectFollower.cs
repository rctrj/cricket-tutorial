using UnityEngine;

namespace Utils
{
    public class ObjectFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool isFollowing;

        private Vector3 _initPos;
        private Quaternion _initRot;
        private Vector3 _gap;

        private Transform _transform;

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        private void Awake()
        {
            _transform = transform;
            _initPos = _transform.position;
            _initRot = _transform.rotation;
        }

        public void Follow(Transform target)
        {
            Target = target;
            isFollowing = true;
            _gap = target.position - _transform.position;
        }

        public void Reset()
        {
            isFollowing = false;
            _transform.position = _initPos;
            _transform.rotation = _initRot;
        }

        private void Update()
        {
            if (!isFollowing) return;
            _transform.position = target.position - _gap;
            _transform.LookAt(target);
        }
    }
}