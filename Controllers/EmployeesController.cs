using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_application_dotnet.data;
using test_application_dotnet.Models;
using test_application_dotnet.Models.Domain;

namespace test_application_dotnet.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDbContext mvcDbContext;
        public EmployeesController(MVCDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employees()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateofBirth = addEmployeeRequest.DateofBirth,
            };

            await mvcDbContext.Employees.AddAsync(employee);
            await mvcDbContext.SaveChangesAsync();
            return RedirectToAction("AddEmployee");

        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(Guid Id)
        {
            var employees = await mvcDbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);

            if (employees != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = Guid.NewGuid(),
                    Name = employees.Name,
                    Email = employees.Email,
                    Salary = employees.Salary,
                    Department = employees.Department,
                    DateofBirth = employees.DateofBirth,
                };
                return await Task.Run(() => View("UpdateEmployee", viewModel));
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateofBirth = model.DateofBirth;
                employee.Department = model.Department;

                await mvcDbContext.SaveChangesAsync();

                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDbContext.Employees.FindAsync(model.Id);

            if(employee != null) 
            { 
                mvcDbContext.Employees.Remove(employee);
                await mvcDbContext.SaveChangesAsync();

                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }
    }
}
