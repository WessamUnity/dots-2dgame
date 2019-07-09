using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class InputTrackingSystem : ComponentSystem
{
	// cache the main camera to avoid repeated calls to Camera.main
	private Camera camera;
	private Camera MainCamera
	{
		get
		{
			if (camera == null)
			{
				camera = Camera.main;
			}
			return camera;
		}
	}

	protected override void OnUpdate()
	{
		// get mouse data and save it in components
		float2 currentLoc = new float2(Input.mousePosition.x, Input.mousePosition.y);
		bool bMouseDown = Input.GetMouseButtonDown(0);
		bool bMouseUp = Input.GetMouseButtonUp(0);
		float timeNow = Time.time;

		Entities.ForEach((Entity e, ref InputLocations inputLocations, ref InputTimes inputTimes, ref ProjectedInputs projectedInputs, ref InputEvents inputEvents) =>
		{
			inputLocations.Current = currentLoc;

			Vector3 mousePos = new Vector3(currentLoc.x, currentLoc.y, 2);
			mousePos = MainCamera.ScreenToWorldPoint(mousePos);
			float2 worldLoc = new float2(mousePos.x, mousePos.z);
			projectedInputs.Current = worldLoc;

			inputEvents.PrimaryDown = bMouseDown;
			inputEvents.PrimaryUp = bMouseUp;

			if (bMouseDown)
			{
				inputLocations.LastPrimaryDown = currentLoc;
				inputTimes.LastPrimaryDown = timeNow;
				projectedInputs.LastPrimaryDown = projectedInputs.Current;
			}
			else if (bMouseUp)
			{
				inputLocations.LastPrimaryUp = currentLoc;
				inputTimes.LastPrimaryUp = timeNow;
				projectedInputs.LastPrimaryUp = projectedInputs.Current;
			}
		});
	}
}