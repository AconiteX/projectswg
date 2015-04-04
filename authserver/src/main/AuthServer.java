package main;

import java.io.IOException;
import java.net.InetSocketAddress;

import org.apache.mina.core.service.IoAcceptor;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.transport.socket.nio.NioSocketAcceptor;

public class AuthServer {
	public static void main(String[] args) throws IOException {
		IoAcceptor acceptor = new NioSocketAcceptor();
		acceptor.setHandler(new AuthServerHandler());
		acceptor.getSessionConfig().setReadBufferSize(2048);
		acceptor.setCloseOnDeactivation(true);
		
		try {		
			acceptor.bind(new InetSocketAddress(43000));
			System.out.println("Started server.");
		} catch (IOException e) {
			e.printStackTrace();
			System.out.println("An error occured - check that"
					+ " an instance of the login server is not already running.");
			
			for (IoSession ss : acceptor.getManagedSessions().values()) {
			  ss.close(true);
			}
			acceptor.unbind();
			acceptor.dispose();
		}
		
	}
}
