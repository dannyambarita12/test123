using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectorySpawner : Singleton<TrajectorySpawner>
{
    public const string TRAJECTORY_PREFAB_PATH = "TemporaryLineVisual";

    private UnitDiceTrajectoryDrawer _cachedTrajectoryDrawer;

    public static UnitDiceTrajectoryDrawer Spawn(Transform startPoint, Transform endPoint)
    {
        return Instance.SpawnTrajectory(startPoint, endPoint);
    }

    public static UnitDiceTrajectoryDrawer Spawn()
    {
        return Instance.SpawnTrajectory();
    }

    private UnitDiceTrajectoryDrawer SpawnTrajectory(Transform startPoint, Transform endPoint)
    {
        if (_cachedTrajectoryDrawer == null)
            _cachedTrajectoryDrawer = Resources.Load<UnitDiceTrajectoryDrawer>(TRAJECTORY_PREFAB_PATH);
        
        var trajectory = LeanPool.Spawn(_cachedTrajectoryDrawer, Vector3.zero, Quaternion.identity);
        trajectory.SetOrigin(startPoint);
        trajectory.SetTarget(endPoint);
        trajectory.DrawTrajectory();

        return trajectory;
    }

    private UnitDiceTrajectoryDrawer SpawnTrajectory()
    {
        if (_cachedTrajectoryDrawer == null)
            _cachedTrajectoryDrawer = Resources.Load<UnitDiceTrajectoryDrawer>(TRAJECTORY_PREFAB_PATH);

        return LeanPool.Spawn(_cachedTrajectoryDrawer, Vector3.zero, Quaternion.identity);
    }
}
