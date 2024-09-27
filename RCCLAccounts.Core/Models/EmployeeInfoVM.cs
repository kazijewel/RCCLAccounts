using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Models
{
    public class EmployeeInfoVM
    {

    public int? EmpolyeeId { get; set; }

      //[Required(ErrorMessage = "Employee No Is Required:")]
      [Display(Name = "Employee No:")]
     public string? EmployeeNo { get; set; }

     [Required(ErrorMessage = "Employee Name Is Required:")]
     [Display(Name ="Employee Name:")]
     public string EmployeeName { get; set; } 

     
     [Display(Name ="Father Name:")]
     //[Required(ErrorMessage = "Father Name Is Required:")]
     public string? FatherName { get; set; }

     
     [Display(Name ="Mother Name :")]
     [Required(ErrorMessage = "Mother Name Is Required:")]
     public string MotherName { get; set; }

     //[Required(ErrorMessage = "Mobile Number Is Required:")]
     [Display(Name = "Mobile Number :")]
     public string? MobileNo { get; set; }

     [Display(Name = "Email :")]
     public string ? Email { get; set; }

     [Display(Name = "MaritalStatus :")]
     public string? MaritalStatus { get; set; }

     [Required(ErrorMessage = "Spouse Name Is Required if not married use N/A:")]
    [Display(Name = "Spouse :")]
    public string SpouseName { get; set; } = null!;

     [Required(ErrorMessage = "Nominee Is Required:")]
     [Display(Name = "Nominee :")]
     public string Nominee { get; set; }

        [Display(Name = "Relation :")]
       [Required(ErrorMessage = "Relation Is Required:")]
        public string Relation { get; set; }

    // [Required(ErrorMessage = "Gender Is Required:")]
     [Display(Name ="Gender :")]
     public string?  Gender { get; set; }

    public string? PicturePath { get; set; }

    public string? SigniturePath { get; set; }
    public string? NidPath { get; set; }

     [Display(Name ="Date Of Birth :")]
     [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Joining Date :")]
     public DateTime JoiningDate { get; set; }

     [DataType(DataType.Date)]
     [Display(Name = "Retiredment Date :")]
     public DateTime RetiredMentDate { get; set; }

     [Required(ErrorMessage = "PresentAddress Is Required:")]
     [Display(Name = "Present Address :")]
     public string  PresentAddress { get; set; } = null!;

     [Required(ErrorMessage = "PermanentAddress Is Required:")]
     [Display(Name = "Permanent Address :")]
     public string PermanentAddress { get; set; } = null!;


      [Required]
      [Display(Name = "Department :")]
      public int  DepartmentId { get; set; }
     
      public string? DepartmentName { get; set; }

      
        [Required]
        [Display(Name = "Basic Salary :")]
        public decimal BasicSalary { get; set; }
       
        [Display(Name = "Gross Salary :")]
        public decimal? GrossSalary { get; set; }

        [Required]
        [Display(Name = "Own Contribution :")]
        public decimal OwnContPer { get; set; }

        [Required]
        [Display(Name = "Company Contribution :")]
        public decimal CompanyContPer { get; set; }

        [Required]
        [Display(Name ="Branch Name")]
        public int BranchId { get; set; }

        public string? BranchName { get; set; }

        [Required]
        [Display(Name = "IsActive:")]
        public int  EmployeeStatus { get; set; }

        [Required]
        [Display(Name = "CPF Status:")]
        public int Cpfstatus { get; set; }

      
        [Display(Name = "CPF Start Date:")]
        [DataType(DataType.Date)]
        public DateTime CpfStartDate { get; set; }

        public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserIp { get; set; }

    public DateTime? EntryTime { get; set; }

       [Required (ErrorMessage ="Designation Is Required")]
        [Display(Name = "Designation:")]
        public int DesignationId { get; set; }

     public string? DesignationName { get; set; }

        [Display(Name = "Upload Employee Image:")]
        public IFormFile ? Image { get; set; }
        
        [Display(Name = "Upload Employee Signature:")]
        public IFormFile? Signature { get; set; }

        [Display(Name = "Upload Nid:")]
        public IFormFile? Nid { get; set; }

        public string? NID { get; set; }

        [NotMapped]
        public DateTime? PostingDate { get; set; }
        [NotMapped]
        public DateTime? LeaveDate { get; set; }



    }
}
