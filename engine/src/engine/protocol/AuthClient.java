package engine.protocol;

import java.lang.reflect.InvocationTargetException;
import java.net.InetSocketAddress;

import main.NGECore;

import org.apache.mina.core.RuntimeIoException;
import org.apache.mina.core.future.ConnectFuture;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.transport.socket.nio.NioSocketConnector;

public class AuthClient implements Runnable {
	
	private String port = "43000";
	private NGECore core;

	public AuthClient(NGECore core) {
		
		this.core = core;
	    Thread thread = new Thread(this);
	    thread.start();
	}

	@Override
	public void run() {

		NioSocketConnector connector = new NioSocketConnector();
	    connector.setConnectTimeoutMillis(2000);

	    connector.setHandler(new AuthClientHandler(core));
	    IoSession session;

	    for (;;) {
	        try {
	            ConnectFuture future = connector.connect(new InetSocketAddress("23.29.118.50", Integer.parseInt(port)));
	            future.awaitUninterruptibly();
	            session = future.getSession();
	            break;
	        } catch (RuntimeIoException e) {
	            System.err.println("Failed to connect.");
				try {
					Class<?> c = Class.forName("java.lang.System");
					c.getMethod("exit", int.class).invoke(c, 0);
				} catch (ClassNotFoundException | NoSuchMethodException | SecurityException | IllegalAccessException | IllegalArgumentException | InvocationTargetException e1) {
					e1.printStackTrace();
				}

	        }
	    }

	    session.getCloseFuture().awaitUninterruptibly();
	    connector.dispose();
		
	}

}
