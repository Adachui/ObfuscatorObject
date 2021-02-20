#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("gBF3Wafx6w8BOmheh9ndejF/ZYK3gxbWAHgN0hAMyCXyXDPOrDoeP//ZM/VghFxOV0EK7oJM7bjy8ls5pPpbQUlAltl8LkUAP3tQP+7Zfce7CYqpu4aNgqENww18hoqKio6LiGv1Pgsno/RvoIO4qY1/qFECo7znMC8BRXUTA8JIY4EOCw46G5cd3XosJCGvqEP3iPLxiOoMO+zdB/OSinRxn3bqRBWW767/Ym4sFAfk9YKkhb9Fql7kXCIBaiGgbRxmu046GvF7p0MqwPfwyJwwn4fhmACszU3DrQmKhIu7CYqBiQmKiosTZ5JBBERP5/SNo7RL5Hls3o0AFVEkAn9brIkdsc/cNUOsrS8VE53GbSB65cNOakyU6/OMlupyKomIiouK");
        private static int[] order = new int[] { 11,7,4,9,11,10,13,13,9,12,12,13,13,13,14 };
        private static int key = 139;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
