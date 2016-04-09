﻿using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.translator.cubic
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Transform a voxel mesh part into a marshing mesh.
  ///   Accordingly to lookup tables exposed at : http://paulbourke.net/geometry/polygonise/
  /// </summary>
  [RequireComponent(typeof(MeshFilter))]
  [ExecuteInEditMode]
  public abstract class MarshingVoxelMeshBuilder : Translator
  {
    /// <summary>
    ///   Computed mesh.
    /// </summary>
    private Mesh _mesh;

    /// <summary>
    ///   Builded mesh vertices.
    /// </summary>
    private List<Vector3> _meshVertices;

    /// <summary>
    ///   Builded mesh vertices colors.
    /// </summary>
    private List<Color32> _meshVerticesColor;

    /// <summary>
    ///   Builded mesh texture coordinates.
    /// </summary>
    private List<Vector2> _meshUV;

    /// <summary>
    ///   Builded mesh triangles.
    /// </summary>
    private List<int> _meshTriangles;

    /// <summary>
    ///   Marshing cases facets (256 x 13), according to this
    ///   lookup table : http://paulbourke.net/geometry/polygonise/table2.txt
    /// </summary>
    private static int[,] marshingCases = new int[,] {
      // First table
      /*{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 3, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 9, 0, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 3, 1, 8, 1, 9,-1,-1,-1,-1,-1,-1,-1},
      {10, 1, 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 3, 0, 1, 2,10,-1,-1,-1,-1,-1,-1,-1},
      { 9, 0, 2, 9, 2,10,-1,-1,-1,-1,-1,-1,-1},
      { 3, 2, 8, 2,10, 8, 8,10, 9,-1,-1,-1,-1},
      {11, 2, 3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      {11, 2, 0,11, 0, 8,-1,-1,-1,-1,-1,-1,-1},
      {11, 2, 3, 0, 1, 9,-1,-1,-1,-1,-1,-1,-1},
      { 2, 1,11, 1, 9,11,11, 9, 8,-1,-1,-1,-1},
      {10, 1, 3,10, 3,11,-1,-1,-1,-1,-1,-1,-1},
      { 1, 0,10, 0, 8,10,10, 8,11,-1,-1,-1,-1},
      { 0, 3, 9, 3,11, 9, 9,11,10,-1,-1,-1,-1},
      { 8,10, 9, 8,11,10,-1,-1,-1,-1,-1,-1,-1},
      { 8, 4, 7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 3, 0, 4, 3, 4, 7,-1,-1,-1,-1,-1,-1,-1},
      { 1, 9, 0, 8, 4, 7,-1,-1,-1,-1,-1,-1,-1},
      { 9, 4, 1, 4, 7, 1, 1, 7, 3,-1,-1,-1,-1},
      {10, 1, 2, 8, 4, 7,-1,-1,-1,-1,-1,-1,-1},
      { 2,10, 1, 0, 4, 7, 0, 7, 3,-1,-1,-1,-1},
      { 4, 7, 8, 0, 2,10, 0,10, 9,-1,-1,-1,-1},
      { 2, 7, 3, 2, 9, 7, 7, 9, 4, 2,10, 9,-1},
      { 2, 3,11, 7, 8, 4,-1,-1,-1,-1,-1,-1,-1},
      { 7,11, 4,11, 2, 4, 4, 2, 0,-1,-1,-1,-1},
      { 3,11, 2, 4, 7, 8, 9, 0, 1,-1,-1,-1,-1},
      { 2, 7,11, 2, 1, 7, 1, 4, 7, 1, 9, 4,-1},
      { 8, 4, 7,11,10, 1,11, 1, 3,-1,-1,-1,-1},
      {11, 4, 7, 1, 4,11, 1,11,10, 1, 0, 4,-1},
      { 3, 8, 0, 7,11, 4,11, 9, 4,11,10, 9,-1},
      { 7,11, 4, 4,11, 9,11,10, 9,-1,-1,-1,-1},
      { 9, 5, 4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 3, 0, 8, 4, 9, 5,-1,-1,-1,-1,-1,-1,-1},
      { 5, 4, 0, 5, 0, 1,-1,-1,-1,-1,-1,-1,-1},
      { 4, 8, 5, 8, 3, 5, 5, 3, 1,-1,-1,-1,-1},
      { 2,10, 1, 9, 5, 4,-1,-1,-1,-1,-1,-1,-1},
      { 0, 8, 3, 5, 4, 9,10, 1, 2,-1,-1,-1,-1},
      {10, 5, 2, 5, 4, 2, 2, 4, 0,-1,-1,-1,-1},
      { 3, 4, 8, 3, 2, 4, 2, 5, 4, 2,10, 5,-1},
      {11, 2, 3, 9, 5, 4,-1,-1,-1,-1,-1,-1,-1},
      { 9, 5, 4, 8,11, 2, 8, 2, 0,-1,-1,-1,-1},
      { 3,11, 2, 1, 5, 4, 1, 4, 0,-1,-1,-1,-1},
      { 8, 5, 4, 2, 5, 8, 2, 8,11, 2, 1, 5,-1},
      { 5, 4, 9, 1, 3,11, 1,11,10,-1,-1,-1,-1},
      { 0, 9, 1, 4, 8, 5, 8,10, 5, 8,11,10,-1},
      { 3, 4, 0, 3,10, 4, 4,10, 5, 3,11,10,-1},
      { 4, 8, 5, 5, 8,10, 8,11,10,-1,-1,-1,-1},
      { 9, 5, 7, 9, 7, 8,-1,-1,-1,-1,-1,-1,-1},
      { 0, 9, 3, 9, 5, 3, 3, 5, 7,-1,-1,-1,-1},
      { 8, 0, 7, 0, 1, 7, 7, 1, 5,-1,-1,-1,-1},
      { 1, 7, 3, 1, 5, 7,-1,-1,-1,-1,-1,-1,-1},
      { 1, 2,10, 5, 7, 8, 5, 8, 9,-1,-1,-1,-1},
      { 9, 1, 0,10, 5, 2, 5, 3, 2, 5, 7, 3,-1},
      { 5, 2,10, 8, 2, 5, 8, 5, 7, 8, 0, 2,-1},
      {10, 5, 2, 2, 5, 3, 5, 7, 3,-1,-1,-1,-1},
      {11, 2, 3, 8, 9, 5, 8, 5, 7,-1,-1,-1,-1},
      { 9, 2, 0, 9, 7, 2, 2, 7,11, 9, 5, 7,-1},
      { 0, 3, 8, 2, 1,11, 1, 7,11, 1, 5, 7,-1},
      { 2, 1,11,11, 1, 7, 1, 5, 7,-1,-1,-1,-1},
      { 3, 9, 1, 3, 8, 9, 7,11,10, 7,10, 5,-1},
      { 9, 1, 0,10, 7,11,10, 5, 7,-1,-1,-1,-1},
      { 3, 8, 0, 7,10, 5, 7,11,10,-1,-1,-1,-1},
      {11, 5, 7,11,10, 5,-1,-1,-1,-1,-1,-1,-1},
      {10, 6, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 3, 0,10, 6, 5,-1,-1,-1,-1,-1,-1,-1},
      { 0, 1, 9, 5,10, 6,-1,-1,-1,-1,-1,-1,-1},
      {10, 6, 5, 9, 8, 3, 9, 3, 1,-1,-1,-1,-1},
      { 1, 2, 6, 1, 6, 5,-1,-1,-1,-1,-1,-1,-1},
      { 0, 8, 3, 2, 6, 5, 2, 5, 1,-1,-1,-1,-1},
      { 5, 9, 6, 9, 0, 6, 6, 0, 2,-1,-1,-1,-1},
      { 9, 6, 5, 3, 6, 9, 3, 9, 8, 3, 2, 6,-1},
      { 3,11, 2,10, 6, 5,-1,-1,-1,-1,-1,-1,-1},
      { 6, 5,10, 2, 0, 8, 2, 8,11,-1,-1,-1,-1},
      { 1, 9, 0, 6, 5,10,11, 2, 3,-1,-1,-1,-1},
      { 1,10, 2, 5, 9, 6, 9,11, 6, 9, 8,11,-1},
      {11, 6, 3, 6, 5, 3, 3, 5, 1,-1,-1,-1,-1},
      { 0, 5, 1, 0,11, 5, 5,11, 6, 0, 8,11,-1},
      { 0, 5, 9, 0, 3, 5, 3, 6, 5, 3,11, 6,-1},
      { 5, 9, 6, 6, 9,11, 9, 8,11,-1,-1,-1,-1},
      {10, 6, 5, 4, 7, 8,-1,-1,-1,-1,-1,-1,-1},
      { 5,10, 6, 7, 3, 0, 7, 0, 4,-1,-1,-1,-1},
      { 5,10, 6, 0, 1, 9, 8, 4, 7,-1,-1,-1,-1},
      { 4, 5, 9, 6, 7,10, 7, 1,10, 7, 3, 1,-1},
      { 7, 8, 4, 5, 1, 2, 5, 2, 6,-1,-1,-1,-1},
      { 4, 1, 0, 4, 5, 1, 6, 7, 3, 6, 3, 2,-1},
      { 9, 4, 5, 8, 0, 7, 0, 6, 7, 0, 2, 6,-1},
      { 4, 5, 9, 6, 3, 2, 6, 7, 3,-1,-1,-1,-1},
      { 7, 8, 4, 2, 3,11,10, 6, 5,-1,-1,-1,-1},
      {11, 6, 7,10, 2, 5, 2, 4, 5, 2, 0, 4,-1},
      {11, 6, 7, 8, 0, 3, 1,10, 2, 9, 4, 5,-1},
      { 6, 7,11, 1,10, 2, 9, 4, 5,-1,-1,-1,-1},
      { 6, 7,11, 4, 5, 8, 5, 3, 8, 5, 1, 3,-1},
      { 6, 7,11, 4, 1, 0, 4, 5, 1,-1,-1,-1,-1},
      { 4, 5, 9, 3, 8, 0,11, 6, 7,-1,-1,-1,-1},
      { 9, 4, 5, 7,11, 6,-1,-1,-1,-1,-1,-1,-1},
      {10, 6, 4,10, 4, 9,-1,-1,-1,-1,-1,-1,-1},
      { 8, 3, 0, 9,10, 6, 9, 6, 4,-1,-1,-1,-1},
      { 1,10, 0,10, 6, 0, 0, 6, 4,-1,-1,-1,-1},
      { 8, 6, 4, 8, 1, 6, 6, 1,10, 8, 3, 1,-1},
      { 9, 1, 4, 1, 2, 4, 4, 2, 6,-1,-1,-1,-1},
      { 1, 0, 9, 3, 2, 8, 2, 4, 8, 2, 6, 4,-1},
      { 2, 4, 0, 2, 6, 4,-1,-1,-1,-1,-1,-1,-1},
      { 3, 2, 8, 8, 2, 4, 2, 6, 4,-1,-1,-1,-1},
      { 2, 3,11, 6, 4, 9, 6, 9,10,-1,-1,-1,-1},
      { 0,10, 2, 0, 9,10, 4, 8,11, 4,11, 6,-1},
      {10, 2, 1,11, 6, 3, 6, 0, 3, 6, 4, 0,-1},
      {10, 2, 1,11, 4, 8,11, 6, 4,-1,-1,-1,-1},
      { 1, 4, 9,11, 4, 1,11, 1, 3,11, 6, 4,-1},
      { 0, 9, 1, 4,11, 6, 4, 8,11,-1,-1,-1,-1},
      {11, 6, 3, 3, 6, 0, 6, 4, 0,-1,-1,-1,-1},
      { 8, 6, 4, 8,11, 6,-1,-1,-1,-1,-1,-1,-1},
      { 6, 7,10, 7, 8,10,10, 8, 9,-1,-1,-1,-1},
      { 9, 3, 0, 6, 3, 9, 6, 9,10, 6, 7, 3,-1},
      { 6, 1,10, 6, 7, 1, 7, 0, 1, 7, 8, 0,-1},
      { 6, 7,10,10, 7, 1, 7, 3, 1,-1,-1,-1,-1},
      { 7, 2, 6, 7, 9, 2, 2, 9, 1, 7, 8, 9,-1},
      { 1, 0, 9, 3, 6, 7, 3, 2, 6,-1,-1,-1,-1},
      { 8, 0, 7, 7, 0, 6, 0, 2, 6,-1,-1,-1,-1},
      { 2, 7, 3, 2, 6, 7,-1,-1,-1,-1,-1,-1,-1},
      { 7,11, 6, 3, 8, 2, 8,10, 2, 8, 9,10,-1},
      {11, 6, 7,10, 0, 9,10, 2, 0,-1,-1,-1,-1},
      { 2, 1,10, 7,11, 6, 8, 0, 3,-1,-1,-1,-1},
      { 1,10, 2, 6, 7,11,-1,-1,-1,-1,-1,-1,-1},
      { 7,11, 6, 3, 9, 1, 3, 8, 9,-1,-1,-1,-1},
      { 9, 1, 0,11, 6, 7,-1,-1,-1,-1,-1,-1,-1},
      { 0, 3, 8,11, 6, 7,-1,-1,-1,-1,-1,-1,-1},
      {11, 6, 7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      {11, 7, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 0, 8, 3,11, 7, 6,-1,-1,-1,-1,-1,-1,-1},
      { 9, 0, 1,11, 7, 6,-1,-1,-1,-1,-1,-1,-1},
      { 7, 6,11, 3, 1, 9, 3, 9, 8,-1,-1,-1,-1},
      { 1, 2,10, 6,11, 7,-1,-1,-1,-1,-1,-1,-1},
      { 2,10, 1, 7, 6,11, 8, 3, 0,-1,-1,-1,-1},
      {11, 7, 6,10, 9, 0,10, 0, 2,-1,-1,-1,-1},
      { 7, 6,11, 3, 2, 8, 8, 2,10, 8,10, 9,-1},
      { 2, 3, 7, 2, 7, 6,-1,-1,-1,-1,-1,-1,-1},
      { 8, 7, 0, 7, 6, 0, 0, 6, 2,-1,-1,-1,-1},
      { 1, 9, 0, 3, 7, 6, 3, 6, 2,-1,-1,-1,-1},
      { 7, 6, 2, 7, 2, 9, 2, 1, 9, 7, 9, 8,-1},
      { 6,10, 7,10, 1, 7, 7, 1, 3,-1,-1,-1,-1},
      { 6,10, 1, 6, 1, 7, 7, 1, 0, 7, 0, 8,-1},
      { 9, 0, 3, 6, 9, 3, 6,10, 9, 6, 3, 7,-1},
      { 6,10, 7, 7,10, 8,10, 9, 8,-1,-1,-1,-1},
      { 8, 4, 6, 8, 6,11,-1,-1,-1,-1,-1,-1,-1},
      {11, 3, 6, 3, 0, 6, 6, 0, 4,-1,-1,-1,-1},
      { 0, 1, 9, 4, 6,11, 4,11, 8,-1,-1,-1,-1},
      { 1, 9, 4,11, 1, 4,11, 3, 1,11, 4, 6,-1},
      {10, 1, 2,11, 8, 4,11, 4, 6,-1,-1,-1,-1},
      {10, 1, 2,11, 3, 6, 6, 3, 0, 6, 0, 4,-1},
      { 0, 2,10, 0,10, 9, 4,11, 8, 4, 6,11,-1},
      { 2,11, 3, 6, 9, 4, 6,10, 9,-1,-1,-1,-1},
      { 3, 8, 2, 8, 4, 2, 2, 4, 6,-1,-1,-1,-1},
      { 2, 0, 4, 2, 4, 6,-1,-1,-1,-1,-1,-1,-1},
      { 1, 9, 0, 3, 8, 2, 2, 8, 4, 2, 4, 6,-1},
      { 9, 4, 1, 1, 4, 2, 4, 6, 2,-1,-1,-1,-1},
      { 8, 4, 6, 8, 6, 1, 6,10, 1, 8, 1, 3,-1},
      { 1, 0,10,10, 0, 6, 0, 4, 6,-1,-1,-1,-1},
      { 8, 0, 3, 9, 6,10, 9, 4, 6,-1,-1,-1,-1},
      {10, 4, 6,10, 9, 4,-1,-1,-1,-1,-1,-1,-1},
      { 9, 5, 4, 7, 6,11,-1,-1,-1,-1,-1,-1,-1},
      { 4, 9, 5, 3, 0, 8,11, 7, 6,-1,-1,-1,-1},
      { 6,11, 7, 4, 0, 1, 4, 1, 5,-1,-1,-1,-1},
      { 6,11, 7, 4, 8, 5, 5, 8, 3, 5, 3, 1,-1},
      { 6,11, 7, 1, 2,10, 9, 5, 4,-1,-1,-1,-1},
      {11, 7, 6, 8, 3, 0, 1, 2,10, 9, 5, 4,-1},
      {11, 7, 6,10, 5, 2, 2, 5, 4, 2, 4, 0,-1},
      { 7, 4, 8, 2,11, 3,10, 5, 6,-1,-1,-1,-1},
      { 4, 9, 5, 6, 2, 3, 6, 3, 7,-1,-1,-1,-1},
      { 9, 5, 4, 8, 7, 0, 0, 7, 6, 0, 6, 2,-1},
      { 4, 0, 1, 4, 1, 5, 6, 3, 7, 6, 2, 3,-1},
      { 7, 4, 8, 5, 2, 1, 5, 6, 2,-1,-1,-1,-1},
      { 4, 9, 5, 6,10, 7, 7,10, 1, 7, 1, 3,-1},
      { 5, 6,10, 0, 9, 1, 8, 7, 4,-1,-1,-1,-1},
      { 5, 6,10, 7, 0, 3, 7, 4, 0,-1,-1,-1,-1},
      {10, 5, 6, 4, 8, 7,-1,-1,-1,-1,-1,-1,-1},
      { 5, 6, 9, 6,11, 9, 9,11, 8,-1,-1,-1,-1},
      { 0, 9, 5, 0, 5, 3, 3, 5, 6, 3, 6,11,-1},
      { 0, 1, 5, 0, 5,11, 5, 6,11, 0,11, 8,-1},
      {11, 3, 6, 6, 3, 5, 3, 1, 5,-1,-1,-1,-1},
      { 1, 2,10, 5, 6, 9, 9, 6,11, 9,11, 8,-1},
      { 1, 0, 9, 6,10, 5,11, 3, 2,-1,-1,-1,-1},
      { 6,10, 5, 2, 8, 0, 2,11, 8,-1,-1,-1,-1},
      { 3, 2,11,10, 5, 6,-1,-1,-1,-1,-1,-1,-1},
      { 9, 5, 6, 3, 9, 6, 3, 8, 9, 3, 6, 2,-1},
      { 5, 6, 9, 9, 6, 0, 6, 2, 0,-1,-1,-1,-1},
      { 0, 3, 8, 2, 5, 6, 2, 1, 5,-1,-1,-1,-1},
      { 1, 6, 2, 1, 5, 6,-1,-1,-1,-1,-1,-1,-1},
      {10, 5, 6, 9, 3, 8, 9, 1, 3,-1,-1,-1,-1},
      { 0, 9, 1, 5, 6,10,-1,-1,-1,-1,-1,-1,-1},
      { 8, 0, 3,10, 5, 6,-1,-1,-1,-1,-1,-1,-1},
      {10, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      {11, 7, 5,11, 5,10,-1,-1,-1,-1,-1,-1,-1},
      { 3, 0, 8, 7, 5,10, 7,10,11,-1,-1,-1,-1},
      { 9, 0, 1,10,11, 7,10, 7, 5,-1,-1,-1,-1},
      { 3, 1, 9, 3, 9, 8, 7,10,11, 7, 5,10,-1},
      { 2,11, 1,11, 7, 1, 1, 7, 5,-1,-1,-1,-1},
      { 0, 8, 3, 2,11, 1, 1,11, 7, 1, 7, 5,-1},
      { 9, 0, 2, 9, 2, 7, 2,11, 7, 9, 7, 5,-1},
      {11, 3, 2, 8, 5, 9, 8, 7, 5,-1,-1,-1,-1},
      {10, 2, 5, 2, 3, 5, 5, 3, 7,-1,-1,-1,-1},
      { 5,10, 2, 8, 5, 2, 8, 7, 5, 8, 2, 0,-1},
      { 9, 0, 1,10, 2, 5, 5, 2, 3, 5, 3, 7,-1},
      { 1,10, 2, 5, 8, 7, 5, 9, 8,-1,-1,-1,-1},
      { 1, 3, 7, 1, 7, 5,-1,-1,-1,-1,-1,-1,-1},
      { 8, 7, 0, 0, 7, 1, 7, 5, 1,-1,-1,-1,-1},
      { 0, 3, 9, 9, 3, 5, 3, 7, 5,-1,-1,-1,-1},
      { 9, 7, 5, 9, 8, 7,-1,-1,-1,-1,-1,-1,-1},
      { 4, 5, 8, 5,10, 8, 8,10,11,-1,-1,-1,-1},
      { 3, 0, 4, 3, 4,10, 4, 5,10, 3,10,11,-1},
      { 0, 1, 9, 4, 5, 8, 8, 5,10, 8,10,11,-1},
      { 5, 9, 4, 1,11, 3, 1,10,11,-1,-1,-1,-1},
      { 8, 4, 5, 2, 8, 5, 2,11, 8, 2, 5, 1,-1},
      { 3, 2,11, 1, 4, 5, 1, 0, 4,-1,-1,-1,-1},
      { 9, 4, 5, 8, 2,11, 8, 0, 2,-1,-1,-1,-1},
      {11, 3, 2, 9, 4, 5,-1,-1,-1,-1,-1,-1,-1},
      { 3, 8, 4, 3, 4, 2, 2, 4, 5, 2, 5,10,-1},
      {10, 2, 5, 5, 2, 4, 2, 0, 4,-1,-1,-1,-1},
      { 0, 3, 8, 5, 9, 4,10, 2, 1,-1,-1,-1,-1},
      { 2, 1,10, 9, 4, 5,-1,-1,-1,-1,-1,-1,-1},
      { 4, 5, 8, 8, 5, 3, 5, 1, 3,-1,-1,-1,-1},
      { 5, 0, 4, 5, 1, 0,-1,-1,-1,-1,-1,-1,-1},
      { 3, 8, 0, 4, 5, 9,-1,-1,-1,-1,-1,-1,-1},
      { 9, 4, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 7, 4,11, 4, 9,11,11, 9,10,-1,-1,-1,-1},
      { 3, 0, 8, 7, 4,11,11, 4, 9,11, 9,10,-1},
      {11, 7, 4, 1,11, 4, 1,10,11, 1, 4, 0,-1},
      { 8, 7, 4,11, 1,10,11, 3, 1,-1,-1,-1,-1},
      { 2,11, 7, 2, 7, 1, 1, 7, 4, 1, 4, 9,-1},
      { 3, 2,11, 4, 8, 7, 9, 1, 0,-1,-1,-1,-1},
      { 7, 4,11,11, 4, 2, 4, 0, 2,-1,-1,-1,-1},
      { 2,11, 3, 7, 4, 8,-1,-1,-1,-1,-1,-1,-1},
      { 2, 3, 7, 2, 7, 9, 7, 4, 9, 2, 9,10,-1},
      { 4, 8, 7, 0,10, 2, 0, 9,10,-1,-1,-1,-1},
      { 2, 1,10, 0, 7, 4, 0, 3, 7,-1,-1,-1,-1},
      {10, 2, 1, 8, 7, 4,-1,-1,-1,-1,-1,-1,-1},
      { 9, 1, 4, 4, 1, 7, 1, 3, 7,-1,-1,-1,-1},
      { 1, 0, 9, 8, 7, 4,-1,-1,-1,-1,-1,-1,-1},
      { 3, 4, 0, 3, 7, 4,-1,-1,-1,-1,-1,-1,-1},
      { 8, 7, 4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 9,10, 8,10,11,-1,-1,-1,-1,-1,-1,-1},
      { 0, 9, 3, 3, 9,11, 9,10,11,-1,-1,-1,-1},
      { 1,10, 0, 0,10, 8,10,11, 8,-1,-1,-1,-1},
      {10, 3, 1,10,11, 3,-1,-1,-1,-1,-1,-1,-1},
      { 2,11, 1, 1,11, 9,11, 8, 9,-1,-1,-1,-1},
      {11, 3, 2, 0, 9, 1,-1,-1,-1,-1,-1,-1,-1},
      {11, 0, 2,11, 8, 0,-1,-1,-1,-1,-1,-1,-1},
      {11, 3, 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 3, 8, 2, 2, 8,10, 8, 9,10,-1,-1,-1,-1},
      { 9, 2, 0, 9,10, 2,-1,-1,-1,-1,-1,-1,-1},
      { 8, 0, 3, 1,10, 2,-1,-1,-1,-1,-1,-1,-1},
      {10, 2, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 1, 3, 8, 9, 1,-1,-1,-1,-1,-1,-1,-1},
      { 9, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      { 8, 0, 3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
      {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}*/
      // Another table
      {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 1, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 8, 3, 9, 8, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 3, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {9, 2, 10, 0, 2, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {2, 8, 3, 2, 10, 8, 10, 9, 8, -1, -1, -1, -1, -1, -1, -1},
      {3, 11, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 11, 2, 8, 11, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 9, 0, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 11, 2, 1, 9, 11, 9, 8, 11, -1, -1, -1, -1, -1, -1, -1},
      {3, 10, 1, 11, 10, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 10, 1, 0, 8, 10, 8, 11, 10, -1, -1, -1, -1, -1, -1, -1},
      {3, 9, 0, 3, 11, 9, 11, 10, 9, -1, -1, -1, -1, -1, -1, -1},
      {9, 8, 10, 10, 8, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 7, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 3, 0, 7, 3, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 1, 9, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 1, 9, 4, 7, 1, 7, 3, 1, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 10, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {3, 4, 7, 3, 0, 4, 1, 2, 10, -1, -1, -1, -1, -1, -1, -1},
      {9, 2, 10, 9, 0, 2, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1},
      {2, 10, 9, 2, 9, 7, 2, 7, 3, 7, 9, 4, -1, -1, -1, -1},
      {8, 4, 7, 3, 11, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {11, 4, 7, 11, 2, 4, 2, 0, 4, -1, -1, -1, -1, -1, -1, -1},
      {9, 0, 1, 8, 4, 7, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1},
      {4, 7, 11, 9, 4, 11, 9, 11, 2, 9, 2, 1, -1, -1, -1, -1},
      {3, 10, 1, 3, 11, 10, 7, 8, 4, -1, -1, -1, -1, -1, -1, -1},
      {1, 11, 10, 1, 4, 11, 1, 0, 4, 7, 11, 4, -1, -1, -1, -1},
      {4, 7, 8, 9, 0, 11, 9, 11, 10, 11, 0, 3, -1, -1, -1, -1},
      {4, 7, 11, 4, 11, 9, 9, 11, 10, -1, -1, -1, -1, -1, -1, -1},
      {9, 5, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {9, 5, 4, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 5, 4, 1, 5, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {8, 5, 4, 8, 3, 5, 3, 1, 5, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 10, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {3, 0, 8, 1, 2, 10, 4, 9, 5, -1, -1, -1, -1, -1, -1, -1},
      {5, 2, 10, 5, 4, 2, 4, 0, 2, -1, -1, -1, -1, -1, -1, -1},
      {2, 10, 5, 3, 2, 5, 3, 5, 4, 3, 4, 8, -1, -1, -1, -1},
      {9, 5, 4, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 11, 2, 0, 8, 11, 4, 9, 5, -1, -1, -1, -1, -1, -1, -1},
      {0, 5, 4, 0, 1, 5, 2, 3, 11, -1, -1, -1, -1, -1, -1, -1},
      {2, 1, 5, 2, 5, 8, 2, 8, 11, 4, 8, 5, -1, -1, -1, -1},
      {10, 3, 11, 10, 1, 3, 9, 5, 4, -1, -1, -1, -1, -1, -1, -1},
      {4, 9, 5, 0, 8, 1, 8, 10, 1, 8, 11, 10, -1, -1, -1, -1},
      {5, 4, 0, 5, 0, 11, 5, 11, 10, 11, 0, 3, -1, -1, -1, -1},
      {5, 4, 8, 5, 8, 10, 10, 8, 11, -1, -1, -1, -1, -1, -1, -1},
      {9, 7, 8, 5, 7, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {9, 3, 0, 9, 5, 3, 5, 7, 3, -1, -1, -1, -1, -1, -1, -1},
      {0, 7, 8, 0, 1, 7, 1, 5, 7, -1, -1, -1, -1, -1, -1, -1},
      {1, 5, 3, 3, 5, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {9, 7, 8, 9, 5, 7, 10, 1, 2, -1, -1, -1, -1, -1, -1, -1},
      {10, 1, 2, 9, 5, 0, 5, 3, 0, 5, 7, 3, -1, -1, -1, -1},
      {8, 0, 2, 8, 2, 5, 8, 5, 7, 10, 5, 2, -1, -1, -1, -1},
      {2, 10, 5, 2, 5, 3, 3, 5, 7, -1, -1, -1, -1, -1, -1, -1},
      {7, 9, 5, 7, 8, 9, 3, 11, 2, -1, -1, -1, -1, -1, -1, -1},
      {9, 5, 7, 9, 7, 2, 9, 2, 0, 2, 7, 11, -1, -1, -1, -1},
      {2, 3, 11, 0, 1, 8, 1, 7, 8, 1, 5, 7, -1, -1, -1, -1},
      {11, 2, 1, 11, 1, 7, 7, 1, 5, -1, -1, -1, -1, -1, -1, -1},
      {9, 5, 8, 8, 5, 7, 10, 1, 3, 10, 3, 11, -1, -1, -1, -1},
      {5, 7, 0, 5, 0, 9, 7, 11, 0, 1, 0, 10, 11, 10, 0, -1},
      {11, 10, 0, 11, 0, 3, 10, 5, 0, 8, 0, 7, 5, 7, 0, -1},
      {11, 10, 5, 7, 11, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {10, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 3, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {9, 0, 1, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 8, 3, 1, 9, 8, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1},
      {1, 6, 5, 2, 6, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 6, 5, 1, 2, 6, 3, 0, 8, -1, -1, -1, -1, -1, -1, -1},
      {9, 6, 5, 9, 0, 6, 0, 2, 6, -1, -1, -1, -1, -1, -1, -1},
      {5, 9, 8, 5, 8, 2, 5, 2, 6, 3, 2, 8, -1, -1, -1, -1},
      {2, 3, 11, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {11, 0, 8, 11, 2, 0, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1},
      {0, 1, 9, 2, 3, 11, 5, 10, 6, -1, -1, -1, -1, -1, -1, -1},
      {5, 10, 6, 1, 9, 2, 9, 11, 2, 9, 8, 11, -1, -1, -1, -1},
      {6, 3, 11, 6, 5, 3, 5, 1, 3, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 11, 0, 11, 5, 0, 5, 1, 5, 11, 6, -1, -1, -1, -1},
      {3, 11, 6, 0, 3, 6, 0, 6, 5, 0, 5, 9, -1, -1, -1, -1},
      {6, 5, 9, 6, 9, 11, 11, 9, 8, -1, -1, -1, -1, -1, -1, -1},
      {5, 10, 6, 4, 7, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 3, 0, 4, 7, 3, 6, 5, 10, -1, -1, -1, -1, -1, -1, -1},
      {1, 9, 0, 5, 10, 6, 8, 4, 7, -1, -1, -1, -1, -1, -1, -1},
      {10, 6, 5, 1, 9, 7, 1, 7, 3, 7, 9, 4, -1, -1, -1, -1},
      {6, 1, 2, 6, 5, 1, 4, 7, 8, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 5, 5, 2, 6, 3, 0, 4, 3, 4, 7, -1, -1, -1, -1},
      {8, 4, 7, 9, 0, 5, 0, 6, 5, 0, 2, 6, -1, -1, -1, -1},
      {7, 3, 9, 7, 9, 4, 3, 2, 9, 5, 9, 6, 2, 6, 9, -1},
      {3, 11, 2, 7, 8, 4, 10, 6, 5, -1, -1, -1, -1, -1, -1, -1},
      {5, 10, 6, 4, 7, 2, 4, 2, 0, 2, 7, 11, -1, -1, -1, -1},
      {0, 1, 9, 4, 7, 8, 2, 3, 11, 5, 10, 6, -1, -1, -1, -1},
      {9, 2, 1, 9, 11, 2, 9, 4, 11, 7, 11, 4, 5, 10, 6, -1},
      {8, 4, 7, 3, 11, 5, 3, 5, 1, 5, 11, 6, -1, -1, -1, -1},
      {5, 1, 11, 5, 11, 6, 1, 0, 11, 7, 11, 4, 0, 4, 11, -1},
      {0, 5, 9, 0, 6, 5, 0, 3, 6, 11, 6, 3, 8, 4, 7, -1},
      {6, 5, 9, 6, 9, 11, 4, 7, 9, 7, 11, 9, -1, -1, -1, -1},
      {10, 4, 9, 6, 4, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 10, 6, 4, 9, 10, 0, 8, 3, -1, -1, -1, -1, -1, -1, -1},
      {10, 0, 1, 10, 6, 0, 6, 4, 0, -1, -1, -1, -1, -1, -1, -1},
      {8, 3, 1, 8, 1, 6, 8, 6, 4, 6, 1, 10, -1, -1, -1, -1},
      {1, 4, 9, 1, 2, 4, 2, 6, 4, -1, -1, -1, -1, -1, -1, -1},
      {3, 0, 8, 1, 2, 9, 2, 4, 9, 2, 6, 4, -1, -1, -1, -1},
      {0, 2, 4, 4, 2, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {8, 3, 2, 8, 2, 4, 4, 2, 6, -1, -1, -1, -1, -1, -1, -1},
      {10, 4, 9, 10, 6, 4, 11, 2, 3, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 2, 2, 8, 11, 4, 9, 10, 4, 10, 6, -1, -1, -1, -1},
      {3, 11, 2, 0, 1, 6, 0, 6, 4, 6, 1, 10, -1, -1, -1, -1},
      {6, 4, 1, 6, 1, 10, 4, 8, 1, 2, 1, 11, 8, 11, 1, -1},
      {9, 6, 4, 9, 3, 6, 9, 1, 3, 11, 6, 3, -1, -1, -1, -1},
      {8, 11, 1, 8, 1, 0, 11, 6, 1, 9, 1, 4, 6, 4, 1, -1},
      {3, 11, 6, 3, 6, 0, 0, 6, 4, -1, -1, -1, -1, -1, -1, -1},
      {6, 4, 8, 11, 6, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {7, 10, 6, 7, 8, 10, 8, 9, 10, -1, -1, -1, -1, -1, -1, -1},
      {0, 7, 3, 0, 10, 7, 0, 9, 10, 6, 7, 10, -1, -1, -1, -1},
      {10, 6, 7, 1, 10, 7, 1, 7, 8, 1, 8, 0, -1, -1, -1, -1},
      {10, 6, 7, 10, 7, 1, 1, 7, 3, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 6, 1, 6, 8, 1, 8, 9, 8, 6, 7, -1, -1, -1, -1},
      {2, 6, 9, 2, 9, 1, 6, 7, 9, 0, 9, 3, 7, 3, 9, -1},
      {7, 8, 0, 7, 0, 6, 6, 0, 2, -1, -1, -1, -1, -1, -1, -1},
      {7, 3, 2, 6, 7, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {2, 3, 11, 10, 6, 8, 10, 8, 9, 8, 6, 7, -1, -1, -1, -1},
      {2, 0, 7, 2, 7, 11, 0, 9, 7, 6, 7, 10, 9, 10, 7, -1},
      {1, 8, 0, 1, 7, 8, 1, 10, 7, 6, 7, 10, 2, 3, 11, -1},
      {11, 2, 1, 11, 1, 7, 10, 6, 1, 6, 7, 1, -1, -1, -1, -1},
      {8, 9, 6, 8, 6, 7, 9, 1, 6, 11, 6, 3, 1, 3, 6, -1},
      {0, 9, 1, 11, 6, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {7, 8, 0, 7, 0, 6, 3, 11, 0, 11, 6, 0, -1, -1, -1, -1},
      {7, 11, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {7, 6, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {3, 0, 8, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 1, 9, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {8, 1, 9, 8, 3, 1, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1},
      {10, 1, 2, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 10, 3, 0, 8, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1},
      {2, 9, 0, 2, 10, 9, 6, 11, 7, -1, -1, -1, -1, -1, -1, -1},
      {6, 11, 7, 2, 10, 3, 10, 8, 3, 10, 9, 8, -1, -1, -1, -1},
      {7, 2, 3, 6, 2, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {7, 0, 8, 7, 6, 0, 6, 2, 0, -1, -1, -1, -1, -1, -1, -1},
      {2, 7, 6, 2, 3, 7, 0, 1, 9, -1, -1, -1, -1, -1, -1, -1},
      {1, 6, 2, 1, 8, 6, 1, 9, 8, 8, 7, 6, -1, -1, -1, -1},
      {10, 7, 6, 10, 1, 7, 1, 3, 7, -1, -1, -1, -1, -1, -1, -1},
      {10, 7, 6, 1, 7, 10, 1, 8, 7, 1, 0, 8, -1, -1, -1, -1},
      {0, 3, 7, 0, 7, 10, 0, 10, 9, 6, 10, 7, -1, -1, -1, -1},
      {7, 6, 10, 7, 10, 8, 8, 10, 9, -1, -1, -1, -1, -1, -1, -1},
      {6, 8, 4, 11, 8, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {3, 6, 11, 3, 0, 6, 0, 4, 6, -1, -1, -1, -1, -1, -1, -1},
      {8, 6, 11, 8, 4, 6, 9, 0, 1, -1, -1, -1, -1, -1, -1, -1},
      {9, 4, 6, 9, 6, 3, 9, 3, 1, 11, 3, 6, -1, -1, -1, -1},
      {6, 8, 4, 6, 11, 8, 2, 10, 1, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 10, 3, 0, 11, 0, 6, 11, 0, 4, 6, -1, -1, -1, -1},
      {4, 11, 8, 4, 6, 11, 0, 2, 9, 2, 10, 9, -1, -1, -1, -1},
      {10, 9, 3, 10, 3, 2, 9, 4, 3, 11, 3, 6, 4, 6, 3, -1},
      {8, 2, 3, 8, 4, 2, 4, 6, 2, -1, -1, -1, -1, -1, -1, -1},
      {0, 4, 2, 4, 6, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 9, 0, 2, 3, 4, 2, 4, 6, 4, 3, 8, -1, -1, -1, -1},
      {1, 9, 4, 1, 4, 2, 2, 4, 6, -1, -1, -1, -1, -1, -1, -1},
      {8, 1, 3, 8, 6, 1, 8, 4, 6, 6, 10, 1, -1, -1, -1, -1},
      {10, 1, 0, 10, 0, 6, 6, 0, 4, -1, -1, -1, -1, -1, -1, -1},
      {4, 6, 3, 4, 3, 8, 6, 10, 3, 0, 3, 9, 10, 9, 3, -1},
      {10, 9, 4, 6, 10, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 9, 5, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 3, 4, 9, 5, 11, 7, 6, -1, -1, -1, -1, -1, -1, -1},
      {5, 0, 1, 5, 4, 0, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1},
      {11, 7, 6, 8, 3, 4, 3, 5, 4, 3, 1, 5, -1, -1, -1, -1},
      {9, 5, 4, 10, 1, 2, 7, 6, 11, -1, -1, -1, -1, -1, -1, -1},
      {6, 11, 7, 1, 2, 10, 0, 8, 3, 4, 9, 5, -1, -1, -1, -1},
      {7, 6, 11, 5, 4, 10, 4, 2, 10, 4, 0, 2, -1, -1, -1, -1},
      {3, 4, 8, 3, 5, 4, 3, 2, 5, 10, 5, 2, 11, 7, 6, -1},
      {7, 2, 3, 7, 6, 2, 5, 4, 9, -1, -1, -1, -1, -1, -1, -1},
      {9, 5, 4, 0, 8, 6, 0, 6, 2, 6, 8, 7, -1, -1, -1, -1},
      {3, 6, 2, 3, 7, 6, 1, 5, 0, 5, 4, 0, -1, -1, -1, -1},
      {6, 2, 8, 6, 8, 7, 2, 1, 8, 4, 8, 5, 1, 5, 8, -1},
      {9, 5, 4, 10, 1, 6, 1, 7, 6, 1, 3, 7, -1, -1, -1, -1},
      {1, 6, 10, 1, 7, 6, 1, 0, 7, 8, 7, 0, 9, 5, 4, -1},
      {4, 0, 10, 4, 10, 5, 0, 3, 10, 6, 10, 7, 3, 7, 10, -1},
      {7, 6, 10, 7, 10, 8, 5, 4, 10, 4, 8, 10, -1, -1, -1, -1},
      {6, 9, 5, 6, 11, 9, 11, 8, 9, -1, -1, -1, -1, -1, -1, -1},
      {3, 6, 11, 0, 6, 3, 0, 5, 6, 0, 9, 5, -1, -1, -1, -1},
      {0, 11, 8, 0, 5, 11, 0, 1, 5, 5, 6, 11, -1, -1, -1, -1},
      {6, 11, 3, 6, 3, 5, 5, 3, 1, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 10, 9, 5, 11, 9, 11, 8, 11, 5, 6, -1, -1, -1, -1},
      {0, 11, 3, 0, 6, 11, 0, 9, 6, 5, 6, 9, 1, 2, 10, -1},
      {11, 8, 5, 11, 5, 6, 8, 0, 5, 10, 5, 2, 0, 2, 5, -1},
      {6, 11, 3, 6, 3, 5, 2, 10, 3, 10, 5, 3, -1, -1, -1, -1},
      {5, 8, 9, 5, 2, 8, 5, 6, 2, 3, 8, 2, -1, -1, -1, -1},
      {9, 5, 6, 9, 6, 0, 0, 6, 2, -1, -1, -1, -1, -1, -1, -1},
      {1, 5, 8, 1, 8, 0, 5, 6, 8, 3, 8, 2, 6, 2, 8, -1},
      {1, 5, 6, 2, 1, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 3, 6, 1, 6, 10, 3, 8, 6, 5, 6, 9, 8, 9, 6, -1},
      {10, 1, 0, 10, 0, 6, 9, 5, 0, 5, 6, 0, -1, -1, -1, -1},
      {0, 3, 8, 5, 6, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {10, 5, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {11, 5, 10, 7, 5, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {11, 5, 10, 11, 7, 5, 8, 3, 0, -1, -1, -1, -1, -1, -1, -1},
      {5, 11, 7, 5, 10, 11, 1, 9, 0, -1, -1, -1, -1, -1, -1, -1},
      {10, 7, 5, 10, 11, 7, 9, 8, 1, 8, 3, 1, -1, -1, -1, -1},
      {11, 1, 2, 11, 7, 1, 7, 5, 1, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 3, 1, 2, 7, 1, 7, 5, 7, 2, 11, -1, -1, -1, -1},
      {9, 7, 5, 9, 2, 7, 9, 0, 2, 2, 11, 7, -1, -1, -1, -1},
      {7, 5, 2, 7, 2, 11, 5, 9, 2, 3, 2, 8, 9, 8, 2, -1},
      {2, 5, 10, 2, 3, 5, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1},
      {8, 2, 0, 8, 5, 2, 8, 7, 5, 10, 2, 5, -1, -1, -1, -1},
      {9, 0, 1, 5, 10, 3, 5, 3, 7, 3, 10, 2, -1, -1, -1, -1},
      {9, 8, 2, 9, 2, 1, 8, 7, 2, 10, 2, 5, 7, 5, 2, -1},
      {1, 3, 5, 3, 7, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 7, 0, 7, 1, 1, 7, 5, -1, -1, -1, -1, -1, -1, -1},
      {9, 0, 3, 9, 3, 5, 5, 3, 7, -1, -1, -1, -1, -1, -1, -1},
      {9, 8, 7, 5, 9, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {5, 8, 4, 5, 10, 8, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1},
      {5, 0, 4, 5, 11, 0, 5, 10, 11, 11, 3, 0, -1, -1, -1, -1},
      {0, 1, 9, 8, 4, 10, 8, 10, 11, 10, 4, 5, -1, -1, -1, -1},
      {10, 11, 4, 10, 4, 5, 11, 3, 4, 9, 4, 1, 3, 1, 4, -1},
      {2, 5, 1, 2, 8, 5, 2, 11, 8, 4, 5, 8, -1, -1, -1, -1},
      {0, 4, 11, 0, 11, 3, 4, 5, 11, 2, 11, 1, 5, 1, 11, -1},
      {0, 2, 5, 0, 5, 9, 2, 11, 5, 4, 5, 8, 11, 8, 5, -1},
      {9, 4, 5, 2, 11, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {2, 5, 10, 3, 5, 2, 3, 4, 5, 3, 8, 4, -1, -1, -1, -1},
      {5, 10, 2, 5, 2, 4, 4, 2, 0, -1, -1, -1, -1, -1, -1, -1},
      {3, 10, 2, 3, 5, 10, 3, 8, 5, 4, 5, 8, 0, 1, 9, -1},
      {5, 10, 2, 5, 2, 4, 1, 9, 2, 9, 4, 2, -1, -1, -1, -1},
      {8, 4, 5, 8, 5, 3, 3, 5, 1, -1, -1, -1, -1, -1, -1, -1},
      {0, 4, 5, 1, 0, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {8, 4, 5, 8, 5, 3, 9, 0, 5, 0, 3, 5, -1, -1, -1, -1},
      {9, 4, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 11, 7, 4, 9, 11, 9, 10, 11, -1, -1, -1, -1, -1, -1, -1},
      {0, 8, 3, 4, 9, 7, 9, 11, 7, 9, 10, 11, -1, -1, -1, -1},
      {1, 10, 11, 1, 11, 4, 1, 4, 0, 7, 4, 11, -1, -1, -1, -1},
      {3, 1, 4, 3, 4, 8, 1, 10, 4, 7, 4, 11, 10, 11, 4, -1},
      {4, 11, 7, 9, 11, 4, 9, 2, 11, 9, 1, 2, -1, -1, -1, -1},
      {9, 7, 4, 9, 11, 7, 9, 1, 11, 2, 11, 1, 0, 8, 3, -1},
      {11, 7, 4, 11, 4, 2, 2, 4, 0, -1, -1, -1, -1, -1, -1, -1},
      {11, 7, 4, 11, 4, 2, 8, 3, 4, 3, 2, 4, -1, -1, -1, -1},
      {2, 9, 10, 2, 7, 9, 2, 3, 7, 7, 4, 9, -1, -1, -1, -1},
      {9, 10, 7, 9, 7, 4, 10, 2, 7, 8, 7, 0, 2, 0, 7, -1},
      {3, 7, 10, 3, 10, 2, 7, 4, 10, 1, 10, 0, 4, 0, 10, -1},
      {1, 10, 2, 8, 7, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 9, 1, 4, 1, 7, 7, 1, 3, -1, -1, -1, -1, -1, -1, -1},
      {4, 9, 1, 4, 1, 7, 0, 8, 1, 8, 7, 1, -1, -1, -1, -1},
      {4, 0, 3, 7, 4, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {4, 8, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {9, 10, 8, 10, 11, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {3, 0, 9, 3, 9, 11, 11, 9, 10, -1, -1, -1, -1, -1, -1, -1},
      {0, 1, 10, 0, 10, 8, 8, 10, 11, -1, -1, -1, -1, -1, -1, -1},
      {3, 1, 10, 11, 3, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 2, 11, 1, 11, 9, 9, 11, 8, -1, -1, -1, -1, -1, -1, -1},
      {3, 0, 9, 3, 9, 11, 1, 2, 9, 2, 11, 9, -1, -1, -1, -1},
      {0, 2, 11, 8, 0, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {3, 2, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {2, 3, 8, 2, 8, 10, 10, 8, 9, -1, -1, -1, -1, -1, -1, -1},
      {9, 10, 2, 0, 9, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {2, 3, 8, 2, 8, 10, 0, 1, 8, 1, 10, 8, -1, -1, -1, -1},
      {1, 10, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {1, 3, 8, 9, 1, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 9, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {0, 3, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
      {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
    };


    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void Awake()
    {      
      this._meshTriangles = new List<int>();
      this._meshUV = new List<Vector2>();
      this._meshVertices = new List<Vector3>();
      this._meshVerticesColor = new List<Color32>();

      this._mesh = new Mesh();
      this._mesh.MarkDynamic();
      this._mesh.name = "Translated Marshing Voxel Mesh";

      this.GetComponent<MeshFilter>().sharedMesh = this._mesh;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected override void OnDestroy()
    {
      this.GetComponent<MeshFilter>().sharedMesh = null;
      DestroyImmediate(this._mesh);
      base.OnDestroy();
    }

    /// <summary>
    ///   Clear generated mesh data.
    /// </summary>
    protected void Clear()
    {
      this._meshTriangles.Clear();
      this._meshUV.Clear();
      this._meshVertices.Clear();
      this._meshVerticesColor.Clear();
    }

    /// <summary>
    ///   Publish the generated mesh.
    /// </summary>
    protected void Publish()
    {
      this._mesh.Clear();

      this._mesh.vertices = this._meshVertices.ToArray();
      this._mesh.uv = this._meshUV.ToArray();
      this._mesh.colors32 = this._meshVerticesColor.ToArray();
      this._mesh.triangles = this._meshTriangles.ToArray();

      this._mesh.RecalculateNormals();
      this._mesh.RecalculateBounds();

      this._mesh.UploadMeshData(false);
      
      this.Clear();
    }

    /// <summary>
    ///   Translate a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void Translate(int x, int y, int z)
    {
      int index = 0;

      index |= (!this.GlobalMesh.IsEmpty(x + 0, y + 0, z + 0)) ? 1 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 1, y + 0, z + 0)) ? 2 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 1, y + 0, z + 1)) ? 4 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 0, y + 0, z + 1)) ? 8 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 0, y + 1, z + 0)) ? 16 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 1, y + 1, z + 0)) ? 32 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 1, y + 1, z + 1)) ? 64 : 0;
      index |= (!this.GlobalMesh.IsEmpty(x + 0, y + 1, z + 1)) ? 128 : 0;

      Color32[,,] colors = new Color32[2,2,2];

      for(int i = 0; i < 2; ++i)
      {
        for(int j = 0; j < 2; ++j)
        {
          for(int k = 0; k < 2; ++k)
          {
            colors[i, j, k] = this.GlobalMesh[x + i, y + j, z + k];
          }
        }
      }

      this.Translate(index, new Vector3(x, y, z), colors);
    }

    /// <summary>
    ///   Translate a marshing cube case at a specific location.
    /// </summary>
    /// <param name="caseIndex"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    private void Translate(int caseIndex, Vector3 position, Color32[,,] colors)
    {
      int triangleIndex = 0;

      while(triangleIndex < 13)
      {
        if(MarshingVoxelMeshBuilder.marshingCases[caseIndex, triangleIndex] >= 0)
        {
          this.TranslateFace(caseIndex, triangleIndex, position, colors);
        }
        triangleIndex += 3;
      }
    }

    /// <summary>
    ///   Translate a marshing cube triangle at a specific location.
    /// </summary>
    /// <param name="caseIndex"></param>
    /// <param name="triangleIndex"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    private void TranslateFace(int caseIndex, int triangleIndex, Vector3 position, Color32[,,] colors)
    {
      Vector3[] vertices = new Vector3[3];
      Color32[] trColors = new Color32[3];

      for(int i = 0; i < 3; ++i)
      {
        vertices[i] = this.Location(
          MarshingVoxelMeshBuilder.marshingCases[caseIndex, triangleIndex + i], 
          position
        );

        trColors[i] = this.Color(
          MarshingVoxelMeshBuilder.marshingCases[caseIndex, triangleIndex + i],
          caseIndex,
          colors
        );
      }

      this.MakeTriangle(vertices, trColors);
    }

    /// <summary>
    ///   Get a vertex color.
    /// </summary>
    /// <param name="edgeIndex"></param>
    /// <param name="caseIndex"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    private Color32 Color(int edgeIndex, int caseIndex, Color32[,,] colors)
    {
      switch (edgeIndex)
      {
        case 0:
          // Check for existing voxels between the two possibilities and choose a color.
          if ((1 & caseIndex) == 0) return colors[1, 0, 0];
          else return colors[0, 0, 0];
        case 1:
          if ((2 & caseIndex) == 0) return colors[1, 0, 1];
          else return colors[1, 0, 0];
        case 2:
          if ((4 & caseIndex) == 0) return colors[0, 0, 1];
          else return colors[1, 0, 1];
        case 3:
          if ((8 & caseIndex) == 0) return colors[0, 0, 0];
          else return colors[0, 0, 1];
        case 4:
          if ((16 & caseIndex) == 0) return colors[1, 1, 0];
          else return colors[0, 1, 0];
        case 5:
          if ((32 & caseIndex) == 0) return colors[1, 1, 1];
          else return colors[1, 1, 0];
        case 6:
          if ((64 & caseIndex) == 0) return colors[0, 1, 1];
          else return colors[1, 1, 1];
        case 7:
          if ((128 & caseIndex) == 0) return colors[0, 1, 0];
          else return colors[0, 1, 1];
        case 8:
          if ((1 & caseIndex) == 0) return colors[0, 1, 0];
          else return colors[0, 0, 0];
        case 9:
          if ((2 & caseIndex) == 0) return colors[1, 1, 0];
          else return colors[1, 0, 0];
        case 10:
          if ((4 & caseIndex) == 0) return colors[1, 1, 1];
          else return colors[1, 0, 1];
        case 11:
          if ((8 & caseIndex) == 0) return colors[0, 1, 1];
          else return colors[0, 0, 1];
        default: throw new InvalidOperationException("Invalid edge index : " + edgeIndex);
      }
    }

    /// <summary>
    ///   Get an edge location.
    /// </summary>
    /// <param name="edgeIndex"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector3 Location(int edgeIndex, Vector3 position)
    {
      switch(edgeIndex)
      {
        case  0: return position + new Vector3(0.5f, 0, 0);
        case  1: return position + new Vector3(1f, 0, 0.5f);
        case  2: return position + new Vector3(0.5f, 0, 1f);
        case  3: return position + new Vector3(0, 0, 0.5f);
        case  4: return position + new Vector3(0.5f, 1f, 0);
        case  5: return position + new Vector3(1f, 1f, 0.5f);
        case  6: return position + new Vector3(0.5f, 1f, 1f);
        case  7: return position + new Vector3(0, 1f, 0.5f);
        case  8: return position + new Vector3(0, 0.5f, 0);
        case  9: return position + new Vector3(1f, 0.5f, 0);
        case 10: return position + new Vector3(1f, 0.5f, 1f);
        case 11: return position + new Vector3(0, 0.5f, 1f);
        default: throw new InvalidOperationException("Invalid edge index : " + edgeIndex);
      }
    }

    /// <summary>
    ///   Create a face.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="color"></param>
    private void MakeTriangle(Vector3[] vertices, Color32[] colors)
    {
      int indexBase = this._meshVertices.Count;

      this._meshVertices.AddRange(vertices);

      this._meshVerticesColor.AddRange(colors);

      this._meshUV.AddRange(new Vector2[] {
        Vector2.zero, Vector2.zero, Vector2.zero
      });

      this._meshTriangles.AddRange(new int[] {
        indexBase, indexBase + 1, indexBase + 2
      });
    }
  }
}
