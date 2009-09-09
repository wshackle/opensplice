namespace test.sacs
{
	/// <date>Jun 2, 2005</date>
	public class Listener6 : Test.Framework.TestCase
	{
		public Listener6() : base("sacs_listener_tc6", "sacs_listener", "listener", "Test if a DomainParticipantListener works."
			, "Test if a DomainParticipantListener works.", null)
		{
			this.AddPreItem(new test.sacs.ListenerInit());
			this.AddPostItem(new test.sacs.ListenerDeinit());
		}

		public override Test.Framework.TestResult Run()
		{
			DDS.IDomainParticipant participant;
			mod.tstDataWriter writer;
			mod.tstDataReader reader;
			mod.tst[] tstHolder;
			DDS.SampleInfo[] sampleInfoHolder;
			Test.Framework.TestResult result;
			test.sacs.MyParticipantListener listener;
			test.sacs.MyDataReaderListener listener2;
			string expResult = "DomainParticipantListener test succeeded.";
			DDS.ReturnCode rc;
			result = new Test.Framework.TestResult(expResult, string.Empty, Test.Framework.TestVerdict
				.Pass, Test.Framework.TestVerdict.Fail);
			participant = (DDS.IDomainParticipant)this.ResolveObject("participant");
			writer = (mod.tstDataWriter)this.ResolveObject("datawriter");
			reader = (mod.tstDataReader)this.ResolveObject("datareader");
			listener = new test.sacs.MyParticipantListener();
			listener2 = new test.sacs.MyDataReaderListener();
			rc = participant.Set_listener(listener, 0);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "set_listener on DomainParticipant failed.");
				return result;
			}
			rc = participant.Set_listener(null, 0);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "Null Listener could not be attached.");
				return result;
			}
			rc = participant.Set_listener(listener, 1012131412);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "Listener could not be attached (2).");
				return result;
			}
			rc = participant.Set_listener(listener, DDS.DATA_AVAILABLE_STATUS.Value);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "Listener could not be attached (3).");
				return result;
			}
			mod.tst data = new mod.tst(1, 2, 3);
			rc = writer.Write(data, 0L);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "tstDataWriter.write failed.");
				return result;
			}
			try
			{
				java.lang.Thread.Sleep(3000);
			}
			catch (System.Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			if (!listener.onDataAvailableCalled)
			{
				result.Result = "on_data_available not called.");
				return result;
			}
			listener.Reset();
			tstHolder = new mod.tst[]();
			sampleInfoHolder = new DDS.SampleInfo[]();
			rc = reader.Take(tstHolder, sampleInfoHolder, 1, DDS.SampleStateKind.Any, DDS.ANY_VIEW_STATE
				.Value, DDS.InstanceStateKind.Any);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "tstDataReader.take failed.");
				return result;
			}
			rc = reader.Set_listener(listener2, DDS.DATA_AVAILABLE_STATUS.Value);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "Listener could not be attached (4).");
				return result;
			}
			rc = writer.Write(data, 0L);
			if (rc != DDS.ReturnCode.Ok)
			{
				result.Result = "tstDataWriter.write failed.");
				return result;
			}
			try
			{
				java.lang.Thread.Sleep(3000);
			}
			catch (System.Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			if (listener.onDataAvailableCalled)
			{
				result.Result = "on_data_available is called but shouldn't be.");
				return result;
			}
			if (!listener2.onDataAvailableCalled)
			{
				result.Result = "on_data_available not called (2).");
				return result;
			}
			listener.Reset();
			listener2.Reset();
			result.Result = expResult);
			result.Verdict = Test.Framework.TestVerdict.Pass);
			return result;
		}
	}
}
