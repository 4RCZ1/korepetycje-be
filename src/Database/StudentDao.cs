using Database.Entities;
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
            .SingleOrDefault(s => s.Id == studentId);
        if(student is null)
            throw new ApplicationException("No student found");
        return student;

    }

    public void UpdateStudent(DbStudent student)
    {
        using var context = new OurDbContext(_connection);
        DbStudent? studentToUpdate;
        studentToUpdate = context.Students
            .SingleOrDefault(s => s.Id == student.Id);
        if(studentToUpdate is null)
            throw new ApplicationException("No student found");
        studentToUpdate = student;
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