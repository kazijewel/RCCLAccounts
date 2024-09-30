using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCCLAccounts.Data.Entities;

public partial class EmployeeTransferHistory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int EmpolyeeTransferId { get; set; }

    [ForeignKey("employeeInfo")]
    public int TranserEmpolyeeId { get; set; }

    public string? EmployeeNo { get; set; }

    public string? EmployeeName { get; set; }

    public string? FatherName { get; set; }

    public string MotherName { get; set; }

    public string? MobileNo { get; set; }

    public string? Email { get; set; }

    public string? MaritalStatus { get; set; }

    public string? SpouseName { get; set; } 

    public string? Nominee { get; set; }

    public string? Relation { get; set; }

    public string? Gender { get; set; }

    public string? PicturePath { get; set; }

    public string? SigniturePath { get; set; }

    public string? NidPath { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? RetiredMentDate { get; set; }

    public string? PresentAddress { get; set; } 

    public string? PermanentAddress { get; set; } 

    public int? DepartmentId { get; set; }

    public decimal BasicSalary { get; set; }
    public decimal? GrossSalary { get; set; }

    public decimal OwnContPer { get; set; }

    public decimal CompanyContPer { get; set; }

    public int BranchId { get; set; }

    public int EmployeeStatus { get; set; }

    public int Cpfstatus { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserIp { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? CpfStartDate { get; set; }

    public int DesignationId { get; set; }

    public string? UdFlag { get; set; }
    public DateTime? PostingDate { get; set; }
    public DateTime? LeaveDate { get; set; }
    public virtual EmployeeInfo employeeInfo { get; set; }
   
}
