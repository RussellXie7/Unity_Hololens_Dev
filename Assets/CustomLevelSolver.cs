using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class CustomLevelSolver : MonoBehaviour {

    public GameObject _picture;

    // this is singleton
    public static CustomLevelSolver Instance;

    // Enums
    public enum QueryStates
    {
        None,
        Processing,
        Finished
    }

    // Structs
    private struct QueryStatus
    {
        public void Reset()
        {
            State = QueryStates.None;
            Name = "";
            CountFail = 0;
            CountSuccess = 0;
            QueryResult = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult>();
        }

        public QueryStates State;
        public string Name;
        public int CountFail;
        public int CountSuccess;
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult> QueryResult;
    }


    // Use to store the PlacementQuery object
    private struct PlacementQuery
    {
        public PlacementQuery(
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
            List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
            List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null)
        {
            PlacementDefinition = placementDefinition;
            PlacementRules = placementRules;
            PlacementConstraints = placementConstraints;
        }

        public SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition PlacementDefinition;
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> PlacementRules;
        public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> PlacementConstraints;
    }

    // The class that spawns the actual object
    private class PlacementResult
    {
        public PlacementResult(float timeDelay, SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult result)
        {
            Picture = Instantiate(CustomLevelSolver.Instance._picture, result.Position, Quaternion.identity);
            Picture.isStatic = false;
            Picture.transform.forward = result.Forward;
            Result = result;
        }

        public GameObject Picture;
        public SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult Result;
    }

    // Properties
    public bool IsSolverInitialized { get; private set; }

    // Privates
    private List<PlacementResult> placementResults = new List<PlacementResult>();
    // Singleton last the lifetime of the program with only one existence
    private QueryStatus queryStatus = new QueryStatus();

    private void Awake()
    {
        Instance = this;
    }

    // first overload function that calls that process the input and deploy to the actual function
    private bool PlaceObjectAsync(
    string placementName,
    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
    List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
    List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null,
    bool clearObjectsFirst = true)
    {
        return PlaceObjectAsync(
            placementName,
            new List<PlacementQuery>() { new PlacementQuery(placementDefinition, placementRules, placementConstraints) },
            clearObjectsFirst);
    }


    private bool PlaceObjectAsync(
        string placementName,
        List<PlacementQuery> placementList,
        bool clearObjectsFirst = true)
    {
        // If we already mid-query, reject the request
        if (queryStatus.State != QueryStates.None)
        {
            return false;
        }

        // Mark it
        queryStatus.Reset();
        queryStatus.State = QueryStates.Processing;
        queryStatus.Name = placementName;

        Debug.Log("Customized Level Solver: Building the Room ... ");

        // Kick off a thread to do process the queries
#if UNITY_EDITOR || !UNITY_WSA
        new System.Threading.Thread
#else
            System.Threading.Tasks.Task.Run
#endif
            (() =>
            {
                // Go through the queries in the list
                for (int i = 0; i < placementList.Count; ++i)
                {
                    // Do the query
                    bool success = PlaceObject(
                        placementName,
                        placementList[i].PlacementDefinition,
                        placementList[i].PlacementRules,
                        placementList[i].PlacementConstraints,
                        clearObjectsFirst,
                        true);

                    // Mark the result
                    queryStatus.CountSuccess = success ? (queryStatus.CountSuccess + 1) : queryStatus.CountSuccess;
                    queryStatus.CountFail = !success ? (queryStatus.CountFail + 1) : queryStatus.CountFail;
                }

                // Done
                queryStatus.State = QueryStates.Finished;
            }
        )
#if UNITY_EDITOR || !UNITY_WSA
            .Start()
#endif
            ;

        return true;
    }

    private bool PlaceObject(
            string placementName,
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
            List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
            List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null,
            bool clearObjectsFirst = true,
            bool isASync = false)
    {
        // Clear objects (if requested)
        if (!isASync && clearObjectsFirst)
        {
            // do nothing
            //ClearGeometry();
        }
        if (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
        {
            return false;
        }

        // New query
        if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(
                placementName,
                SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementDefinition),
                (placementRules != null) ? placementRules.Count : 0,
                ((placementRules != null) && (placementRules.Count > 0)) ? SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementRules.ToArray()) : IntPtr.Zero,
                (placementConstraints != null) ? placementConstraints.Count : 0,
                ((placementConstraints != null) && (placementConstraints.Count > 0)) ? SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementConstraints.ToArray()) : IntPtr.Zero,
                SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResultPtr()) > 0)
        {
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResult();
            if (!isASync)
            {
                // If not running async, we can just add the results to the draw list right now
                //AppState.Instance.ObjectPlacementDescription = placementName + " (1)";
                //float timeDelay = (float)placementResults.Count * AnimatedBox.DelayPerItem;
                float timeDelay = 0;
                placementResults.Add(new PlacementResult(timeDelay, placementResult.Clone() as SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult));
            }
            else
            {
                queryStatus.QueryResult.Add(placementResult.Clone() as SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult);
            }
            return true;
        }
        if (!isASync)
        {
            // don't care ASync
            //AppState.Instance.ObjectPlacementDescription = placementName + " (0)";
        }
        return false;
    }

    private void ProcessPlacementResults()
    {
        // Check it
        if (queryStatus.State != QueryStates.Finished)
        {
            return;
        }
        if (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
        {
            return;
        }

        // Clear results
        //ClearGeometry();

        // We will reject any above or below the ceiling/floor
        SpatialUnderstandingDll.Imports.QueryPlayspaceAlignment(SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignmentPtr());
        SpatialUnderstandingDll.Imports.PlayspaceAlignment alignment = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignment();

        // Copy over the results
        for (int i = 0; i < queryStatus.QueryResult.Count; ++i)
        {
            if ((queryStatus.QueryResult[i].Position.y < alignment.CeilingYValue) &&
                (queryStatus.QueryResult[i].Position.y > alignment.FloorYValue))
            {
                float timeDelay = 0; //(float)placementResults.Count * AnimatedBox.DelayPerItem;
                placementResults.Add(new PlacementResult(timeDelay, queryStatus.QueryResult[i].Clone() as SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult));
            }
        }

        // Text
        //AppState.Instance.ObjectPlacementDescription = queryStatus.Name + " (" + placementResults.Count + "/" + (queryStatus.CountSuccess + queryStatus.CountFail) + ")";

        // Mark done
        queryStatus.Reset();
    }


    public void Query_OnWall()
    {
        List<PlacementQuery> placementQuery = new List<PlacementQuery>();
        for (int i = 0; i < 1; ++i)
        {
            float halfDimSize = UnityEngine.Random.Range(0.3f, 0.6f);
            placementQuery.Add(
                new PlacementQuery(SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnWall(new Vector3(halfDimSize, halfDimSize * 0.5f, 0.05f), 0.5f, 3.0f),
                                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>() {
                                            SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(halfDimSize * 4.0f),
                                    }));
        }
        PlaceObjectAsync("OnWall", placementQuery);
    }


    public bool InitializeSolver()
    {
        if (IsSolverInitialized ||
            !SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
        {
            return IsSolverInitialized;
        }

        if (SpatialUnderstandingDllObjectPlacement.Solver_Init() == 1)
        {
            IsSolverInitialized = true;
        }
        return IsSolverInitialized;
    }

    private void Update()
    {
        // Can't do any of this till we're done with the scanning phase
        if (SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Done)
        {
            return;
        }

        // Make sure the solver has been initialized
        if (!IsSolverInitialized &&
            SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
        {
            InitializeSolver();
        }

        // Constraint queries
        //if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done)
        //{
        //    Update_Queries();
        //}

        // Handle async query results
        ProcessPlacementResults();

        // Lines: Begin
        //LineDraw_Begin();

        //// Drawers
        //bool needsUpdate = false;
        //needsUpdate |= Draw_PlacementResults();

        //// Lines: Finish up
        //LineDraw_End(needsUpdate);
    }
}
