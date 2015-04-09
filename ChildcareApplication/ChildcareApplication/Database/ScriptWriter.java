import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Random;


public class ScriptWriter {

	public static void main(String[] args) throws IOException {
		FileWriter fw = new FileWriter(new File("sqlcreator.sql"));
		int gName = 1;
		int cName = 1;
		String allowID = "100000";
		String transID = "1000000000";
		
		for(int guardianID = 123460, guardian2ID = 123461; guardianID < 134460; guardianID += 10, guardian2ID += 10) {
			String guardianSQL = "INSERT INTO Guardian VALUES ('";
			String guardian2SQL = "INSERT INTO Guardian VALUES ('";
			String childSQL = "INSERT INTO Child VALUES ('";
			String allowedConSQL = "INSERT INTO AllowedConnections VALUES ('";
			String familySQL = "INSERT INTO Family VALUES ('";
			String childTransSQL = "INSERT INTO ChildcareTransaction VALUES ('";
			
			familySQL += (guardianID / 10) + "', 42);";
			
			guardianSQL += guardianID + "', '81DC9BDB52D04DC20036DBD8313ED0', 'Guardian" + gName + "', '" + gName + "guardian', ";
			guardianSQL += "'555-555-5555', 'blah@blah.com', '123 nowhere', null, 'Spokane', 'WA', '99999', '../../Pictures/default.jpg', null);";
			
			guardian2SQL += guardian2ID + "', '81DC9BDB52D04DC20036DBD8313ED0', 'Guardian2" + gName + "', '" + gName + "2guardian', ";
			guardian2SQL += "'555-555-5555', 'blah@blah.com', '123 nowhere', null, 'Spokane', 'WA', '99999', '../../Pictures/default.jpg', null);";
			gName++;
			
			for(int childID = guardianID + 2; childID < guardianID + 6; childID++, cName++) {
				childSQL += childID + "', 'Child" + cName + "', '" + cName + "child', '2006-02-02', null, null, '../../Pictures/default.jpg', null);";
				allowedConSQL += allowID + "', '" + guardianID + "', '" + childID + "', " + (guardianID / 10) + ", null);";
				childTransSQL += transID + "', 'Regular Childcare', '" + allowID + "', '" + randDate() + "', '12:00:00', '14:00:00', 20.50);";
				allowID = "" + (Integer.parseInt(allowID) + 1);
				transID = "" + (Integer.parseInt(transID) + 1);
				
				fw.write(allowedConSQL + "\r\n\r\n");
				fw.write(childSQL + "\r\n\r\n");
				fw.write(childTransSQL + "\r\n\r\n");
				
				allowedConSQL = "INSERT INTO AllowedConnections VALUES ('";
				childSQL = "INSERT INTO Child VALUES ('";
				childTransSQL = "INSERT INTO ChildcareTransaction VALUES ('";
			}
			
			
			fw.write(guardianSQL + "\r\n\r\n");
			
			
			fw.write(familySQL + "\r\n\r\n");
			fw.write(guardian2SQL + "\r\n\r\n");
			
		}
		fw.close();
	}
	
	private static String randDate() {
		Random gen = new Random();
		
		int day = gen.nextInt(28) + 1;
		int month = gen.nextInt(12) + 1;
		int year = gen.nextInt(1) + 2014;
		
		return "" + year + "-" + String.format("%02d", month) + "-" + String.format("%02d", day);
	}
}
