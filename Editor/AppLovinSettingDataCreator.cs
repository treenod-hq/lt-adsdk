using System.IO;
using UnityEngine;
using UnityEditor;

namespace Treenod.Ad
{
    public class AppLovinSettingDataCreator
    {
        [MenuItem( "AdSdk/AppLovin/셋팅데이터 생성" )]
        private static void CreateAppLovinSettings ()
        {
            CreateAppLovinSettingData();
            CreateAppLovinSettingFile();
            
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("알림", "파일을 생성했습니다.", "확인");
        }

        private static void CreateAppLovinSettingData()
        {
            string filePath = string.Format("{0}{1}.asset", AppLovinSettingData.DIRECTORY, AppLovinSettingData.FILENAME);
            
            if ( !Directory.Exists( AppLovinSettingData.DIRECTORY ) )
            {
                Directory.CreateDirectory( AppLovinSettingData.DIRECTORY );
            }
            
            if (AssetDatabase.LoadAssetAtPath<AppLovinSettingData>( filePath ) != null)
            {
                return;
            }
            
            string path = AssetDatabase.GenerateUniqueAssetPath( filePath );

            AppLovinSettingData asset = ScriptableObject.CreateInstance<AppLovinSettingData>();
            AssetDatabase.CreateAsset( asset, path );
        }

        private static void CreateAppLovinSettingFile()
        {
            if ( !Directory.Exists( "Assets/MaxSdk" ) )
            {
                Directory.CreateDirectory( "Assets/MaxSdk" );
            }

            AppLovinSettings settings = AppLovinSettings.Instance;
        }
    }
}