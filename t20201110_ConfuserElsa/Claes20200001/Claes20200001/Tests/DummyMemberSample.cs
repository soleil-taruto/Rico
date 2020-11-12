using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class DummyMemberSample
	{
		public int SS_Count;

		public int SS_GetCount()
		{
			return this.SS_Count;
		}

		public void SS_SetCount(int SS_SetCount_Prm)
		{
			this.SS_Count = SS_SetCount_Prm;
		}

		public void SS_ResetCount()
		{
			this.SS_SetCount(0);
		}

		public int SS_NextCount()
		{
			return this.SS_Count++;
		}

		public class SS_ValueInfo
		{
			public int SS_ValueInfo_A;
			public int SS_ValueInfo_B;
			public int SS_ValueInfo_C;
		}

		public SS_ValueInfo SS_Value;

		public SS_ValueInfo SS_GetValue()
		{
			return this.SS_Value;
		}

		public void SS_SetValue(SS_ValueInfo SS_SetValue_Prm)
		{
			this.SS_Value = SS_SetValue_Prm;
		}

		public void SS_Overload_00()
		{
			this.SS_Overload_01(this.SS_NextCount());
		}

		public void SS_Overload_01(int SS_a)
		{
			this.SS_Overload_02(SS_a, this.SS_NextCount());
		}

		public void SS_Overload_02(int SS_a, int SS_b)
		{
			this.SS_Overload_03(SS_a, SS_b, this.SS_NextCount());
		}

		public void SS_Overload_03(int SS_a, int SS_b, int SS_c)
		{
			this.SS_Overload_04(SS_a, SS_b, SS_c, this.SS_GetValue().SS_ValueInfo_A, this.SS_GetValue().SS_ValueInfo_B, this.SS_GetValue().SS_ValueInfo_C);
		}

		public void SS_Overload_04(int SS_a, int SS_b, int SS_c, int SS_a2, int SS_b2, int SS_c2)
		{
			this.SS_SetValue(new SS_ValueInfo()
			{
				SS_ValueInfo_A = SS_a,
				SS_ValueInfo_B = SS_b,
				SS_ValueInfo_C = SS_c,
			});

			this.SS_Overload_05(SS_a2);
			this.SS_Overload_05(SS_b2);
			this.SS_Overload_05(SS_c2);
		}

		public void SS_Overload_05(int SS_v)
		{
			if (SS_v != this.SS_GetCount())
				this.SS_SetCount(SS_v);
			else
				this.SS_Overload_01(SS_v);
		}
	}
}
