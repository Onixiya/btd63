using MelonLoader;
using Il2CppNinjaKiwi.Common;
using HarmonyLib;
using UnityEngine;
using System.IO;
using System.Reflection;
using Il2CppAssets.Scripts.Unity.Display;
using uObject=UnityEngine.Object;
using Il2CppAssets.Scripts.Unity.Audio;
[assembly: MelonInfo(typeof(btd63.btd63),"btd63","1.0.0","btd63")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace btd63{
    public class btd63:MelonMod{
        private static MelonLogger.Instance mllog=new MelonLogger.Instance("btd63");
        public static AssetBundle bundle=null;
        public static GameObject btd63p=null;
        public static AudioClip btd63a=null;
        public override void OnLateInitializeMelon(){
            Assembly assembly=MelonAssembly.Assembly;
            Stream stream=assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()[0]);
            byte[]bytes=new byte[stream.Length];
            stream.Read(bytes);
            bundle=AssetBundle.LoadFromMemory(bytes);
        }
        public static void Log(object thingtolog,string type="msg"){
            switch(type){
                case"msg":
                    mllog.Msg(thingtolog);
                    break;
                case"warn":
                    mllog.Warning(thingtolog);
                    break;
                 case"error":
                    mllog.Error(thingtolog);
                    break;
            }
        }
        [HarmonyPatch(typeof(LocalizationManager),"GetText")]
        public class LocalizationManagerGetText_Patch{
            public static void Postfix(ref string __result){
                __result="btd63";
            }
        }
        [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0),"_CreateAsync_b__0")]
        public class FactoryCreateAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref Factory.__c__DisplayClass21_0 __instance,ref UnityDisplayNode prototype){
                if(btd63p==null){
                    btd63p=bundle.LoadAssetAsync<GameObject>("btd63p").asset.Cast<GameObject>();
                }
                GameObject gObj=uObject.Instantiate(btd63p,__instance.__4__this.DisplayRoot);
                gObj.name=__instance.objectId.guidRef;
                gObj.transform.position=new(30000,0,30000);
                gObj.AddComponent<UnityDisplayNode>();
                prototype=gObj.GetComponent<UnityDisplayNode>();
                __instance.__4__this.active.Add(prototype);
                __instance.onComplete.Invoke(prototype);
                return false;
            }
        }
        [HarmonyPatch(typeof(AudioFactory),"PlayMusic")]
        public class AudioFactoryPlayMusic_Patch{
            public static void Prefix(ref AudioClip clip){
                if(btd63a==null){
                    btd63a=bundle.LoadAssetAsync<AudioClip>("btd63a").asset.Cast<AudioClip>();
                }
                clip=btd63a;
            }
        }
    }
}