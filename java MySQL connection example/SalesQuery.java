/*
 * Wilbur Abbott
 * CSCD 327
 * Final Assignment - Sales Report
 * 
 * This is the query file, it builds the query and prints out the results
 * 
 */

import java.io.IOException;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.NumberFormat;

public class SalesQuery
{
	private Connection conn = null;
	private Statement statement = null;
	private ResultSet resultSet = null;

	public SalesQuery(Connection c)throws SQLException
	{
		conn = c;
		// Statements allow to issue SQL queries to the database
		statement = conn.createStatement();
	}

	public void BuildReport() throws IOException, SQLException
	{
		System.out.println("Things are working so far");
		
		PrintTotalLine();
		PrintOrders();
	}
	
	public void PrintTotalLine() throws SQLException
	{
		statement = conn.createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE, ResultSet.CONCUR_READ_ONLY);
		resultSet = statement.executeQuery("SELECT count(DISTINCT OrderNum) toto, Cost * sum(OrdQty) totc, sum(OrdQty) totq, UnitWeight * sum(OrdQty) totw FROM invoice NATURAL JOIN invoicelineitem NATURAL JOIN inventory;");
		
		while(resultSet.next())
		{
			int totalOrders = resultSet.getInt("toto");
			double totalCost = resultSet.getDouble("totc");
			int totalQuantity = resultSet.getInt("totq");
			double totalWeight = resultSet.getDouble("totw");
			System.out.println("Total Orders: " + totalOrders + "\tTotal Cost: $" + totalCost + "\tTotal Quantity: " + totalQuantity + "\tTotal Weight" + totalWeight);
		}
	}
	
	public void PrintOrders() throws SQLException
	{
		statement = conn.createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE, ResultSet.CONCUR_READ_ONLY);
		resultSet = statement.executeQuery("SELECT DISTINCT ordernum o FROM invoice;");
		
		int i = 0;
		int[] ordNums = new int[10];

		while(resultSet.next())
		{
			ordNums[i] = resultSet.getInt("o");
			i++;
		}
		
		for(int n = 0; n < i; n++)
		{
			resultSet = statement.executeQuery("SELECT * FROM customer NATURAL JOIN invoice NATURAL JOIN invoicelineitem NATURAL JOIN inventory WHERE OrderNum = " + ordNums[n] + ";");
			
			System.out.println("\n***************************************\nOrder information for Order number " + ordNums[n] + "\n***************************************");
			
			double totalPrice = 0;
			
			while(resultSet.next())
			{
				int id = resultSet.getInt("CustNum");
				String name = resultSet.getString("CustName");
				String addyStreet = resultSet.getString("Street");
				String addyCity = resultSet.getString("City");
				String addyState = resultSet.getString("State");
				int addyZip = resultSet.getInt("Zip");
				
				System.out.println("Customer Information -- ID number: " + id + "\tCustomer Name: " + name + "\tAddress: " + addyStreet + " " + addyCity + ", " + addyState + " " + addyZip);
				
				String sku = resultSet.getString("SKU");
				String desc = resultSet.getString("Description");
				double price = resultSet.getDouble("UnitPrice");
				int quant = resultSet.getInt("OrdQty");
				double weight = resultSet.getDouble("UnitWeight");
				double extPrice = price * quant;
				double extWeight = weight * quant;
				totalPrice += extPrice;
				
				System.out.println("\tLineItem Information -- SKU: " + sku + "\tDescription: " + desc + "\tPrice: " + price + "\tQuantity: "
				+ quant + "\tWeight: " + weight + "\tExtended Price: " + extPrice + "\tExtended Weight: " + extWeight);
			}
			
			NumberFormat f = NumberFormat.getCurrencyInstance();
			String s = f.format(totalPrice);
			System.out.println("Total Order Price: " + s);
		}
	}
}