/*
 * Copyright (c) 2026 ETH Zürich, IT Services
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.ServiceModel;
using SafeExamBrowser.Communication.Contracts.Proxies;

namespace SafeExamBrowser.Communication.Proxies
{
	/// <summary>
	/// Default implementation of the <see cref="IProxyObjectFactory"/> utilizing WCF (<see cref="ChannelFactory"/>).
	/// </summary>
	public class ProxyObjectFactory : IProxyObjectFactory
	{
		public IProxyObject CreateObject(string address)
		{
#if WINDOWS
			var endpoint = new EndpointAddress(address);
			var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport) { MaxReceivedMessageSize = 1000000 };
			var channel = ChannelFactory<IProxyObject>.CreateChannel(binding, endpoint);

			return channel;
#else
			return new NoOpProxy();
#endif
		}

#if !WINDOWS
		private class NoOpProxy : IProxyObject
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

			public SafeExamBrowser.Communication.Contracts.Data.ConnectionResponse Connect(Guid? token = null) => new SafeExamBrowser.Communication.Contracts.Data.ConnectionResponse { ConnectionEstablished = false };
			public SafeExamBrowser.Communication.Contracts.Data.DisconnectionResponse Disconnect(SafeExamBrowser.Communication.Contracts.Data.DisconnectionMessage message) => new SafeExamBrowser.Communication.Contracts.Data.DisconnectionResponse { ConnectionTerminated = true };
			public SafeExamBrowser.Communication.Contracts.Data.Response Send(SafeExamBrowser.Communication.Contracts.Data.Message message) => null;
		}
#endif
	}
}
