using System;

namespace Peak.Can.Basic
{
	[Flags]
	public enum TPCANStatus : uint
	{
		PCAN_ERROR_OK = 0u,
		PCAN_ERROR_XMTFULL = 1u,
		PCAN_ERROR_OVERRUN = 2u,
		PCAN_ERROR_BUSLIGHT = 4u,
		PCAN_ERROR_BUSHEAVY = 8u,
		PCAN_ERROR_BUSWARNING = 8u,
		PCAN_ERROR_BUSPASSIVE = 0x40000u,
		PCAN_ERROR_BUSOFF = 0x10u,
		PCAN_ERROR_ANYBUSERR = 0x4001Cu,
		PCAN_ERROR_QRCVEMPTY = 0x20u,
		PCAN_ERROR_QOVERRUN = 0x40u,
		PCAN_ERROR_QXMTFULL = 0x80u,
		PCAN_ERROR_REGTEST = 0x100u,
		PCAN_ERROR_NODRIVER = 0x200u,
		PCAN_ERROR_HWINUSE = 0x400u,
		PCAN_ERROR_NETINUSE = 0x800u,
		PCAN_ERROR_ILLHW = 0x1400u,
		PCAN_ERROR_ILLNET = 0x1800u,
		PCAN_ERROR_ILLCLIENT = 0x1C00u,
		PCAN_ERROR_ILLHANDLE = 0x1C00u,
		PCAN_ERROR_RESOURCE = 0x2000u,
		PCAN_ERROR_ILLPARAMTYPE = 0x4000u,
		PCAN_ERROR_ILLPARAMVAL = 0x8000u,
		PCAN_ERROR_UNKNOWN = 0x10000u,
		PCAN_ERROR_ILLDATA = 0x20000u,
		PCAN_ERROR_CAUTION = 0x2000000u,
		PCAN_ERROR_INITIALIZE = 0x4000000u,
		PCAN_ERROR_ILLOPERATION = 0x8000000u
	}
}