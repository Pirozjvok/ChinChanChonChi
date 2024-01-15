using System;

namespace Peak.Can.Ccp
{
	[Flags]
	public enum TCCPResourceMask : byte
	{
		CCP_RSM_NONE = 0,
		CCP_RSM_CALIBRATION = 1,
		CCP_RSM_DATA_ADQUISITION = 2,
		CCP_RSM_MEMORY_PROGRAMMING = 0x40
	}
}
