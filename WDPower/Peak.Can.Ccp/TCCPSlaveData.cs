using System.Runtime.InteropServices;

namespace Peak.Can.Ccp
{
	public struct TCCPSlaveData
	{
		public ushort EcuAddress;

		public uint IdCRO;

		public uint IdDTO;

		[MarshalAs(UnmanagedType.U1)]
		public bool IntelFormat;
	}
}
