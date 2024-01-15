using System.Runtime.InteropServices;

namespace Peak.Can.Basic
{
	public struct TPCANMsgFD
	{
		public uint ID;

		[MarshalAs(UnmanagedType.U1)]
		public TPCANMessageType MSGTYPE;

		public byte DLC;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] DATA;
	}
}
