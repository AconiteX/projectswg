package engine.servers;

import java.net.InetSocketAddress;
import java.nio.charset.Charset;

import main.NGECore;

import org.apache.mina.core.service.IoHandler;
import org.apache.mina.filter.codec.ProtocolCodecFilter;
import org.apache.mina.filter.codec.textline.TextLineCodecFactory;
import org.apache.mina.transport.socket.nio.NioSocketAcceptor;

import engine.resources.service.InteractiveJythonAcceptor;

public class InteractiveJythonServer {

	private NioSocketAcceptor nioacceptor;
	private NGECore core;
	private int port;
	private IoHandler iohandler;

	
	public InteractiveJythonServer(IoHandler iohandler, int port, NGECore core) {
		this.port = port;
		this.iohandler = iohandler;
		this.core = core;
	}
	

	private void createDependencies() throws Exception {
		nioacceptor = new NioSocketAcceptor();
		nioacceptor.getFilterChain().addLast("codec", new ProtocolCodecFilter( new TextLineCodecFactory( Charset.forName( "UTF-8" ))));
		nioacceptor.setHandler(iohandler);
		nioacceptor.getSessionConfig().setReadBufferSize(2048);
		nioacceptor.bind(new InetSocketAddress("127.0.0.1", port));
		if(iohandler instanceof InteractiveJythonAcceptor) {
			((InteractiveJythonAcceptor) iohandler).setServer(this);
			((InteractiveJythonAcceptor) iohandler).setCore(core);
		}
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
	
	public NioSocketAcceptor getNioacceptor() {
		return nioacceptor;
	}

	public void setNioacceptor(NioSocketAcceptor nioacceptor) {
		this.nioacceptor = nioacceptor;
	}
	
}
