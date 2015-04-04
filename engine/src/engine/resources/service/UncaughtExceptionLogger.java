package engine.resources.service;

import java.io.BufferedWriter;
import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.lang.Thread.UncaughtExceptionHandler;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;

// This custom class will allow us to catch any UNCAUGHT exceptions that are thrown and log them to a file with some other ease-of-use features I've thrown (pun!) in. 
public class UncaughtExceptionLogger implements UncaughtExceptionHandler {

	private boolean logExceptions = false;
	private boolean silent = false;
	
	private static int maxExceptionsLogged = 20;
	private volatile int exceptionCount;

	private String logDirectory = "";
	private long timestamp = 0L;
	
	public UncaughtExceptionLogger() {
		
	}
	
	public UncaughtExceptionLogger(String logDirectory) {
		this.logDirectory = logDirectory;
		this.logExceptions = true;
		
		File f = new File(logDirectory);

		if (!f.exists()) {
			try {
				Files.createDirectory(Paths.get(logDirectory));
				System.out.println("Created logs directory.");
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
	}
	
	@Override
	public void uncaughtException(Thread t, Throwable e) {
		// t - thread exception occured in
		// e - uncaught throwable (exception)
		if (logExceptions) {
			writeToLog(e);
		}
		
		if (!silent) e.printStackTrace();
	}

	private void writeToLog(Throwable exception) {
		try {
			exceptionCount++;
			if (exceptionCount > maxExceptionsLogged || timestamp == 0L) {
				createLog();
			}
			BufferedWriter writer = Files.newBufferedWriter(Paths.get(logDirectory + "\\" + timestamp + ".txt"), StandardOpenOption.WRITE, StandardOpenOption.APPEND);
			PrintWriter out = new PrintWriter(writer);
			out.println("=== Exception " + exceptionCount + "===");
			exception.printStackTrace(out);
			out.close();
			if (writer != null) writer.close();
		} catch (Exception e1) {
			e1.printStackTrace();
		}
	}
	
	private void createLog() throws IOException {
		timestamp = System.currentTimeMillis();
		File log = new File(logDirectory + "\\" + timestamp + ".txt");
		log.createNewFile();
	}

	public void setLogExceptions(boolean logExceptions) {
		this.logExceptions = logExceptions;
	}

	public void setSilent(boolean silent) {
		this.silent = silent;
	}
}
