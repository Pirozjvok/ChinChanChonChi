using System.Runtime.InteropServices;

namespace Peak.Can.Basic
{
	public struct TPCANMsg
	{
		public uint ID;

		[MarshalAs(UnmanagedType.U1)]
		public TPCANMessageType MSGTYPE;

		public byte LEN;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] DATA;
	}
}
