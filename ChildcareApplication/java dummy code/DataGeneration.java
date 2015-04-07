import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.*;

/*
   Small program to generate SQL scripts that will fill our program with data for testing
   
   test_data_administrators.sql   
   test_data_guardians.sql
   test_data_children.sql
   test_data_families.sql
   test_data_connections.sql
   test_data_transactions.sql
*/
 
public class DataGeneration
{
	public static void main(String[] args) throws IOException
	{
		int a = 5;		//# of administrators
		int g = 60;		//# of guardians
		int c = 100;	//# of children
		int f = 40;		//# of families
		int con = 200;	//# of connections
		int t = 500;	//# of transactions
		
		//create (x) administrators
		generateAdministrator(a);
		
		//create (x) guardians
		generateGuradians(g);
		
		//create (x) children
		generateChild(c);
		
		//create (x) families
		generateFamilies(f);
		
		//create (x) connections from (y) guardians and (z) children and (w) families
		generateConnections(con, g, c, f);
		
		//create (x) transactions
		generateTransactions(t, con);
	}

	private static void generateAdministrator(int n) throws IOException
	{
		String administrator_path = "test_data_administrators.sql";
		String administrator_script = "";
		
		for(int i = 0; i < n; i++)
		{
			administrator_script = administrator_script
								+ "INSERT INTO Administrator"
								+ "\r\nVALUES (\"TestAdmin #" + (i + 1) + "\","
								+ "\r\n\t\"1234\","
								+ "\r\n\t\"" + (i%2 + 1) + "\","
								+ "\r\n\t\"tester@testmail.test\");";
			
			administrator_script = administrator_script + "\r\n\r\n";
		}
		
		writeScript(administrator_path, administrator_script);
	}
	
	private static void generateGuradians(int n) throws IOException
	{
		String guardian_path = "test_data_guardians.sql";
		String guardian_script = "";
		
		for(int i = 0; i < n; i++)
		{
			guardian_script = guardian_script
								+ "INSERT INTO Guardian"
								+ "\r\nVALUES (\"" + (String.format("%06d", (i + 123450))) + "\","
								+ "\r\n\t\"1234\","
								+ "\r\n\t\"TestGuardian\","
								+ "\r\n\t\"#" + (i + 123450) + "\","
								+ "\r\n\t\"(509) 123-4567\","
								+ "\r\n\t\"tester@testmail.test\","
								+ "\r\n\t\"123 Test Street\","
								+ "\r\n\t\"Apartment " + (i + 1) + "\","
								+ "\r\n\t\"Spokane\","
								+ "\r\n\t\"WA\","
								+ "\r\n\t\"99209\","
								+ "\r\n\t\"../../Pictures/default.jpg\","
								+ "\r\n\tNULL);";
			
			guardian_script = guardian_script + "\r\n\r\n";
		}
		
		writeScript(guardian_path, guardian_script);
	}

	private static void generateChild(int n) throws IOException
	{
		String child_path = "test_data_children.sql";
		String child_script = "";
		
		for(int i = 0; i < n; i++)
		{
			child_script = child_script
								+ "INSERT INTO Child"
								+ "\r\nVALUES (\"" + (String.format("%06d", (i + 1))) + "\","
								+ "\r\n\t\"TestChild\","
								+ "\r\n\t\"#" + (i + 1) + "\","
								+ "\r\n\t\'" + randomDate(2003, 2014) + "\',"
								+ "\r\n\t\"ALLERGIES GO HERE\","
								+ "\r\n\t\"MEDICAL CONDITIONS GO HERE\","
								+ "\r\n\t\"../../Pictures/default.jpg\","
								+ "\r\n\tNULL);";
			
			child_script = child_script + "\r\n\r\n";
		}
		
		writeScript(child_path, child_script);
	}
	
	private static void generateFamilies(int n) throws IOException
	{
		String family_path = "test_data_families.sql";
		String family_script = "";
		
		for(int i = 0; i < n; i++)
		{
			family_script = family_script
								+ "INSERT INTO Family"
								+ "\r\nVALUES (\"" + (String.format("%05d", (i + 12345))) + "\", "
								+ "NULL);";
			
			family_script = family_script + "\r\n\r\n";
		}
		
		writeScript(family_path, family_script);		
	}
	
	private static void generateConnections(int n, int g, int c, int f) throws IOException
	{
		String connection_path = "test_data_connections.sql";
		String connection_script = "";
		
		/* IMPORTANT
		 * 
		 * currently these test connections are set randomly with no concern for duplication
		 * meaning there are possibly 2 separate connections that connect the same guardian with the same child and a different family 
		 * or various other unlikely scenarios 
		 */
		
		for(int i = 0; i < n; i++)
		{
			connection_script = connection_script
					+ "INSERT INTO AllowedConnections"
					+ "\r\nVALUES (\"" + (String.format("%06d", (i + 1))) + "\", "
					+ "\"" + String.format("%06d", (randBetween(1,g))) + "\", "
					+ "\"" + String.format("%06d", (randBetween(1,c))) + "\", "
					+ "\"" + String.format("%06d", (randBetween(1,f))) + "\");";
			connection_script = connection_script + "\r\n\r\n";
		}
		
		writeScript(connection_path, connection_script);
	}
	
	private static void generateTransactions(int n, int con) throws IOException
	{
		String transaction_path = "test_data_transactions.sql";
		String transaction_script = "";
			
		for(int i = 0; i < n; i++)
		{
			String randate = randomDate(2014, 2015);
			
			transaction_script = transaction_script
								+ "INSERT INTO ChildcareTransaction"
								+ "\r\nVALUES (\"" + (String.format("%06d", (i + 1))) + "\", "
								+ "\r\n\t\"" + String.format("%06d", randBetween(1,4)) + "\", "
								+ "\r\n\t\"" + String.format("%06d", randBetween(1, con)) + "\", "
								+ "\r\n\t'" + randate + "', "
								+ "\r\n\t'" + String.format("%02d", randBetween(9, 14)) + ":00:00', "
								+ "\r\n\t'" + randBetween(15, 19) + ":00:00', "
								+ "\r\n\t" + randBetween(1,8) + ".50);";
			
			transaction_script = transaction_script + "\r\n\r\n";
		}
		
		writeScript(transaction_path, transaction_script);
	
	}
	
	private static String randomDate(int s, int e)
	{
		GregorianCalendar c = new GregorianCalendar();
		
		int yyyy = randBetween(s, e);
		c.set(c.YEAR, yyyy);
		int day = randBetween(1, c.getActualMaximum(c.DAY_OF_YEAR));
		c.set(c.DAY_OF_YEAR, day);
		
		DateFormat df = new SimpleDateFormat("yyyy-MM-dd");
		
		return df.format(c.getTime());
	}

	private static int randBetween(int lower, int upper)
	{
		return (lower + (int)Math.round(Math.random() * (upper - lower)));
	}
	
	private static void writeScript(String fname, String script) throws IOException
	{
		FileWriter w = new FileWriter(fname, false);
		PrintWriter p = new PrintWriter(w);
		p.print(script);
		p.close();
	}
	
	
	
	
	
	
	
	
	
	
	
	
}
