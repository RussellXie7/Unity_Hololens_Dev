using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

namespace HoloToolkit.Sharing.Tests
{
    public class SphereManager : MonoBehaviour
    {

        private bool dragged;
        Vector3 initialPos;
        Quaternion initialRot;

        public TextMesh AnchorDebugText;

        // Use this for initialization
        void Start()
        {
            if (AnchorDebugText != null)
            {
                AnchorDebugText.text += "\nregisteredmessagehandler";
            }
            CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.sphere] = this.ProcessRemoteSphere;

            dragged = false;
            initialPos = transform.localPosition;
            initialRot = transform.localRotation;

            HoloToolkit.Unity.InputModule.HandDraggable dragInstance = this.GetComponent<HoloToolkit.Unity.InputModule.HandDraggable>();
            HoloToolkit.Unity.InputModule.GesturesInput gesInput = this.GetComponent<HoloToolkit.Unity.InputModule.GesturesInput>();

            
            if (!dragInstance)
            {
                Debug.Log("Could not find HandDraggable Components");

            }
            else
            {
                dragInstance.StartedDragging += DragInstance_StartedDragging;
                dragInstance.StoppedDragging += DragInstance_StoppedDragging;
            }
        }

        private void ProcessRemoteSphere(NetworkInMessage msg)
        {
            long userID = msg.ReadInt64();

            transform.localPosition = initialPos + CustomMessages.Instance.ReadVector3(msg);
            transform.localRotation = initialRot * CustomMessages.Instance.ReadQuaternion(msg);
        }

        private void DragInstance_StartedDragging()
        {
            if (AnchorDebugText != null)
            {
                AnchorDebugText.text += "\nDragging Started";
            }
            dragged = true;
        }

        private void DragInstance_StoppedDragging()
        {
            if (AnchorDebugText != null)
            {
                AnchorDebugText.text += "\nDragging Ended";
            }
            dragged = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (dragged)
            {
                CustomMessages.Instance.SendSphere(this.transform.localPosition - initialPos, this.transform.localRotation * Quaternion.Inverse(initialRot));
            }
        }
    }
}