using System.Runtime.InteropServices;

namespace Peak.Can.Ccp
{
	public struct TCCPMsg
	{
		public uint Source;

		public byte Length;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Data;
	}
}
