namespace Packt.Shared;

public class Employee : Person
{
    public string? EmployeeCode { get; set; }
    public DateOnly HireDate { get; set; }

    public new void WriteToConsole()
    {
        WriteLine("{0} was born on {1:dd/MM/yy} and hired on {2:dd/MM/yy}.", Name, Born, HireDate);
    }

    public override string ToString()
    {
        return $"{Name}'s code is {EmployeeCode}.";
    }
}
