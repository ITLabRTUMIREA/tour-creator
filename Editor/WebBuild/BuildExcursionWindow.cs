﻿using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Packages.Excursion360_Builder.Editor.RemoteItemsControllers;
using Packages.Excursion360_Builder.Editor.Viewer;
using Packages.Excursion360_Builder.Editor.WebBuild.RemoteItems;
using Packages.tour_creator.Editor.WebBuild;
using Packages.tour_creator.Editor.WebBuild.GitHubAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Packages.Excursion360_Builder.Editor.WebBuild
{
    class BuildExcursionWindow : EditorWindow
    {
        private string outFolderPath;
        private int imageCroppingLevel;

        private void OnEnable()
        {
            outFolderPath = Tour.Instance != null ? Tour.Instance.targetBuildLocation : null;
            imageCroppingLevel = Tour.Instance != null ? Tour.Instance.croppingLevel : 6;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select Viewer", EditorStyles.boldLabel);
            var selectedViewer = ViewerBuildsGUI.Draw();
            if (selectedViewer == null)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Exporting excursion", EditorStyles.boldLabel);
            DrawExportingSection();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Place desktop viewer", EditorStyles.boldLabel);
            var selectedDesktopClient = DesktopClientBuildsGUI.Draw();

            if (selectedDesktopClient == null)
            {
                return;
            }

            EditorGUILayout.Space();
            DrawExportButton(selectedViewer, selectedDesktopClient);
        }

        private void DrawExportingSection()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Target path");
            outFolderPath = EditorGUILayout.TextField(outFolderPath);
            if (GUILayout.Button("..."))
            {
                outFolderPath = EditorUtility.OpenFolderPanel("Select folder to export", outFolderPath, "");
                if (Tour.Instance != null)
                {
                    Tour.Instance.targetBuildLocation = outFolderPath;
                    EditorUtility.SetDirty(Tour.Instance);
                }
                Repaint();
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();
            var newImageCroppingLevel = EditorGUILayout.IntSlider("Image cropping level", imageCroppingLevel, ImageCropper.MIN_PARTS_COUNT, ImageCropper.MAX_PARTS_COUNT);
            if (newImageCroppingLevel != imageCroppingLevel)
            {
                imageCroppingLevel = newImageCroppingLevel;
                Tour.Instance.croppingLevel = imageCroppingLevel;
                EditorUtility.SetDirty(Tour.Instance);
            }

        }

        private void DrawExportButton(WebViewerBuildPack selectedViewer, DesktopClientBuildPack selectedDesktopClient)
        {
            if (GUILayout.Button("Export"))
            {
                if (!TourExporter.TryGetTargetFolder(outFolderPath))
                {
                    return;
                }
                TourExporter.ExportTour(new TourExporter.ExportOptions(selectedViewer, selectedDesktopClient, outFolderPath, imageCroppingLevel));
            }
        }
    }
}
