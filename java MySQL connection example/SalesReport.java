/*
 * Wilbur Abbott
 * CSCD 327
 * Final Assignment - Sales Report
 * 
 * This is the main file, it does main stuff and delegates to the query file for operations
 * 
 */

import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class SalesReport
{
	public static void main(String[] args)
	{
		try 
		{
			Connection conn = getConnection();
			SalesQuery report = new SalesQuery(conn);

			report.BuildReport();

			conn.close();
		} 
		catch (SQLException e)
		{
			e.printStackTrace();
		}
		catch (IOException e)
		{
			e.printStackTrace();
		}
	}

	public static Connection getConnection() throws SQLException
	{
		Connection connection;
		
		try
		{
			Class.forName("com.mysql.jdbc.Driver").newInstance();
		} 
		catch (InstantiationException e1) 
		{
			e1.printStackTrace();
		} catch (IllegalAccessException e1) 
		{
			e1.printStackTrace();
		} catch (ClassNotFoundException e1) 
		{
			e1.printStackTrace();
		}
		//Create a connection to the database
		String serverName = "localhost";
		String mydatabase = "salesorders";
		String url = "jdbc:mysql://" + serverName + "/" + mydatabase; // a JDBC url
		String username = "root";
		String password = "1234";
		connection = DriverManager.getConnection(url, username, password);
		return connection;
	}
}