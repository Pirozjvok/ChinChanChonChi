using System.Runtime.InteropServices;

namespace Peak.Can.Ccp
{
	public struct TCCPStartStopData
	{
		[MarshalAs(UnmanagedType.U1)]
		public TCCPStartStopMode Mode;

		public byte ListNumber;

		public byte LastODTNumber;

		public byte EventChannel;

		public ushort TransmissionRatePrescaler;
	}
}
