using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // this is method is just displaying the fields & returning the view in UI
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // this method is adding the entities to DB - CREATE
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();

            return View();
        }

        // this method is displaying the list of entities in UI from the DB - READ
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await dbContext.Students.ToListAsync();  // we are getting the list back & saving in a variable

            return View(students);

        }

        // this method is reading or finding the student to - EDIT (1)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);

            return View(student);
        }

        // now this method is going to update the students - EDIT (2)
        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student =  await dbContext.Students.FindAsync(viewModel.Id);

            if(student is not  null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                // save the updates into DB
                await dbContext.SaveChangesAsync();
            }

            // redirect back to the list page
            return RedirectToAction("List", "Students");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var student = await dbContext.Students
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            
            if(student is not null)
            {
                dbContext.Students.Remove(viewModel);   // Delete
                await dbContext.SaveChangesAsync();     // save changes
            }

            // redirect back to the list page
            return RedirectToAction("List", "Students");    
        }

    }


}
