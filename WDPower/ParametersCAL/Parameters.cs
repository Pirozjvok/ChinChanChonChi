using System.Runtime.InteropServices;

namespace ParametersCAL
{
	[StructLayout(LayoutKind.Auto, Size = 4)]
	public struct Parameters
	{
		public string NameCH;

		public string NameEN;

		public string Fmt;

		public string pMin;

		public string pMax;

		public string pVal;

		public string pPv;

		public byte Lenth;

		public uint Addr;
	}
}
