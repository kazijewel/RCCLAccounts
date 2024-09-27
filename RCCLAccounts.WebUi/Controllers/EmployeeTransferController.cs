using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Core.Services;
using ProvidentFund.Data;
using ProvidentFund.Data.Entities;
using ProvidentFund.WebUi.Models;
using System;

namespace ProvidentFund.WebUi.Controllers
{
    public class EmployeeTransferController : Controller
    {
        public IEmpService empService { get; set; }
        private AppDbContext _db;
        public EmployeeTransferController(IEmpService _empService, AppDbContext db) 
        {
         this.empService = _empService;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var allEmployees = await empService.GetAllEmployeesAsync();
            return View(allEmployees);
        }

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await empService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = new SelectList(await empService.GetDepartments(), "Id", "Name");
            ViewBag.Designations = new SelectList(await empService.GetDesignations(), "Id", "Name");
            ViewBag.Branches = new SelectList(await empService.GetBranches(), "BranchId", "BranchName");

            ViewBag.Gender = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Male",Value="Male"},
              new SelectListItem() { Text="Female",Value="Female"}
            };
            ViewBag.Gender = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Male",Value="Male"},
              new SelectListItem() { Text="Female",Value="Female"}
            };
            ViewBag.Status = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Active",Value="1"},
              new SelectListItem() { Text="Inactive",Value="0"}
            };
            ViewBag.MaritalStatus = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Married",Value="1"},
              new SelectListItem() { Text="Single",Value="0"},
               new SelectListItem() { Text="Divorced",Value="2"}
            };

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeInfoVM model)
        {


            ViewBag.Departments = new SelectList(await empService.GetDepartments(), "Id", "Name");
            ViewBag.Designations = new SelectList(await empService.GetDesignations(), "Id", "Name");
            ViewBag.Branches = new SelectList(await empService.GetBranches(), "BranchId", "BranchName");

            ViewBag.Gender = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Male",Value="Male"},
              new SelectListItem() { Text="Female",Value="Female"}
            };
            ViewBag.Gender = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Male",Value="Male"},
              new SelectListItem() { Text="Female",Value="Female"}
            };
            ViewBag.Status = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Active",Value="1"},
              new SelectListItem() { Text="Inactive",Value="0"}
            };
            ViewBag.MaritalStatus = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Married",Value="1"},
              new SelectListItem() { Text="Single",Value="0"},
               new SelectListItem() { Text="Divorced",Value="2"}
            };

  
            string pictureFolder = "images/picture/";
            string signatureFolder = "images/signature/";
            string nidFolder = "images/nid/";
            string picturepath = "";
            string signaturepath = "";
            string nidpath = "";
            string rootpath = Directory.GetCurrentDirectory() + "/wwwroot";

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (ModelState.IsValid)
            {
                try
                {

                    var dataToCopy = _db.EmployeeInfos.Where(x => x.EmpolyeeId == model.EmpolyeeId); // Define your specific condition here
                    var dataToInsert = dataToCopy.Select(item => new EmployeeTransferHistory
                    {
                        // Map properties from SourceTable to DestinationTable
                        TranserEmpolyeeId = item.EmpolyeeId,
                        EmployeeNo=item.EmployeeNo,
                        EmployeeName=item.EmployeeName,
                        FatherName= item.FatherName,
                        MotherName = item.MotherName,
                        MobileNo = item.MobileNo,
                        Email= item.Email,
                        MaritalStatus=item.MaritalStatus,
                        SpouseName=item.SpouseName,
                        Nominee=item.Nominee,
                        Relation=item.Relation,
                        Gender=item.Gender,
                        PicturePath=item.PicturePath,
                        SigniturePath=item.SigniturePath,
                        NidPath=item.NidPath,
                        DateOfBirth=item.DateOfBirth,
                        JoiningDate=item.JoiningDate,
                        RetiredMentDate=item.RetiredMentDate,
                        PresentAddress=item.PresentAddress,
                        PermanentAddress =item.PermanentAddress,
                        DepartmentId =item.DepartmentId,
                        BasicSalary =item.BasicSalary,
                        GrossSalary = item.GrossSalary,
                        OwnContPer = item.OwnContPer,
                        CompanyContPer = item.CompanyContPer,
                        BranchId =item.BranchId,
                        EmployeeStatus =item.EmployeeStatus,
                        Cpfstatus =item.Cpfstatus,
                        UserId =item.UserId,
                        UserName =item.UserName,
                        UserIp = item.UserIp,
                        EntryTime = item.EntryTime,
                        CpfStartDate = item.CpfStartDate,
                        DesignationId = item.DesignationId,
                        UdFlag= "Employee Transfer",
                        PostingDate = model.PostingDate,
                        LeaveDate =model.LeaveDate


                   // Map other properties as needed
                     });

                    _db.EmployeeTransferHistory.AddRange(dataToInsert);
                    _db.SaveChanges();

                    picturepath = model.PicturePath;
                    signaturepath = model.SigniturePath;
                    nidpath = model.NidPath;

                    if (model.Image!=null)
                    {
                        if (model.Image.Length > 0)
                        {
                            var file = rootpath + "/" + model.PicturePath;
                            System.IO.File.Delete(file);

                            picturepath = pictureFolder + Guid.NewGuid() +
                               model.Image.FileName;

                            await model.Image.CopyToAsync
                             (new FileStream(Path.Combine(rootpath, picturepath),
                             FileMode.Create));


                        }

                    }

                    if (model.Signature!=null)
                    {
                        if (model.Signature.Length > 0)
                        {
                            var file = rootpath + "/" + model.SigniturePath;
                            System.IO.File.Delete(file);

                            signaturepath = signatureFolder + Guid.NewGuid() +
                               model.Nid.FileName;

                            await model.Signature.CopyToAsync
                             (new FileStream(Path.Combine(rootpath, signaturepath),
                             FileMode.Create));

                        }
                    }


                    if (model.Nid!=null)
                    {
                        if (model.Nid.Length > 0)
                        {
                            var file = rootpath + "/" + model.NidPath;
                            System.IO.File.Delete(file);


                            nidpath = nidFolder + Guid.NewGuid() +
                               model.Nid.FileName;

                            await model.Nid.CopyToAsync
                             (new FileStream(Path.Combine(rootpath, nidpath),
                             FileMode.Create));

                        }
                    }
                   

                    model.SigniturePath = signaturepath;
                    model.PicturePath = picturepath;
                    model.NidPath = nidpath;

                    await empService.UpdateAsync(model);



                }
                catch (DbUpdateConcurrencyException)
                {
                    /*if (!await EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }*/
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


    }
}
