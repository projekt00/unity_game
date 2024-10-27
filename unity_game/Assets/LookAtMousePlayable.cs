using UnityEngine;
using UnityEngine.Playables;

public class LookAtMousePlayable : PlayableAsset
{
    public ExposedReference<Transform> headTransform;
    public ExposedReference<Camera> mainCamera;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LookAtMouseBehaviour>.Create(graph);

        LookAtMouseBehaviour behaviour = playable.GetBehaviour();
        behaviour.headTransform = headTransform.Resolve(graph.GetResolver());
        behaviour.mainCamera = mainCamera.Resolve(graph.GetResolver());

        return playable;
    }
}

public class LookAtMouseBehaviour : PlayableBehaviour
{
    public Transform headTransform;
    public Camera mainCamera;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (headTransform == null || mainCamera == null) return;

        // Uzyskaj pozycjê kursora myszy w przestrzeni œwiata
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane + 1f));

        // Oblicz kierunek i interpoluj obrót g³owy
        Vector3 direction = worldPosition - headTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        headTransform.rotation = Quaternion.Slerp(headTransform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}