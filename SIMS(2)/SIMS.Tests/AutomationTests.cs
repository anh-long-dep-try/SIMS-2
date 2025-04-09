using System;
using System.Threading.Tasks;
using Xunit;
using SIMS_2_.Models;

namespace SIMS_2_.Tests
{
    public class AutomationTests
    {
        [Fact]
        public async Task RegisterNewStudent()
        {
            // Arrange
            var student = new Student
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(2001, 5, 10),
                Grade = "12"
            };

            // Act
            bool registerResult = await RegisterStudent(student);

            // Assert
            Assert.True(registerResult, "Student data should be saved successfully.");
        }

        [Fact]
        public async Task ValidateMandatoryFields()
        {
            // Arrange
            var student = new Student
            {
                FirstName = "", // Missing required field
                LastName = "Smith",
                DateOfBirth = new DateTime(2000, 1, 1),
                Grade = "10"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await RegisterStudent(student));
        }

        [Fact]
        public async Task PreventDuplicateStudentRegistration()
        {
            // Arrange
            var student = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2000, 1, 1),
                Grade = "10"
            };
            await RegisterStudent(student); // First registration

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateEntryException>(async () => await RegisterStudent(student));
        }

        [Fact]
        public async Task AddNewCourse()
        {
            // Arrange
            var course = new Course
            {
                CourseName = "Physics",
                CourseCode = "PHY101",
                Credits = 3
            };

            // Act
            bool addResult = await AddCourse(course);

            // Assert
            Assert.True(addResult, "Course details should be saved successfully.");
        }

        [Fact]
        public async Task AssignStudentToCourseTest()
        {
            // Arrange
            int studentId = 1;
            int courseId = 1;

            // Act
            bool assignResult = await AssignStudentToCourse(studentId, courseId);

            // Assert
            Assert.True(assignResult, "Student should be enrolled successfully.");
        }

        [Fact]
        public async Task LoginWithValidCredentials()
        {
            // Arrange
            string username = "testuser";
            string password = "testpass";

            // Act
            bool loginResult = await LoginUser(username, password);

            // Assert
            Assert.True(loginResult, "User should log in successfully.");
        }

        [Fact]
        public async Task LoginWithInvalidCredentials()
        {
            // Arrange
            string username = "invaliduser";
            string password = "wrongpass";

            // Act
            bool loginResult = await LoginUser(username, password);

            // Assert
            Assert.False(loginResult, "Error should be displayed for invalid credentials.");
        }

        [Fact]
        public async Task DeleteStudentTest()
        {
            // Arrange
            int studentId = 1;

            // Act
            bool deleteResult = await DeleteStudent(studentId);

            // Assert
            Assert.True(deleteResult, "Student data should be removed successfully.");
        }

        [Fact]
        public async Task EditCourse()
        {
            // Arrange
            int courseId = 1;
            var updatedCourse = new CourseUpdateInfo
            {
                CourseName = "Advanced Physics",
                Credits = 4
            };

            // Act
            bool updateResult = await UpdateCourse(courseId, updatedCourse);

            // Assert
            Assert.True(updateResult, "Course details should be updated successfully.");
        }

        [Fact]
        public async Task EditStudentEnrolled()
        {
            // Arrange
            int enrollmentId = 1;
            var updatedEnrollment = new EnrollmentUpdateInfo
            {
                CourseId = 2,
                EnrollmentDate = DateTime.Now
            };

            // Act
            bool updateResult = await UpdateEnrollment(enrollmentId, updatedEnrollment);

            // Assert
            Assert.True(updateResult, "Enrollment details should be updated successfully.");
        }

        // Helper classes (assumed to be part of the system)
        private class Student
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public DateTime DateOfBirth { get; set; }
            public string Grade { get; set; } = string.Empty;
        }

        private class Course
        {
            public string CourseName { get; set; } = string.Empty;
            public string CourseCode { get; set; } = string.Empty;
            public int Credits { get; set; }
        }

        private class CourseUpdateInfo
        {
            public string CourseName { get; set; } = string.Empty;
            public int Credits { get; set; }
        }

        private class EnrollmentUpdateInfo
        {
            public int CourseId { get; set; }
            public DateTime EnrollmentDate { get; set; }
        }

        // Mock implementation of service methods
        private async Task<bool> RegisterStudent(Student student)
        {
            await Task.Delay(100);
            if (string.IsNullOrEmpty(student.FirstName)) throw new ValidationException("FirstName is required.");
            return true;
        }

        private async Task<bool> AddCourse(Course course)
        {
            await Task.Delay(100);
            return true;
        }

        private async Task<bool> AssignStudentToCourse(int studentId, int courseId)
        {
            await Task.Delay(100);
            return true;
        }

        private async Task<bool> LoginUser(string username, string password)
        {
            await Task.Delay(100);
            return username == "testuser" && password == "testpass";
        }

        private async Task<bool> DeleteStudent(int studentId)
        {
            await Task.Delay(100);
            return true;
        }

        private async Task<bool> UpdateCourse(int courseId, CourseUpdateInfo info)
        {
            await Task.Delay(100);
            return true;
        }

        private async Task<bool> UpdateEnrollment(int enrollmentId, EnrollmentUpdateInfo info)
        {
            await Task.Delay(100);
            return true;
        }
    }

    // Custom exceptions for testing
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string message) : base(message) { }
    }
}
