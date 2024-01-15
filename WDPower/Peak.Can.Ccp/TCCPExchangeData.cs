using System.Runtime.InteropServices;

namespace Peak.Can.Ccp
{
	public struct TCCPExchangeData
	{
		public byte IdLength;

		public byte DataType;

		[MarshalAs(UnmanagedType.U1)]
		public TCCPResourceMask AvailabilityMask;

		[MarshalAs(UnmanagedType.U1)]
		public TCCPResourceMask ProtectionMask;
	}
}
