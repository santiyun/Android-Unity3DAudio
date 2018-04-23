using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class XcodeProject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#if UNITY_EDITOR
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject) {
		if (buildTarget == BuildTarget.iOS) {
			// Handle xcodeproj
			string projPath = PBXProject.GetPBXProjectPath (pathToBuiltProject);
			PBXProject proj = new PBXProject ();
			proj.ReadFromString (File.ReadAllText (projPath));
			string target = proj.TargetGuidByName (PBXProject.GetUnityTargetName ());

			proj.SetBuildProperty (target, "ENABLE_BITCODE", "NO");

			proj.AddFrameworkToProject (target, "libxml2.tbd", false);
			proj.AddFrameworkToProject (target, "libz.tbd", false);

			File.WriteAllText (projPath, proj.WriteToString ());

			// Handle plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			PlistElementDict rootDict = plist.root;
			//rootDict.SetString ("CFBundleDisplayName", "TTTRtcEngineUnityDemo");
			rootDict.SetString ("NSCameraUsageDescription", "是否允许此App使用您的相机？");
			rootDict.SetString ("NSMicrophoneUsageDescription", "是否允许此App使用您的麦克风？");
			rootDict.SetBoolean ("UIFileSharingEnabled", true);

			File.WriteAllText(plistPath, plist.WriteToString()); 
		}
	}
	#endif
}
