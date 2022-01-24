using System.IO;
using UnityEngine;
using UnityEditor;

namespace Treenod.Ad
{
    public class AppLovinSettingLocater
    {
        private readonly static string ASSET_PATH = @"Assets\";
        private readonly static string PACKAGE_PATH = @"Assets\lt-adsdk-applovin\";
        private readonly static string APPLOVIN_NAME = "MaxSdk";
        private readonly static string PLUGIN_DIR_NAME = "Plugins";
        private readonly static string EXTERNAL_DEPENDENCY_DIR_NAME = "ExternalDependencyManager";
        
        [MenuItem( "AdSdk/AppLovin/앱러빈 SDK -> ASSET 디렉토리로 이동" )]
        private static void MoveAppLovinFileToAssetDirectory ()
        {
            if (MoveDirectory(PACKAGE_PATH, ASSET_PATH))
            {
                EditorUtility.DisplayDialog("알림", "파일 이동했습니다.", "확인");
            }
            else
            {
                EditorUtility.DisplayDialog("알림", "패키지 폴더에 앱러빈 SDK가 없습니다.", "확인");
            }
        }
        
        [MenuItem( "AdSdk/AppLovin/앱러빈 SDK -> 패키지 디렉토리로 이동" )]
        private static void MoveAppLovinFileToPackageDirectory ()
        {
            if (MoveDirectory(ASSET_PATH, PACKAGE_PATH))
            {
                EditorUtility.DisplayDialog("알림", "파일 이동했습니다.", "확인");
            }
            else
            {
                EditorUtility.DisplayDialog("알림", "Assets폴더에 앱러빈 SDK가 없습니다.", "확인");
            }
        }

        private static bool MoveDirectory(string sourcePath, string destPath)
        {
            if ( !Directory.Exists( sourcePath + APPLOVIN_NAME ) || !Directory.Exists( destPath ))
            {
                return false;
            }
            
            Directory.Move(sourcePath + APPLOVIN_NAME, destPath + APPLOVIN_NAME);

            if (Directory.Exists(sourcePath + PLUGIN_DIR_NAME))
            {
                Directory.Move(sourcePath + PLUGIN_DIR_NAME, destPath + PLUGIN_DIR_NAME);
            }
            
            if (Directory.Exists(sourcePath + EXTERNAL_DEPENDENCY_DIR_NAME))
            {
                Directory.Move(sourcePath + EXTERNAL_DEPENDENCY_DIR_NAME, destPath + EXTERNAL_DEPENDENCY_DIR_NAME);
            }

            AssetDatabase.Refresh();
            
            return true;
        }
    }
}