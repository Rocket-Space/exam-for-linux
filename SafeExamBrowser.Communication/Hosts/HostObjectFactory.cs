/*
 * Copyright (c) 2026 ETH Zürich, IT Services
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.ServiceModel;
using SafeExamBrowser.Communication.Contracts;
using SafeExamBrowser.Communication.Contracts.Hosts;

namespace SafeExamBrowser.Communication.Hosts
{
	/// <summary>
	/// Default implementation of the <see cref="IHostObjectFactory"/> utilizing WCF (<see cref="ServiceHost"/>).
	/// </summary>
	public class HostObjectFactory : IHostObjectFactory
	{
		public IHostObject CreateObject(string address, ICommunication communicationObject)
		{
#if WINDOWS
			var host = new Host(communicationObject);

			host.AddServiceEndpoint(typeof(ICommunication), new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport), address);

			return host;
#else
			return new NoOpHost();
#endif
		}

#if WINDOWS
		private class Host : ServiceHost, IHostObject
		{
			internal Host(object singletonInstance, params Uri[] baseAddresses) : base(singletonInstance, baseAddresses)
			{
			}
		}
#else
		private class NoOpHost : IHostObject
		{
			public CommunicationState State => CommunicationState.Closed;
			public event EventHandler Closed = delegate { };
			public event EventHandler Closing = delegate { };
			public event EventHandler Faulted = delegate { };
			public event EventHandler Opened = delegate { };
			public event EventHandler Opening = delegate { };
			public void Abort() { }
			public void Close() { }
			public void Close(TimeSpan timeout) { }
			public IAsyncResult BeginClose(AsyncCallback callback, object state) => throw new NotImplementedException();
			public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state) => throw new NotImplementedException();
			public void EndClose(IAsyncResult result) { }
			public void Open() { }
			public void Open(TimeSpan timeout) { }
			public IAsyncResult BeginOpen(AsyncCallback callback, object state) => throw new NotImplementedException();
			public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state) => throw new NotImplementedException();
			public void EndOpen(IAsyncResult result) { }
		}
#endif
	}
}
