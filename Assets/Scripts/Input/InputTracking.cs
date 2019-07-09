using Unity.Entities;
using Unity.Mathematics;

public struct InputLocations : IComponentData
{
	public float2 Current;
	public float2 LastPrimaryDown;
	public float2 LastPrimaryUp;
}

public struct InputTimes : IComponentData
{
	public float LastPrimaryDown;
	public float LastPrimaryUp;
}

public struct InputEvents : IComponentData
{
	public bool PrimaryDown;
	public bool PrimaryUp;
}

public struct ProjectedInputs : IComponentData
{
	public float2 Current;
	public float2 LastPrimaryDown;
	public float2 LastPrimaryUp;
}