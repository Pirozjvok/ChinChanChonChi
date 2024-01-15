using System;

namespace Peak.Can.Basic
{
	[Flags]
	public enum TPCANMessageType : byte
	{
		PCAN_MESSAGE_STANDARD = 0,
		PCAN_MESSAGE_RTR = 1,
		PCAN_MESSAGE_EXTENDED = 2,
		PCAN_MESSAGE_FD = 4,
		PCAN_MESSAGE_BRS = 8,
		PCAN_MESSAGE_ESI = 0x10,
		PCAN_MESSAGE_ERRFRAME = 0x40,
		PCAN_MESSAGE_STATUS = 0x80
	}
}
