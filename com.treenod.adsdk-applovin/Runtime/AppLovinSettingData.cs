using UnityEngine;

namespace Treenod.Ad
{
    public class AppLovinSettingData : ScriptableObject
    {
        public static string DIRECTORY = "Assets/Resources/AdSdk/";
        public static string RESOURCES_FOLDER = "AdSdk/";
        public static string FILENAME = "AppLovinSettingData";

        public static AppLovinSettingData LoadData ()
        {
            return Resources.Load<AppLovinSettingData>( RESOURCES_FOLDER + FILENAME );
        }

        [SerializeField]
        private string adUnitIdentifierForIOS = "";
        [SerializeField]
        private string adUnitIdentifierForAOS = "";
        [SerializeField]
        private string sdkKey = "";

        public string AdUnitIdentifire
        {
            get
            {
#if UNITY_IOS
            return adUnitIdentifierForIOS;
#else
                return adUnitIdentifierForAOS;
#endif
            }
        }

        public string SdkKey
        {
            get
            {
                return sdkKey;
            }
        }
    }
}
