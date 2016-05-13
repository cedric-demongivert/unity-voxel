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
      if (emitionTime <= 0) return;

      timeLeft += Time.deltaTime;

      while(timeLeft > emitionTime)
      {
        for(int i = -1; i <= 1; ++i)
        {
          for (int j = -1; j <= 1; ++j)
          {
            this.simulation.Add(position + new Vector3(i, j, 0), speed);
          }
        }
        timeLeft -= emitionTime;
      }
    }
  }
}
