using org.rnp.voxel.mesh;
using org.rnp.voxel.translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.physics
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Some collision detection methods for voxel meshes.
  /// </summary>
  public static class VoxelPhysics
  {
    /// <summary>
    ///   Return true if a Ray collide an unity BoxCollider
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="box"></param>
    /// <returns></returns>
    public static bool IsRayCollideBox(Ray ray, BoxCollider box)
    {
      RaycastHit hitInfo;
      return box.Raycast(ray, out hitInfo, float.MaxValue);
    }

    /// <summary>
    ///   Check if a Ray collide a VoxelMesh by using of a VoxelFilter
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="filter"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    public static bool IsRayCollideVoxelMesh(Ray ray, VoxelFilter filter, out VoxelHit hitInfo)
    {
      if (filter == null || filter.Mesh == null || filter.Mesh.Mesh == null)
      {
        hitInfo = null;
        return false;
      }

      return VoxelPhysics.IsRayCollideVoxelMesh(ray, filter.Mesh.Mesh, filter.transform.FindChild("Genered Translator").transform, out hitInfo);
    }

    /// <summary>
    ///   Check if a Ray Collide a VoxelMesh by using of a VoxelMesh and a Transform object
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="mesh"></param>
    /// <param name="transform"></param>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    public static bool IsRayCollideVoxelMesh(Ray ray, VoxelMesh mesh, Transform transform, out VoxelHit hitInfo)
    {
      if (mesh == null)
      {
        hitInfo = null;
        return false;
      }

      GameObject tmpObject = new GameObject();

      BoxCollider box = tmpObject.AddComponent<BoxCollider>();

      box.center = ((Vector3)mesh.Start.Add(mesh.Start.Add(mesh.Dimensions))) / 2f;
      box.size = new Vector3(mesh.Dimensions.Width, mesh.Dimensions.Height, mesh.Dimensions.Depth);
      
      ray.origin = transform.InverseTransformPoint(ray.origin);
      ray.direction = transform.InverseTransformVector(ray.direction);

      bool result = false;

      if(VoxelPhysics.IsRayCollideBox(ray, box))
      {
        result = VoxelPhysics.ResolveNode(ray, mesh, mesh.Start, mesh.Dimensions, box, out hitInfo);
      }
      else
      {
        hitInfo = null;
      }

      GameObject.Destroy(tmpObject);

      return result;
    }

    /// <summary>
    ///   Resolve an octree node collision.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="mesh"></param>
    /// <param name="transform"></param>
    /// <param name="start"></param>
    /// <param name="dimensions"></param>
    /// <param name="box"></param>
    /// <returns></returns>
    private static bool ResolveNode(Ray ray, VoxelMesh mesh, VoxelLocation start, Dimensions3D dimensions, BoxCollider box, out VoxelHit hit)
    {
      if(dimensions.Width <= 0 || dimensions.Height <= 0 || dimensions.Depth <= 0)
      {
        hit = null;
        return false;
      }
      else if(dimensions.Width == 1 && dimensions.Height == 1 && dimensions.Depth == 1)
      {
        if(Voxels.IsEmpty(mesh[start]))
        {
          hit = null;
          return false;
        }
        else
        {
          box.center = ((Vector3)start.Add(start.Add(dimensions))) / 2f;
          box.size = new Vector3(dimensions.Width, dimensions.Height, dimensions.Depth);

          RaycastHit hitInfo;
          box.Raycast(ray, out hitInfo, float.MaxValue);

          hit = new VoxelHit(start, hitInfo);

          return true;
        }
      }
      else
      {
        return VoxelPhysics.DoResolution(ray, mesh, start, dimensions, box, out hit);
      }
    }

    private static bool DoResolution(Ray ray, VoxelMesh mesh, VoxelLocation start, Dimensions3D dimensions, BoxCollider box, out VoxelHit hit)
    {
      Dimensions3D baseChildDimension = dimensions.Div(2);

      bool result = false;
      VoxelHit resultHit = null;

      for (int i = 0; i < 2; ++i)
      {
        for (int j = 0; j < 2; ++j)
        {
          for (int k = 0; k < 2; ++k)
          {
            // Compute sub-node dimensions
            Dimensions3D childDimension = new Dimensions3D(
              (i == 1) ? dimensions.Width - baseChildDimension.Width : baseChildDimension.Width,
              (j == 1) ? dimensions.Height - baseChildDimension.Height : baseChildDimension.Height,
              (k == 1) ? dimensions.Depth - baseChildDimension.Depth : baseChildDimension.Depth
            );

            // Compute sub-node start
            VoxelLocation childStart = new VoxelLocation(
              (i == 1) ? start.X + baseChildDimension.Width : start.X,
              (j == 1) ? start.Y + baseChildDimension.Height : start.Y,
              (k == 1) ? start.Z + baseChildDimension.Depth : start.Z
            );

            // Apply to the boxCollider
            box.center = ((Vector3)childStart.Add(childStart.Add(childDimension))) / 2f;
            box.size = new Vector3(childDimension.Width, childDimension.Height, childDimension.Depth);

            // Compute collision
            if (VoxelPhysics.IsRayCollideBox(ray, box))
            {
              VoxelHit tmpHit = null;

              if(VoxelPhysics.ResolveNode(ray, mesh, childStart, childDimension, box, out tmpHit))
              {
                result = true;

                if(resultHit == null || resultHit.HitInfo.distance > tmpHit.HitInfo.distance)
                {
                  resultHit = tmpHit;
                }
              }
            }
          }
        }
      }
      
      hit = resultHit;

      return result;
    }
  }
}
