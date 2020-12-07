using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesHierarchy
{
    public class Employees
    {
        public ArrayList employeeList;
        public Dictionary<string, ArrayList> EmployeeDict;

        public Employees(string filePath)
        {
            employeeList = ProcessCsvFile(filePath);
            ValidSalary(employeeList);
            OneCeo(employeeList);
            ReportTo(employeeList);
            CircuralRefference();
        }

        //CVS processing method that returns a well formated Array from the give CCS file
        public ArrayList ProcessCsvFile(string filePath)
        {
            ArrayList employeesList = new ArrayList();
            try
            {
                //check if the CSV FILE path exists in the give directory
                if (File.Exists(filePath))
                {                   
                    string[] csv = File.ReadAllLines(filePath);

                    //check if the CSV FILE is empty
                    if (csv.Length != 0)
                    {
                        for (int c = 0; c <= csv.Length - 1; c++)
                        {
                            string[] fields = csv[c].Split(',');

                            ArrayList List = new ArrayList();
                            foreach (var column in fields)
                            {
                                List.Add(column);
                            }

                            //verify that the file has the required columns
                            if (List.Count != 3)
                            {
                                Console.WriteLine("CSV file value must have 3 columns in each line. At row " + c.ToString());
                            }
                            employeesList.Add(List);
                            Console.WriteLine(fields[0] + " " + fields[1] + " " + fields[2] +"\n");
                        }
                        return employeesList; 
                    }
                    else
                    {
                        Console.WriteLine("CSV file is empty.");
                        return employeesList;
                    }
                }
                else
                {
                    Console.WriteLine("CSV file path does not exist.");
                    return employeesList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
                return employeesList;
            }
        }

        //verify salary in a valid integer or non-negative integer
        public bool ValidSalary(ArrayList list)
        {
            bool swicth = false;
            try
            {
                foreach (ArrayList field in list)
                {
                    int salary;
                    if (!int.TryParse(field[2].ToString(), out salary) || int.Parse(field[2].ToString()) < 0)
                    {
                        Console.WriteLine("The following row is having invalid salary value " + field[2]);
                        swicth = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
            }
           
            return swicth;
        }

        //verify there is only  one CEO
        public bool OneCeo(ArrayList list)
        {
            int Ceo = 0;
            bool swicth = false;
            try
            {
                foreach (ArrayList field in list)
                {
                    if (string.IsNullOrEmpty(field[1].ToString()) && Ceo < 1)
                    {
                        Ceo++;
                    }
                    else if (string.IsNullOrEmpty(field[1].ToString()) && Ceo == 1)
                    {
                        Console.WriteLine("The following file has more than one CEO.");
                        swicth = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
            }
               
            return swicth;
        }

        //counter check employees heirarchy
        public bool ReportTo(ArrayList list)
        {
            bool swicth = false;

            try
            {
                EmployeeDict = new Dictionary<string, ArrayList>();

                foreach (ArrayList field in list)
                {
                    //verify employee id is not null/verifies if all managers are employees
                    if (string.IsNullOrEmpty(field[0].ToString()))
                    {
                        Console.WriteLine("This row is missing Empolyee Id");
                        swicth = true;
                    }
                    else
                    {
                        //using Dictionary Add method verifies employee id is not repeated using employee Id as Key
                        //to avoid one employee reporting to more than one manager
                        EmployeeDict.Add( field[0].ToString(), field);
                    }
                }
            }
            catch (Exception  ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
                swicth =true;
            }
            
            return swicth;
        }

        //counter check employees circural refference
        public bool CircuralRefference()
        {
            bool swicth = false;

            try
            {
                foreach (KeyValuePair<string, ArrayList> keyValue in EmployeeDict)
                {
                    Console.WriteLine(keyValue.Key);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
            }
            
            return swicth;
        }
        
        //instance method that returns the salary budget from the specified manager
        public long SalaryBudget(string managerName)
        {
            long totalSalary = 0;
            try
            {
                foreach (ArrayList field in employeeList)
                {
                    if (field[0].ToString().Trim().ToLowerInvariant() == (managerName.Trim().ToLowerInvariant())
                        || field[1].ToString().Trim().ToLowerInvariant() == (managerName.Trim().ToLowerInvariant()))
                    {
                        totalSalary += Convert.ToInt32(field[2]);
                    }
                }
                Console.WriteLine("Budget SubTotal : " + totalSalary);

                
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
            }
            
            return totalSalary;
        }
    }
}
