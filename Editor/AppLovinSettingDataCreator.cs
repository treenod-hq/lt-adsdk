using System.IO;
using UnityEngine;
using UnityEditor;

namespace Treenod.Ad
{
    public class AppLovinSettingDataCreator
    {
        [MenuItem( "AdSdk/AppLovin/셋팅데이터 생성" )]
        private static void CreateAppLovinSettingData ()
        {
            if ( !Directory.Exists( AppLovinSettingData.DIRECTORY ) )
            {
                Directory.CreateDirectory( AppLovinSettingData.DIRECTORY );
            }

            string path = AssetDatabase.GenerateUniqueAssetPath( string.Format( "{0}{1}.asset", AppLovinSettingData.DIRECTORY, AppLovinSettingData.FILENAME ) );

            AppLovinSettingData asset = ScriptableObject.CreateInstance<AppLovinSettingData>();
            AssetDatabase.CreateAsset( asset, path );
            AssetDatabase.Refresh();
        }
    }
}