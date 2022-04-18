using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Broccoli.Base;
using Broccoli.Pipe;
using Broccoli.Catalog;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Branch mesh generator node editor.
	/// </summary>
	[CustomEditor(typeof(BranchMeshGeneratorNode))]
	public class BranchMeshGeneratorNodeEditor : BaseNodeEditor {
		#region Vars
		/// <summary>
		/// The branch mesh generator node.
		/// </summary>
		public BranchMeshGeneratorNode branchMeshGeneratorNode;
		/// <summary>
		/// Options to show on the toolbar.
		/// </summary>
		string[] toolBarOptions = new string[] {"Global", "Branch Shape"};
		/// <summary>
		/// Shape catalog.
		/// </summary>
		ShapeCatalog shapeCatalog;
		/// <summary>
		/// Selected shape index.
		/// </summary>
		int selectedShapeIndex = 0;
		/// <summary>
		/// The welding curve range.
		/// </summary>
		private static Rect weldingCurveRange = new Rect (0f, 0f, 1f, 1f);


		SerializedProperty propMinPolygonSides;
		SerializedProperty propMaxPolygonSides;
		SerializedProperty propUseHardNormals;
		SerializedProperty propMinBranchCurveResolution;
		SerializedProperty propMaxBranchCurveResolution;
		SerializedProperty propMeshMode;
		SerializedProperty propMeshContext;
		SerializedProperty propMeshRange;
		SerializedProperty propMinNodes;
		SerializedProperty propMaxNodes;
		SerializedProperty propMinNodeLength;
		SerializedProperty propMaxNodeLength;
		SerializedProperty propLengthVariance;
		SerializedProperty propNodesDistribution;
		SerializedProperty propShapeScale;
		SerializedProperty propBranchHierarchyScaleAdherence;
		SerializedProperty propUseBranchWelding;
		SerializedProperty propMinBranchWeldingHierarchyRange;
		SerializedProperty propMaxBranchWeldingHierarchyRange;
		SerializedProperty propBranchWeldingHierarchyRangeCurve;
		SerializedProperty propBranchWeldingCurve;
		SerializedProperty propMinBranchWeldingDistance;
		SerializedProperty propMaxBranchWeldingDistance;
		SerializedProperty propMinAdditionalBranchWeldingSegments;
		SerializedProperty propMaxAdditionalBranchWeldingSegments;
		SerializedProperty propMinBranchWeldingUpperSpread;
		SerializedProperty propMaxBranchWeldingUpperSpread;
		SerializedProperty propMinBranchWeldingLowerSpread;
		SerializedProperty propMaxBranchWeldingLowerSpread;
		SerializedProperty propUseRootWelding;
		SerializedProperty propMinRootWeldingHierarchyRange;
		SerializedProperty propMaxRootWeldingHierarchyRange;
		SerializedProperty propRootWeldingHierarchyRangeCurve;
		SerializedProperty propRootWeldingCurve;
		SerializedProperty propMinRootWeldingDistance;
		SerializedProperty propMaxRootWeldingDistance;
		SerializedProperty propMinAdditionalRootWeldingSegments;
		SerializedProperty propMaxAdditionalRootWeldingSegments;
		SerializedProperty propMinRootWeldingUpperSpread;
		SerializedProperty propMaxRootWeldingUpperSpread;
		SerializedProperty propMinRootWeldingLowerSpread;
		SerializedProperty propMaxRootWeldingLowerSpread;
		#endregion

		#region Messages
		private static string MSG_ALPHA = "Shape meshing is a feature currently in alpha release. Although functional, improvements and testing is being performed to identify bugs on this feature.";
		private static string MSG_MIN_POLYGON_SIDES = "Minimum number of sides on the polygon used to create the mesh.";
		private static string MSG_MAX_POLYGON_SIDES = "Maximum number of sides on the polygon used to create the mesh.";
		private static string MSG_MIN_BRANCH_CURVE_RESOLUTION = "Minimum resolution used to process branch curves to create segments. " +
			"The minimum value is used when processing the mesh at the lowest resolution LOD.";
		private static string MSG_MAX_BRANCH_CURVE_RESOLUTION = "Maximum resolution used to process branch curves to create segments. " +
			"The maximum value is used when processing the mesh at the highest resolution LOD.";
		private static string MSG_USE_HARD_NORMALS = "Hard normals increases the number vertices per face while " +
			"keeping the same number of triangles. This option is useful to give a lowpoly flat shaded effect on the mesh.";
		private string MSG_SHAPE = "Selects a shape to use to stylize the branches mesh.";
		private string MSG_MESH_MODE = "Option to select how each branch mesh should be stylized.";
		private string MSG_MESH_CONTEXT = "Selects if a custom shape context encompass a single branch or a follow up series of branches.";
		private string MSG_MESH_RANGE = "Selects if a custom shape should encompass its whole mesh context or be divided by nodes.";
		private string MSG_NODES_MINMAX = "Range of the number of nodes to generate.";
		private string MSG_NODES_LENGTH_VARIANCE = "Variance in length size of nodes. Variance with value 0 gives nodes with the same length within a mesh context.";
		private string MSG_NODES_DISTRIBUTION = "How to distribute nodes along the mesh context.";
		private string MSG_SHAPE_SCALE = "Scale multiplier for the shape.";
		private string MSG_BRANCH_HIERARCHY_SCALE = "How much of the shape scale is taken based on the branch hierarchy. Value of 1 is full adherence to the branch scale at a given hierarchy position.";
		private string MSG_USE_BRANCH_WELDING = "Enables mesh welding between a branch and its parent branch.";
		private string MSG_BRANCH_WELDING_HIERARCHY_RANGE = "Hierarchy limit to apply welding to branches across the tree hierarchy. The base of the trunk is 0, the last tip of a terminal branch is 1.";
		private string MSG_BRANCH_WELDING_HIERARCHY_RANGE_CURVE = "Curve to control the amount of welding applied across the hierarchy limit selected for branches.";
		private string MSG_BRANCH_WELDING_CURVE = "Curve to control the shape of the welding range used on a branch.";
		private string MSG_BRANCH_WELDING_DISTANCE = "How long from the base of a branch welding should expand. This value multiplies the girth at the parent branch to get the distance.";
		private string MSG_ADDITIONAL_BRANCH_WELDING_SEGMENTS = "Adds additional points to the welding range.";
		private string MSG_BRANCH_WELDING_UPPER_SPREAD = "How much length welding should take along the parent branch on the growth (upper) direction.";
		private string MSG_BRANCH_WELDING_LOWER_SPREAD = "How much length welding should take along the parent branch against the growth (lower) direction.";
		private string MSG_USE_ROOT_WELDING = "Enables mesh welding between a root and its parent branch or root.";
		private string MSG_ROOT_WELDING_HIERARCHY_RANGE = "Hierarchy limit to apply welding to roots across the tree hierarchy. The base of the trunk is 0, the last tip of a terminal root is 1.";
		private string MSG_ROOT_WELDING_HIERARCHY_RANGE_CURVE = "Curve to control the amount of welding applied across the hierarchy limit selected for roots.";
		private string MSG_ROOT_WELDING_CURVE = "Curve to control the shape of the welding range used on a root.";
		private string MSG_ROOT_WELDING_DISTANCE = "How long from the base of a root welding should expand. This value multiplies the girth at the parent branch or root to get the distance.";
		private string MSG_ADDITIONAL_ROOT_WELDING_SEGMENTS = "Adds additional points to the welding range.";
		private string MSG_ROOT_WELDING_UPPER_SPREAD = "How much length welding should take along the parent branch on the growth (upper) direction.";
		private string MSG_ROOT_WELDING_LOWER_SPREAD = "How much length welding should take along the parent branch against the growth (lower) direction.";
		#endregion

		#region Events
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected override void OnEnableSpecific () {
			branchMeshGeneratorNode = target as BranchMeshGeneratorNode;

			SetPipelineElementProperty ("branchMeshGeneratorElement");
			propMinPolygonSides = GetSerializedProperty ("minPolygonSides");
			propMaxPolygonSides = GetSerializedProperty ("maxPolygonSides");
			propUseHardNormals = GetSerializedProperty ("useHardNormals");
			propMinBranchCurveResolution = GetSerializedProperty ("minBranchCurveResolution");
			propMaxBranchCurveResolution = GetSerializedProperty ("maxBranchCurveResolution");
			propMeshMode = GetSerializedProperty ("meshMode");
			propMeshContext = GetSerializedProperty ("meshContext");
			propMeshRange = GetSerializedProperty ("meshRange");
			propMinNodes = GetSerializedProperty ("minNodes");
			propMaxNodes = GetSerializedProperty ("maxNodes");
			propMinNodeLength = GetSerializedProperty ("minNodeLength");
			propMaxNodeLength = GetSerializedProperty ("maxNodeLength");
			propLengthVariance = GetSerializedProperty ("nodeLengthVariance");
			propNodesDistribution = GetSerializedProperty ("nodesDistribution");
			propShapeScale = GetSerializedProperty ("shapeScale");;
			propBranchHierarchyScaleAdherence = GetSerializedProperty ("branchHierarchyScaleAdherence");
			propUseBranchWelding = GetSerializedProperty ("useBranchWelding");
			propMinBranchWeldingHierarchyRange = GetSerializedProperty ("minBranchWeldingHierarchyRange");
			propMaxBranchWeldingHierarchyRange = GetSerializedProperty ("maxBranchWeldingHierarchyRange");
			propBranchWeldingHierarchyRangeCurve = GetSerializedProperty ("branchWeldingHierarchyRangeCurve");
			propBranchWeldingCurve = GetSerializedProperty ("branchWeldingCurve");
			propMinBranchWeldingDistance = GetSerializedProperty ("minBranchWeldingDistance");
			propMaxBranchWeldingDistance = GetSerializedProperty ("maxBranchWeldingDistance");
			propMinAdditionalBranchWeldingSegments = GetSerializedProperty ("minAdditionalBranchWeldingSegments");
			propMaxAdditionalBranchWeldingSegments = GetSerializedProperty ("maxAdditionalBranchWeldingSegments");
			propMinBranchWeldingUpperSpread = GetSerializedProperty ("minBranchWeldingUpperSpread");
			propMaxBranchWeldingUpperSpread = GetSerializedProperty ("maxBranchWeldingUpperSpread");
			propMinBranchWeldingLowerSpread = GetSerializedProperty ("minBranchWeldingLowerSpread");
			propMaxBranchWeldingLowerSpread = GetSerializedProperty ("maxBranchWeldingLowerSpread");
			propUseRootWelding = GetSerializedProperty ("useRootWelding");
			propMinRootWeldingHierarchyRange = GetSerializedProperty ("minRootWeldingHierarchyRange");
			propMaxRootWeldingHierarchyRange = GetSerializedProperty ("maxRootWeldingHierarchyRange");
			propRootWeldingHierarchyRangeCurve = GetSerializedProperty ("rootWeldingHierarchyRangeCurve");
			propRootWeldingCurve = GetSerializedProperty ("rootWeldingCurve");
			propMinRootWeldingDistance = GetSerializedProperty ("minRootWeldingDistance");
			propMaxRootWeldingDistance = GetSerializedProperty ("maxRootWeldingDistance");
			propMinAdditionalRootWeldingSegments = GetSerializedProperty ("minAdditionalRootWeldingSegments");
			propMaxAdditionalRootWeldingSegments = GetSerializedProperty ("maxAdditionalRootWeldingSegments");
			propMinRootWeldingUpperSpread = GetSerializedProperty ("minRootWeldingUpperSpread");
			propMaxRootWeldingUpperSpread = GetSerializedProperty ("maxRootWeldingUpperSpread");
			propMinRootWeldingLowerSpread = GetSerializedProperty ("minRootWeldingLowerSpread");
			propMaxRootWeldingLowerSpread = GetSerializedProperty ("maxRootWeldingLowerSpread");

			shapeCatalog = ShapeCatalog.GetInstance ();
			selectedShapeIndex = shapeCatalog.GetShapeIndex (branchMeshGeneratorNode.branchMeshGeneratorElement.selectedShapeId);
		}
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI() {
			CheckUndoRequest ();

			UpdateSerialized ();

			branchMeshGeneratorNode.selectedToolbar = GUILayout.Toolbar (branchMeshGeneratorNode.selectedToolbar, toolBarOptions);
			EditorGUILayout.Space ();

			if (branchMeshGeneratorNode.selectedToolbar == 0) {
				EditorGUI.BeginChangeCheck ();

				EditorGUILayout.LabelField ("Mesh Resolution", EditorStyles.boldLabel);

				int maxPolygonSides = propMaxPolygonSides.intValue;
				EditorGUILayout.IntSlider (propMaxPolygonSides, 3, 16, "Max Polygon Sides");
				ShowHelpBox (MSG_MAX_POLYGON_SIDES);

				int minPolygonSides = propMinPolygonSides.intValue;
				EditorGUILayout.IntSlider (propMinPolygonSides, 3, 16, "Min Polygon Sides");
				ShowHelpBox (MSG_MIN_POLYGON_SIDES);

				float maxBranchCurveResolution = propMaxBranchCurveResolution.floatValue;
				EditorGUILayout.Slider (propMaxBranchCurveResolution, 0, 1, "Max Branch Resolution");
				ShowHelpBox (MSG_MAX_BRANCH_CURVE_RESOLUTION);

				float minBranchCurveResolution = propMinBranchCurveResolution.floatValue;
				EditorGUILayout.Slider (propMinBranchCurveResolution, 0, 1, "Min Branch Resolution");
				ShowHelpBox (MSG_MIN_BRANCH_CURVE_RESOLUTION);
				EditorGUILayout.Space ();
				
				/*
				bool useHardNormals = propUseHardNormals.boolValue;
				EditorGUILayout.PropertyField (propUseHardNormals);
				ShowHelpBox (MSG_USE_HARD_NORMALS);
				EditorGUILayout.Space ();
				*/

				if (GlobalSettings.experimentalBranchWelding) {
					EditorGUILayout.LabelField ("Branch Welding", EditorStyles.boldLabel);
					bool useBranchWelding = propUseBranchWelding.boolValue;
					EditorGUILayout.PropertyField (propUseBranchWelding);
					ShowHelpBox (MSG_USE_BRANCH_WELDING);
					if (useBranchWelding) {
						FloatRangePropertyField (propMinBranchWeldingHierarchyRange, propMaxBranchWeldingHierarchyRange, 0f, 1f, "Hierarchy Range");
						ShowHelpBox (MSG_BRANCH_WELDING_HIERARCHY_RANGE);

						EditorGUILayout.CurveField (propBranchWeldingHierarchyRangeCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_BRANCH_WELDING_HIERARCHY_RANGE_CURVE);
						EditorGUILayout.Space ();

						EditorGUILayout.CurveField (propBranchWeldingCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_BRANCH_WELDING_CURVE);

						FloatRangePropertyField (propMinBranchWeldingDistance, propMaxBranchWeldingDistance, 1.5f, 5f, "Welding Distance");
						ShowHelpBox (MSG_BRANCH_WELDING_DISTANCE);

						IntRangePropertyField (propMinAdditionalBranchWeldingSegments, propMaxAdditionalBranchWeldingSegments, 0, 7, "Additional Segments");
						ShowHelpBox (MSG_ADDITIONAL_BRANCH_WELDING_SEGMENTS);

						FloatRangePropertyField (propMinBranchWeldingUpperSpread, propMaxBranchWeldingUpperSpread, 1f, 4f, "Welding Upper Spread");
						ShowHelpBox (MSG_BRANCH_WELDING_UPPER_SPREAD);

						FloatRangePropertyField (propMinBranchWeldingLowerSpread, propMaxBranchWeldingLowerSpread, 1f, 4f, "Welding Lower Spread");
						ShowHelpBox (MSG_BRANCH_WELDING_LOWER_SPREAD);
					}
					EditorGUILayout.Space ();

					EditorGUILayout.LabelField ("Root Welding", EditorStyles.boldLabel);
					bool useRootWelding = propUseRootWelding.boolValue;
					EditorGUILayout.PropertyField (propUseRootWelding);
					ShowHelpBox (MSG_USE_ROOT_WELDING);
					if (useRootWelding) {
						FloatRangePropertyField (propMinRootWeldingHierarchyRange, propMaxRootWeldingHierarchyRange, 0f, 1f, "Hierarchy Range");
						ShowHelpBox (MSG_ROOT_WELDING_HIERARCHY_RANGE);

						EditorGUILayout.CurveField (propRootWeldingHierarchyRangeCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_ROOT_WELDING_HIERARCHY_RANGE_CURVE);
						EditorGUILayout.Space ();

						EditorGUILayout.CurveField (propRootWeldingCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_ROOT_WELDING_CURVE);

						FloatRangePropertyField (propMinRootWeldingDistance, propMaxRootWeldingDistance, 1.5f, 5f, "Welding Distance");
						ShowHelpBox (MSG_ROOT_WELDING_DISTANCE);

						IntRangePropertyField (propMinAdditionalRootWeldingSegments, propMaxAdditionalRootWeldingSegments, 0, 7, "Additional Segments");
						ShowHelpBox (MSG_ADDITIONAL_ROOT_WELDING_SEGMENTS);

						FloatRangePropertyField (propMinRootWeldingUpperSpread, propMaxRootWeldingUpperSpread, 1f, 4f, "Welding Upper Spread");
						ShowHelpBox (MSG_ROOT_WELDING_UPPER_SPREAD);

						FloatRangePropertyField (propMinRootWeldingLowerSpread, propMaxRootWeldingLowerSpread, 1f, 4f, "Welding Lower Spread");
						ShowHelpBox (MSG_ROOT_WELDING_LOWER_SPREAD);
					}
					EditorGUILayout.Space ();
				}

				if (EditorGUI.EndChangeCheck () &&
					propMinPolygonSides.intValue <= propMaxPolygonSides.intValue &&
					propMinBranchCurveResolution.floatValue <= propMaxBranchCurveResolution.floatValue) {
					ApplySerialized ();
					UpdatePipeline (GlobalSettings.processingDelayHigh);
					NodeEditorFramework.NodeEditor.RepaintClients ();
					branchMeshGeneratorNode.branchMeshGeneratorElement.Validate ();
					SetUndoControlCounter ();
				}
			} else {
				EditorGUI.BeginChangeCheck ();

				// MESHING MODES
				EditorGUILayout.PropertyField (propMeshMode);
				ShowHelpBox (MSG_MESH_MODE);
				EditorGUILayout.Space ();

				// IF SHAPE MODE SELECTED
				if (propMeshMode.enumValueIndex == (int)BranchMeshGeneratorElement.MeshMode.Shape) {
					// ALPHA MESSAGE.
					EditorGUILayout.HelpBox (MSG_ALPHA, MessageType.Warning);
					EditorGUILayout.Space ();

					// SELECT SHAPE.
					selectedShapeIndex = EditorGUILayout.Popup ("Shape", selectedShapeIndex, shapeCatalog.GetShapeOptions ());
					ShowHelpBox (MSG_SHAPE);
					EditorGUILayout.Space ();

					EditorGUILayout.PropertyField (propMeshContext);
					ShowHelpBox (MSG_MESH_CONTEXT);
					EditorGUILayout.Space ();

					EditorGUILayout.PropertyField (propMeshRange);
					ShowHelpBox (MSG_MESH_RANGE);
					EditorGUILayout.Space ();

					// IF NODE MESH RANGE SELECTED
					if (propMeshRange.enumValueIndex == (int)BranchMeshGeneratorElement.MeshRange.Nodes) {
						// Default to number node mode.
						/*
						EditorGUILayout.PropertyField (propNodesMode);
						ShowHelpBox (MSG_NODES_MODE);
						EditorGUILayout.Space ();

						if (propNodesMode.enumValueIndex == (int)BranchMeshGeneratorElement.NodesMode.Length) {
							// IF NODE MODE LENGTH
							FloatRangePropertyField (propMinNodeLength, propMaxNodeLength, 0f, 1f, "Node Length");
							ShowHelpBox (MSG_NODES_MINMAX_LENGTH);
						} else {
						*/
							// IF NODE MODE NUMBER
							IntRangePropertyField (propMinNodes, propMaxNodes, 2, 8, "Nodes");
							ShowHelpBox (MSG_NODES_MINMAX);
						//}
						EditorGUILayout.Space ();

						EditorGUILayout.Slider (propLengthVariance, 0f, 1f, "Node Size Variance");
						ShowHelpBox (MSG_NODES_LENGTH_VARIANCE);
						EditorGUILayout.Space ();

						EditorGUILayout.PropertyField (propNodesDistribution);
						ShowHelpBox (MSG_NODES_DISTRIBUTION);
						EditorGUILayout.Space ();
					}

					// SHAPE SCALE.
					EditorGUILayout.Slider (propShapeScale, 0.1f, 5f);
					ShowHelpBox (MSG_SHAPE_SCALE);
					EditorGUILayout.Space ();

					// BRANCH SCALE HIERARCHY ADHERENCE.
					EditorGUILayout.Slider (propBranchHierarchyScaleAdherence, 0f, 1f, "Scale Adherence");
					ShowHelpBox (MSG_BRANCH_HIERARCHY_SCALE);
					EditorGUILayout.Space ();
				}

				if (EditorGUI.EndChangeCheck () && 
					propMinNodes.intValue <= propMaxNodes.intValue && 
					propMinNodeLength.floatValue <= propMaxNodeLength.floatValue)
				{
					ShapeCatalog.ShapeItem shapeItem = shapeCatalog.GetShapeItem (selectedShapeIndex); // -1 because of the 'default' option
					if (shapeItem == null || propMeshMode.enumValueIndex == (int)BranchMeshGeneratorElement.MeshMode.Default) {
						branchMeshGeneratorNode.branchMeshGeneratorElement.shapeCollection = null;
					} else {
						branchMeshGeneratorNode.branchMeshGeneratorElement.selectedShapeId = shapeItem.id;
						branchMeshGeneratorNode.branchMeshGeneratorElement.shapeCollection = shapeItem.shapeCollection;
					}
					EditorUtility.SetDirty (branchMeshGeneratorNode);
					ApplySerialized ();
					UpdatePipeline (GlobalSettings.processingDelayHigh);
					NodeEditorFramework.NodeEditor.RepaintClients ();
					branchMeshGeneratorNode.branchMeshGeneratorElement.Validate ();
					SetUndoControlCounter ();
				}
			}

			if (branchMeshGeneratorNode.branchMeshGeneratorElement.showLODInfoLevel == 1) {
			} else if (branchMeshGeneratorNode.branchMeshGeneratorElement.showLODInfoLevel == 2) {
			} else {
				EditorGUILayout.HelpBox ("LOD0\nVertex Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.verticesCountSecondPass +
					"\nTriangle Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.trianglesCountSecondPass + "\nLOD1\nVertex Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.verticesCountFirstPass +
				"\nTriangle Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.trianglesCountFirstPass, MessageType.Info);
			}
			EditorGUILayout.Space ();
	
			// Field descriptors option.
			DrawFieldHelpOptions ();
		}
		#endregion
	}
}