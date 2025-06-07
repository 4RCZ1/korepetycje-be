using Database.Entities;
using Endpoints.Interfaces;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class StudentDao : IStudentDao
{

    public void AddStudent(DbStudent student)
    {
        using var context = new OurDbContext(_connection);

        try
        {
            context.Students.Add(student);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new ApplicationException("Something went wrong!", e);
        }
    }

    public DbStudent GetStudent(int studentId)
    {
        using var context = new OurDbContext(_connection);
        var student = context.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)?
            .SingleOrDefault();
        if(student is null)
            throw new ApplicationException("No student found");
        return student;

    }

    public void UpdateStudent(int studentId, DbStudent student)
    {
        using var context = new OurDbContext(_connection);
        DbStudent studentToUpdate;
        try
        {
            studentToUpdate = context.Students.Where(s => s.Id == studentId)
                .SingleOrDefault();
        }
        catch (ArgumentNullException ex)
        {
            throw new ApplicationException("Student not found!", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Something went wrong!", ex);
        }
        studentToUpdate.Name = string.IsNullOrEmpty(student.Name) ? studentToUpdate.Name : student.Name;
        studentToUpdate.Surname = string.IsNullOrEmpty(student.Surname) ? studentToUpdate.Surname : student.Surname;
        studentToUpdate.Address = student.Address;
        studentToUpdate.AddressId = student.AddressId;
        context.SaveChanges();
    }

    public void DeleteStudent(int studentId)
    {
        using var context = new OurDbContext(_connection);
        
        var studentToDelete = context.Students.Where(s => s.Id == studentId).SingleOrDefault();
        if (studentToDelete is null)
            throw new ApplicationException("No student found");
        context.Students.Remove(studentToDelete);
        context.SaveChanges();
    }
    public StudentDao(string connection)
    {
        _connection = connection;
    }
    private readonly string _connection;
}