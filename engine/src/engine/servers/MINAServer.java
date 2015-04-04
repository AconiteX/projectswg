package engine.servers;

import java.net.InetSocketAddress;

import org.apache.mina.core.filterchain.DefaultIoFilterChainBuilder;
import org.apache.mina.core.service.IoHandler;
import org.apache.mina.filter.codec.ProtocolCodecFilter;
import org.apache.mina.transport.socket.DatagramSessionConfig;
import org.apache.mina.transport.socket.nio.NioDatagramAcceptor;

import engine.protocol.SOEProtocolCodecFactory;
import engine.resources.config.Config;
import engine.resources.config.DefaultConfig;
import engine.resources.service.NetworkDispatch;

public class MINAServer {
	
	public static final boolean debugOutput = false;
	
	private int port;
	private IoHandler iohandler;
	private NioDatagramAcceptor nioacceptor;
	
	public MINAServer(IoHandler iohandler, int Port) {
		this.port = Port;
		this.iohandler = iohandler;
	}
	
	private void createDependencies() throws Exception {
		nioacceptor = new NioDatagramAcceptor();
		nioacceptor.setHandler(iohandler);
		nioacceptor.bind(new InetSocketAddress(port));
		if(iohandler instanceof NetworkDispatch) {
			((NetworkDispatch) iohandler).setServer(this);
		}
		// create new filter for SOEProtocol
		Config config = new Config();
		config.setFilePath("nge.cfg");
		if(!(config.loadConfigFile()))
		{
			config = DefaultConfig.getConfig();
		}

		DefaultIoFilterChainBuilder IOFilterChain = nioacceptor.getFilterChain();
		//IOFilterChain.addLast("logger", new LoggingFilter());
		IOFilterChain.addLast("SOE_Protocol", new ProtocolCodecFilter(new SOEProtocolCodecFactory()));

		// make sure the socket reuses the address
		DatagramSessionConfig dcfg = nioacceptor.getSessionConfig();
		dcfg.setReuseAddress(true);

	}
	
	public void start() {
		try {
			createDependencies();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	public void stop() {
		nioacceptor.dispose();
	}

	public NioDatagramAcceptor getNioacceptor() {
		return nioacceptor;
	}

	public void setNioacceptor(NioDatagramAcceptor nioacceptor) {
		this.nioacceptor = nioacceptor;
	}
}
