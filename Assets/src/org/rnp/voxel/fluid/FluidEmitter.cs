using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.fluid
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Regulary adding fluid to a simulation.
  /// </summary>
  public class FluidEmitter : MonoBehaviour
  {
    /// <summary>
    ///   Simulation in with the emitter add fluid.
    /// </summary>
    public FluidSimulation simulation;

    /// <summary>
    ///   Location of emition in space.
    /// </summary>
    public Vector3 position = Vector3.zero;

    /// <summary>
    ///   Speed of emitted amount of fluid.
    /// </summary>
    public Vector3 speed = Vector3.down;

    /// <summary>
    ///   Quantity of emitted fluid.
    /// </summary>
    public float quantity = 1f;

    /// <summary>
    ///   Time between two emitions.
    /// </summary>
    public float emitionTime = 1f;

    private float timeLeft;

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    void Start()
    {
      timeLeft = 0;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    void Update()
    {
      timeLeft += Time.deltaTime;

      while(timeLeft > emitionTime)
      {
        this.simulation.Add(quantity, position, speed);
        timeLeft -= emitionTime;
      }
    }
  }
}
