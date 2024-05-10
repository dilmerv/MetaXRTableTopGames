using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform target; // Target object to follow
        [SerializeField] private float followSpeed = 5f; // Speed of following
        [SerializeField] private IgnoreRotationAxis[] ignoreRotationAxis;
    
        private Vector3 offset;

        private enum IgnoreRotationAxis
        {
            None,
            XAxis,
            YAxis,
            ZAxis
        }
    
        private void Start()
        {
            offset = target.position - transform.position;
        }

        void Update()
        {
            Vector3 targetPositionWithOffset = target.position - offset;
            transform.position = Vector3.Lerp(transform.position, targetPositionWithOffset, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, GetTargetRotation(), Time.deltaTime * followSpeed);
        }

        private Quaternion GetTargetRotation()
        {
            Quaternion currentRotation = target.rotation;
            Quaternion targetRotation = Quaternion.identity;

            // Modify the target rotation based on the specified ignore axes
            targetRotation = Quaternion.Euler(
                ignoreRotationAxis.Contains(IgnoreRotationAxis.XAxis) ? 0 : currentRotation.eulerAngles.x,
                ignoreRotationAxis.Contains(IgnoreRotationAxis.YAxis) ? 0 : currentRotation.eulerAngles.y,
                ignoreRotationAxis.Contains(IgnoreRotationAxis.ZAxis) ? 0 : currentRotation.eulerAngles.z
            );
        
            return targetRotation;
        }
    }
}
