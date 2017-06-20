using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class Gesture: MonoBehaviour {

    public static Gesture Instance { get; private set; }

    public GameObject FocusedObject { get; private set; }

    private GestureRecognizer recognizer;
    private GestureRecognizer NavigationRecognizer;
    private GestureRecognizer ManipulationRecognizer;

    private GestureRecognizer ActiveRecognizer;
    public bool isNavigating { get; private set; }
    public Vector3 NavigationPosition { get; private set; }

    public bool isManipulation { get; private set; }
    public Vector3 ManipulationPosition { get; private set; }


    // Use this for initialization
    void Awake()
    {
        // singleTon
        Instance = this;

        // for recognizing Tapping
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += Recognizer_TappedEvent;
        recognizer.StartCapturingGestures();

        // for navigation gesture
        NavigationRecognizer = new GestureRecognizer();
        NavigationRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationRailsX |
            GestureSettings.NavigationRailsY | GestureSettings.NavigationRailsZ);
        //NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
        NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

        // for dragging
        ManipulationRecognizer = new GestureRecognizer();
        ManipulationRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;

        ResetGestureRecognizers();
    }

    private void ResetGestureRecognizers()
    {
        GestureTransition(ManipulationRecognizer);
    }

    public void GestureTransitionOuterControl(int index)
    {
        if(index == 0)
        {
            GestureTransition(NavigationRecognizer);
        }

        else if(index == 1)
        {
            GestureTransition(ManipulationRecognizer);
        }
    }
    public void GestureTransition(GestureRecognizer myRecognizer)
    {
        if(myRecognizer == null)
        {
            return;
        }

        if(ActiveRecognizer != null)
        {
            if(ActiveRecognizer == myRecognizer)
            {
                return;
            }

            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }

        myRecognizer.StartCapturingGestures();
        ActiveRecognizer = myRecognizer;
    }

    private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isManipulation = false;
    }

    private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        //throw new System.NotImplementedException();

        isManipulation = false;
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isManipulation = true;

        ManipulationPosition = cumulativeDelta;

        if(FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("PerformManipulationUpate", cumulativeDelta);
        }
    }

    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isManipulation = true;

        ManipulationPosition = cumulativeDelta;

        if(FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("PerformManipulationStart", cumulativeDelta);
        }
    }

    private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isNavigating = false;
    }

    private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isNavigating = false;
    }

    private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isNavigating = true;
        NavigationPosition = normalizedOffset;
    }

    private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
    {
        //throw new System.NotImplementedException();
        isNavigating = true;
        NavigationPosition = normalizedOffset;
    }

    //private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    //{
    //    throw new System.NotImplementedException();
    //}

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        //throw new System.NotImplementedException();
        if(FocusedObject != null)
        {
            if (FocusedObject.tag == "MyButton")
            {
                FocusedObject.SendMessageUpwards("Toggle", FocusedObject);

                Debug.Log("object is " + FocusedObject);
            }
            else
            {
                //SendMessageUpwards sends to both ancesters and the children
                FocusedObject.SendMessageUpwards("OnSelect");
            }
        }
    }

    // Update is called once per frame
    void Update () {
        GameObject oldFocusedObject = FocusedObject;

        var headPosition = Camera.main.transform.position;
        var headDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if(Physics.Raycast(headPosition, headDirection, out hitInfo))
        {
            if(!isNavigating && !isManipulation)
            {
                FocusedObject = hitInfo.collider.gameObject;
            }
            
        }
        else
        {
            if (!isManipulation && !isNavigating)
            {
                // does hot hit anything
                FocusedObject = null;
            }
        }

        if(FocusedObject != oldFocusedObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
	}   
}
