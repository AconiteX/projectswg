package main;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class DatabaseConnection {
	private String host;
	private String database;
	private String user;
	private String pass;
	private Connection connection = null;
	private Statement statement = null;
	private String type;

	public DatabaseConnection() {
	}

	public DatabaseConnection(String database, String user, String pass,
			String type) {
		this.database = database;
		this.user = user;
		this.pass = pass;
		this.type = type;
	}

	public boolean connect() {
		if ((this.database == null) || (this.user == null)
				|| (this.pass == null)) {
			return false;
		}
		return connect(this.host, this.database, this.user, this.pass,
				this.type);
	}

	public boolean connect(String host, String database, String user,
			String pass, String type) {
		ResultSet resultSet = null;
		try {
			if (type == "mysql") {
				this.connection = DriverManager.getConnection(
						"jdbc:" + type + "://" + host + "/" + database
								+ "?autoReconnect=true", user, pass);
				this.statement = this.connection.createStatement();
				resultSet = this.statement.executeQuery("SELECT VERSION()");

				return true;
			}
			this.connection = DriverManager.getConnection("jdbc:" + type
					+ "://" + host + "/" + database, user, pass);
			this.statement = this.connection.createStatement();
			resultSet = this.statement.executeQuery("SELECT VERSION()");

			return true;
		} catch (SQLException e) {
			e.printStackTrace();
		} finally {
			try {
				if (resultSet != null) {
					resultSet.close();
				}
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}
		return false;
	}

	public void close() {
		try {
			if (this.statement != null) {
				this.statement.close();
			}
			if (this.connection != null) {
				this.connection.close();
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	public ResultSet query(String query) {
		if (this.statement == null) {
			return null;
		}
		try {
			synchronized (this.statement) {
				return this.statement.executeQuery(query);
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
		return null;
	}

	public PreparedStatement preparedStatement(String preparedStatement)
			throws SQLException {
		return this.connection.prepareStatement(preparedStatement);
	}
}
