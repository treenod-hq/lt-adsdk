using UnityEngine;

namespace Treenod.Ads.AppLovin
{
    public class AppLovinSettingData : ScriptableObject
    {
        public static string DIRECTORY = "Assets/AdSdk/Resources/";
        public static string FILENAME = "AppLovinSettingData";

        public static AppLovinSettingData LoadData ()
        {
            return Resources.Load<AppLovinSettingData>( FILENAME );
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
