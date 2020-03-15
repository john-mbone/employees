using Lib.csv;
using System;
using System.IO;
using Xunit;

namespace EmployeesUnitTest
{
    public class EmployeeTest
    {

        [Fact]
        public void Test_Employees()
        {
            var lines = File.ReadAllLines("../../../test.txt");
            Employees employees = new Employees(lines);
            Assert.Equal(3300, employees.Budget("Employee1"));
            Assert.Equal(1000, employees.Budget("Employee2"));
            Assert.NotEqual(100, employees.Budget("Employee3"));
        }

        [Fact]
        public void Workers_Dict_Greater_Zero_Returns_True()
        {
            var lines = File.ReadAllLines("../../../test.txt");
            Employees employees = new Employees(lines);
            Assert.NotStrictEqual(0, employees.workers.Count);
        }

        [Fact]
        public void One_Ceo_Returns_True()
        {
            var lines = File.ReadAllLines("../../../test.txt");
            Employees employees = new Employees(lines);
            Assert.StrictEqual(1, employees.Owner);
        }

        [Fact]
        public void When_Employee_Appears_Twice()
        {
            var lines = File.ReadAllLines("../../../test2.txt");
            Employees employees = new Employees(lines);
            Assert.Equal(3800, employees.Budget("Employee1"));
            Assert.Equal(1800, employees.Budget("Employee2"));
            Assert.Equal(500, employees.Budget("Employee3"));

        }

        [Fact]
        public void Skip_Repeated_Value()
        {
            var lines = File.ReadAllLines("../../../test1.txt");
            Employees employees = new Employees(lines);
            Assert.Equal(3300, employees.Budget("Employee1"));
            Assert.Equal(1000, employees.Budget("Employee2"));

        }


        //test invalid salary value for employ
        [Fact]
        public void Test_Non_Numeric_NotInt()
        {
            var lines = File.ReadAllLines("../../../test3.txt");
            Employees employees = new Employees(lines);
            Assert.Equal(3300, employees.Budget("Employee1"));
            Assert.Equal(1300, employees.Budget("Employee2"));
            Assert.Equal(500, employees.Budget("Employee3"));
            Assert.Equal(0, employees.Budget("Employee6"));
        }


        //All test passed
    }
}
