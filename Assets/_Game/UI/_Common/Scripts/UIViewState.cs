using System.Collections;
using CustomUtilities.Attributes;
using UnityEngine;
using ProjectCore.UI;
using ProjectCore.StateMachine;
using ProjectCore.Events;
using ProjectCore.Utilities;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "UIViewState", menuName = "ProjectCore/State Machine/States/UI View State")]
public class UIViewState : State
{
    [Header("Configuration")]
    [SerializeField] protected AssetReferenceGameObject ViewReference;
    [SerializeField] protected GameEventWithInt EventWithCloseReason;
    
    // Runtime Data
    [System.NonSerialized] public Camera RenderCamera;
    [System.NonSerialized] public UICloseReasons ViewSourceCloseReason;

    protected UIView View;
    private AsyncOperationHandle<GameObject> _loadingHandle;

    public override IEnumerator Init(IState listener)
    {
        yield return base.Init(listener);

        yield return AddressablesHelper.Instantiate<UIView>(
            ViewReference,
            (viewComponent, handle) =>
            {
                View = viewComponent;
                _loadingHandle = handle;

                // Inject Camera (Specific logic for UIViewState)
                Canvas canvas = viewComponent.GetComponent<Canvas>();
                if (canvas != null && RenderCamera != null)
                {
                    canvas.worldCamera = RenderCamera;
                }
            },
            null,
            this
        );
    }

    public override IEnumerator Execute()
    {
        yield return base.Execute();
        if (View != null)
        {
            yield return View.Show(false);
        }
        
    }

    public override IEnumerator Exit()
    {
        if (View != null)
        {
            yield return View.Hide(true); // Helper to animate out
            
            if (_loadingHandle.IsValid())
            {
                Addressables.ReleaseInstance(_loadingHandle);
            }
            View = null;
        }
        yield return base.Exit();
    }

    public override IEnumerator Pause()
    {
        if (View != null)
        {
            yield return View.Hide(false); // Hide but keep in memory
        }
        yield return base.Pause();
    }

    public override IEnumerator Resume()
    {
        // If view was lost (memory cleanup), reload it
        if (View == null)
        {
            // Re-run Init logic to reload
            yield return Init(null); 
        }

        if (View != null)
        {
            yield return View.Show(true);
        }
        yield return base.Resume();
    }

    public override IEnumerator Cleanup()
    {
        if (View != null)
        {
            // Force immediate destroy if needed, though ReleaseInstance is preferred
            if (_loadingHandle.IsValid())
            {
                Addressables.ReleaseInstance(_loadingHandle);
            }
            View = null;
        }
        yield return base.Cleanup();
    }
    
    [Button]
    public virtual void CloseView()
    {
        //Close with Default or Runtime Reason Passed In
        CloseView(ViewSourceCloseReason);
    }

    [Button]
    public virtual void CloseView (UICloseReasons reasons)
    {
        if (EventWithCloseReason != null)
        {
            EventWithCloseReason.Invoke((int)reasons);
        }
    }
}
