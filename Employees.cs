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

        public Employees(string filePath)
        {
            employeeList = ProcessCsvFile(filePath);
            ValidSalary(employeeList);
            OneCeo(employeeList);
            ReportTo(employeeList);
            CircuralRefference(employeeList);
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
                            Console.WriteLine(fields[0] + " " + fields[1] + " " + fields[2] + "\n");
                            Console.WriteLine("...................................................");
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
                Dictionary<string, ArrayList> EmployeeDict = new Dictionary<string, ArrayList>();

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
                        //using Dictionary ContainsKey method verifies employee id is not repeated using employee Id as Key
                        //to avoid one employee reporting to more than one manager
                        if (!EmployeeDict.ContainsKey(field[0].ToString()))
                        {
                            EmployeeDict.Add(field[0].ToString(), field);
                        }
                        else if (EmployeeDict.ContainsKey(field[0].ToString()))
                        {
                            Console.WriteLine("EmployeeId can't be duplicated as it may led, an employee reporting to more than one manager.");
                            swicth = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured : " + ex.Message);
            }

            return swicth;
        }

        //object employee
        public class Employee
        {
            public string EmployeeId { get; set; }
            public string ManagerId { get; set; }
            public long Salary { get; set; }
        }

        //counter check employees circural refference
        public bool CircuralRefference(ArrayList list)
        {
            bool swicth = false;

            try
            {
                ArrayList ceo = new ArrayList();
                ArrayList managers = new ArrayList();
                ArrayList staffs = new ArrayList();
                ArrayList employees = new ArrayList();

                Dictionary<int,List<Employee>> employeeDict = new Dictionary<int,List<Employee>>();

                List<Employee> emp = new List<Employee>();

                Console.WriteLine("####################################   CEO    ");

                foreach (ArrayList field in list)
                {
                    //Console.WriteLine(field[0]);
                    employees.Add(field[0].ToString().Trim());

                    //verify employee id is not null
                    if (string.IsNullOrEmpty(field[0].ToString()))
                    {
                        Console.WriteLine("This row is missing Empolyee Id");
                        swicth = true;
                    }
                    else
                    {
                        //ceo check
                        if (string.IsNullOrEmpty(field[1].ToString().Trim()))
                        {
                            Console.WriteLine(field[0]);
                            ceo.Add(field[0]);
                            Console.WriteLine("************************************ MANAGERS ");
                        }
                        //managers check
                        else if (!ceo.Contains(field[1].ToString().Trim()))
                        {
                            Console.WriteLine(field[1]);
                            managers.Add(field[1]);
                        }
                    }

                    //add every employee to list of employees
                    emp.Add(new Employee 
                     { 
                        EmployeeId= field[0].ToString(), 
                        ManagerId = field[1].ToString(), 
                        Salary = int.Parse(field[2].ToString())
                     });
                }

                Console.WriteLine("********************************* NON-MANAGERS ");

                for (int c = 0; c < list.Count; c++)
                {
                    //non-managers check
                    if (!ceo.Contains(employees[c]) && !managers.Contains(employees[c]))
                    {
                        Console.WriteLine(employees[c]);
                        staffs.Add(employees[c]);
                    }

                    //add a list of employees to employee dictionary
                    employeeDict.Add(c, emp);
                }

                Console.WriteLine("************************** EMPLOYEES DICTIONARY ");

                foreach (KeyValuePair<int, List<Employee>> employee in employeeDict)
                {
                    Console.WriteLine(employee.Key + "  " + employee.Value[employee.Key].EmployeeId + "  " + employee.Value[employee.Key].ManagerId + "  " + employee.Value[employee.Key].Salary);

                    if (employee.Value[employee.Key].ManagerId.Contains(ceo[0].ToString()) && staffs.Contains(employee.Value[employee.Key].EmployeeId))
                    {
                        //Console.WriteLine(employee.Value[employee.Key].EmployeeId + "  " + employee.Value[employee.Key].ManagerId);
                        //Console.WriteLine("circural refference found");
                        swicth = true;
                    }

                    if (employee.Value[employee.Key].ManagerId.Contains(managers[0].ToString()) && staffs.Contains(employee.Value[employee.Key].EmployeeId))
                    {

                        //Console.WriteLine(employee.Value[employee.Key].EmployeeId + "  " + employee.Value[employee.Key].ManagerId);
                        //Console.WriteLine("circural refference found");
                        swicth = true;
                    }
                }
               
                Console.WriteLine("**************************************************");
            }
            catch (Exception ex)
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
