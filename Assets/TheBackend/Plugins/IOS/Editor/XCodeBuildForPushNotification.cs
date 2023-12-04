#if UNITY_IOS && UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;

public class XCodeBuildForPushNotification
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetForFramework = proj.GetUnityFrameworkTargetGuid();
            
            // UserNotifications.framework를 추가합니다.
            proj.AddFrameworkToProject(targetForFramework, "UserNotifications.framework", false);
            File.WriteAllText(projPath, proj.WriteToString());

            // Push Notifications Capability를 추가합니다.
            string targetForCapability = proj.GetUnityMainTargetGuid();

            var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, targetForCapability);
            manager.AddPushNotifications(true);
            manager.WriteToFile();
        }
    }
}
#endif