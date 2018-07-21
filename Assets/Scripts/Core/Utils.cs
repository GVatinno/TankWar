

using UnityEngine;

class Utils
{

    public static readonly int LayerTank = LayerMask.NameToLayer("Tank");
    public static readonly int LayerGround = LayerMask.NameToLayer("Ground");
    public static readonly int LayerObstacle = LayerMask.NameToLayer("Obstacle");

	public static float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}