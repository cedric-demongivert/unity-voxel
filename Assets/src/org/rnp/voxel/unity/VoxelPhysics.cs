using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.unity.components.meshes;

namespace org.rnp.voxel.unity
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Physics methods for the voxel engine.
  /// </summary>
  public sealed class VoxelPhysics
  {
    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastVoxelHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers)
    {
      RaycastHit hit;
      bool result = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

      if(result)
      {
        hitInfo = new RaycastVoxelHit(hit);
      }
      else
      {
        hitInfo = null;
      }

      return result;
    }

    public static bool Raycast(Ray ray, out RaycastVoxelHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers)
    {
      RaycastHit hit;
      bool result = Physics.Raycast(ray, out hit, maxDistance, layerMask);

      if (result)
      {
        hitInfo = new RaycastVoxelHit(hit);
      }
      else
      {
        hitInfo = null;
      }

      return result;
    }
  }
}
