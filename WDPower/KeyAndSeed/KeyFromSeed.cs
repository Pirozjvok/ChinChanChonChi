using System.Runtime.InteropServices;

namespace KeyAndSeed
{
	internal class KeyFromSeed
	{
		[DllImport(".\\dll\\PG_Default.dll", EntryPoint = "ASAP1A_CCP_ComputeKeyFromSeed")]
		public static extern bool getKeyFromSeed(byte[] seed, ushort sizeSeed, byte[] key, ushort maxSizeKey, ushort[] sizeKey);
	}
}
