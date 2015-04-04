package engine.resources.service;

import java.io.IOException;
import java.io.OutputStream;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import main.NGECore;

import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IoSession;
import org.python.util.PythonInterpreter;

import engine.servers.InteractiveJythonServer;

public class InteractiveJythonAcceptor extends IoHandlerAdapter {

	protected ExecutorService eventThreadPool;
	private InteractiveJythonServer server;
	private NGECore core;
	
	public void setServer(InteractiveJythonServer server) {
		this.server =server;
		eventThreadPool = Executors.newCachedThreadPool();
	}

	@Override
	public void exceptionCaught(IoSession session, Throwable exception)
			throws Exception {
		
		session.write(exception.getStackTrace());
	}

	@Override
	public void messageReceived(final IoSession session, final Object message) throws Exception {
		
    	eventThreadPool.execute(new Runnable() {
	    	public void run() {
	    		
	    		
		    		String s = message.toString();
					System.out.println(s);
		    		if (s.trim().equalsIgnoreCase("quit")) {
		    			session.write("BYE");
		    			session.close(true);
		    			return;
		    		}
		    	
		    		Object o = session.getAttribute("JythonSession");
		    		if (!(o instanceof PythonInterpreter)) {
		    			session.write("problem with python interpreter");
		    			return;
		    		}
		    		PythonInterpreter python = ((PythonInterpreter)o);
		    		try {
		    			python.exec(s);
		    		} catch (Exception e) {
		    			session.write(e.toString());
		    		}
		    		
		    	}
    	});
	}

	@Override
	public void sessionCreated(IoSession session) throws Exception {
		PythonInterpreter python = new PythonInterpreter();
		python.setOut(new SessionOutputStream(session));
		session.setAttribute("JythonSession", python);
		if (core != null) {
			python.set("core", core);
			session.write("core set.");
		}
		session.write(">>>");
	}
	
	private class SessionOutputStream extends OutputStream {
		
		private IoSession session;
		private ByteBuffer buffer;
		
		public SessionOutputStream(IoSession session) {
			this.session = session;
			buffer = ByteBuffer.allocate(2048);
		}

		@Override
		public void write(int output) throws IOException {
			if (session == null) { return; }
			buffer.put((byte) output);
			if (output == 10 || buffer.position() == 2047) {
				String out = new String ( buffer.array(), Charset.forName("UTF-8"));
				buffer = ByteBuffer.allocate(2048);				session.write(out);
				System.out.println(out);
				
			}
			
		}
		
	}

	public void setCore(NGECore core) {
		this.core = core;
	}

}
