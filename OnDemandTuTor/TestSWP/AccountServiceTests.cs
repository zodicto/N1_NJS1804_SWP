using Moq;
using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ODTLearning.Models;
using ODTLearning.Entities;
using ODTLearning.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using ODTLearning.Helpers;
using System.Text;

namespace YourProject.Tests.UnitTests
{
    public class AccountServiceTests
    {
        [Fact] // Đăng ký với những thông tin hợp lệ 
        public void SignUp_ValidInformation_Success()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var configurationMock = new Mock<IConfiguration>();
            var signUpModel = new SignUpModelOfAccount
            {
                FullName = "Test User",
                Password = "password",
                Phone = "123456789",
                Email = "test@example.com",
                date_of_birth = DateOnly.FromDateTime(DateTime.Now),
                Gender = "Male"
            };

            var mockDbSet = new Mock<DbSet<Account>>();
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var service = new AccountRepository(contextMock.Object, configurationMock.Object, null);

            // Act
            var result = service.SignUpOfAccount(signUpModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(signUpModel.FullName, result.FullName);
            Assert.Equal(signUpModel.Email, result.Email);
            Assert.Equal(signUpModel.date_of_birth, (result.Date_of_birth));
            Assert.Equal(signUpModel.Gender, result.Gender);

            contextMock.Verify(x => x.Accounts.Add(It.IsAny<Account>()), Times.Once);
            contextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]// kiểm tra trường hợp email đã tồn tại 
        public void SignUp_EmailAlreadyExists_Failure()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var configurationMock = new Mock<IConfiguration>();
            var signUpModel = new SignUpModelOfAccount
            {
                FullName = "Test User",
                Password = "password",
                Phone = "123456789",
                Email = "test@example.com",
                date_of_birth = DateOnly.FromDateTime(DateTime.Now),
                Gender = "Male"
            };

            var existingUser = new Account
            {
                Email = "test@example.com"
            };

            var data = new List<Account> { existingUser }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var service = new AccountRepository(contextMock.Object, configurationMock.Object, null);

            // Act
            var emailExists = service.IsEmailExist(signUpModel.Email);
            UserResponse result = null;
            if (!emailExists)
            {
                result = service.SignUpOfAccount(signUpModel);
            }

            // Assert
            Assert.True(emailExists);
            Assert.Null(result);
        }



        [Fact]// đăng nhập với trường email và mật khấu hợp lệ 
        public void SignInValidationOfAccount_ValidInformation_Success()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var signInModel = new SignInModel
            {
                Email = "test@example.com",
                Password = "password"
            };

            var account = new Account
            {
                Id = "valid-account-id",
                FullName = "Test User",
                Email = "test@example.com",
                Password = "password", // In a real scenario, this should be hashed
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                Gender = "Male",
                Roles = "học sinh",
                Avatar = "avatar.png",
                Address = "123 Main St",
                Phone = "123456789",
                AccountBalance = 100
            };

            var data = new List<Account> { account }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var service = new AccountRepository(contextMock.Object, null, null);

            // Act
            var result = service.SignInValidationOfAccount(signInModel);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Đăng nhập thành công", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(account.Id, result.Data.Id);
        }


        [Fact]// đăng nhập với eail ko tồn tại 
        public void SignInValidationOfAccount_NonExistentEmail_Failure()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var signInModel = new SignInModel
            {
                Email = "nonexistent@example.com",
                Password = "password"
            };

            var data = new List<Account>().AsQueryable(); // No accounts exist
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var service = new AccountRepository(contextMock.Object, null, null);

            // Act
            var result = service.SignInValidationOfAccount(signInModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email hoặc password không đúng", result.Message);
            Assert.Null(result.Data);
        }


        [Fact] // đăng nhập voiw ml không đúng 
        public void SignInValidationOfAccount_InvalidPassword_Failure()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var signInModel = new SignInModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var account = new Account
            {
                Id = "valid-account-id",
                FullName = "Test User",
                Email = "test@example.com",
                Password = "password", // In a real scenario, this should be hashed
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                Gender = "Male",
                Roles = "học sinh",
                Avatar = "avatar.png",
                Address = "123 Main St",
                Phone = "123456789",
                AccountBalance = 100
            };

            var data = new List<Account> { account }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var service = new AccountRepository(contextMock.Object, null, null);

            // Act
            var result = service.SignInValidationOfAccount(signInModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email hoặc password không đúng", result.Message);
            Assert.Null(result.Data);
        }


        [Fact]// trường hợp đăng ký gia sư happy casse
        public void SignUpOfTutor_ValidInformation_Success()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var configurationMock = new Mock<IConfiguration>();
            var imgLibMock = new Mock<IImageLibrary>();
            var signUpModel = new SignUpModelOfTutor
            {
                SpecializedSkills = "Mathematics",
                Experience = 5,
                QualificationName = "PhD in Mathematics",
                Type = "Degree",
                ImageQualification = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy file")), 0, 0, "Data", "dummy.jpg"),
                Subject = "Mathematics"
            };

            var existingUser = new Account
            {
                Id = "valid-account-id"
            };

            var data = new List<Account> { existingUser }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var subject = new Subject
            {
                Id = "valid-subject-id",
                SubjectName = "Mathematics"
            };

            var subjectData = new List<Subject> { subject }.AsQueryable();
            var subjectDbSet = new Mock<DbSet<Subject>>();
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.Provider).Returns(subjectData.Provider);
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.Expression).Returns(subjectData.Expression);
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.ElementType).Returns(subjectData.ElementType);
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.GetEnumerator()).Returns(subjectData.GetEnumerator());
            contextMock.Setup(c => c.Subjects).Returns(subjectDbSet.Object);

            var tutorData = new List<Tutor>().AsQueryable();
            var tutorDbSet = new Mock<DbSet<Tutor>>();
            tutorDbSet.As<IQueryable<Tutor>>().Setup(m => m.Provider).Returns(tutorData.Provider);
            tutorDbSet.As<IQueryable<Tutor>>().Setup(m => m.Expression).Returns(tutorData.Expression);
            tutorDbSet.As<IQueryable<Tutor>>().Setup(m => m.ElementType).Returns(tutorData.ElementType);
            tutorDbSet.As<IQueryable<Tutor>>().Setup(m => m.GetEnumerator()).Returns(tutorData.GetEnumerator());
            contextMock.Setup(c => c.Tutors).Returns(tutorDbSet.Object);

            var educationalQualificationData = new List<EducationalQualification>().AsQueryable();
            var educationalQualificationDbSet = new Mock<DbSet<EducationalQualification>>();
            educationalQualificationDbSet.As<IQueryable<EducationalQualification>>().Setup(m => m.Provider).Returns(educationalQualificationData.Provider);
            educationalQualificationDbSet.As<IQueryable<EducationalQualification>>().Setup(m => m.Expression).Returns(educationalQualificationData.Expression);
            educationalQualificationDbSet.As<IQueryable<EducationalQualification>>().Setup(m => m.ElementType).Returns(educationalQualificationData.ElementType);
            educationalQualificationDbSet.As<IQueryable<EducationalQualification>>().Setup(m => m.GetEnumerator()).Returns(educationalQualificationData.GetEnumerator());
            contextMock.Setup(c => c.EducationalQualifications).Returns(educationalQualificationDbSet.Object);

            var tutorSubjectData = new List<TutorSubject>().AsQueryable();
            var tutorSubjectDbSet = new Mock<DbSet<TutorSubject>>();
            tutorSubjectDbSet.As<IQueryable<TutorSubject>>().Setup(m => m.Provider).Returns(tutorSubjectData.Provider);
            tutorSubjectDbSet.As<IQueryable<TutorSubject>>().Setup(m => m.Expression).Returns(tutorSubjectData.Expression);
            tutorSubjectDbSet.As<IQueryable<TutorSubject>>().Setup(m => m.ElementType).Returns(tutorSubjectData.ElementType);
            tutorSubjectDbSet.As<IQueryable<TutorSubject>>().Setup(m => m.GetEnumerator()).Returns(tutorSubjectData.GetEnumerator());
            contextMock.Setup(c => c.TutorSubjects).Returns(tutorSubjectDbSet.Object);

            imgLibMock.Setup(l => l.UploadImage(It.IsAny<IFormFile>())).ReturnsAsync(true);

            var service = new AccountRepository(contextMock.Object, configurationMock.Object, imgLibMock.Object);

            // Act
            var result = service.SignUpOftutor(existingUser.Id, signUpModel);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Đăng ký gia sư thành công. Bạn vui lòng chờ duyệt", result.Message);
        }

        [Fact]
        public void SignUpOfTutor_NonExistentAccountId_Failure()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var configurationMock = new Mock<IConfiguration>();
            var imgLibMock = new Mock<IImageLibrary>();
            var signUpModel = new SignUpModelOfTutor
            {
                SpecializedSkills = "Mathematics",
                Experience = 5,
                QualificationName = "PhD in Mathematics",
                Type = "Degree",
                ImageQualification = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy file")), 0, 0, "Data", "dummy.jpg"),
                Subject = "Mathematics"
            };

            var data = new List<Account>().AsQueryable(); // Không có tài khoản nào tồn tại
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var service = new AccountRepository(contextMock.Object, configurationMock.Object, imgLibMock.Object);

            // Act
            var result = service.SignUpOftutor("non-existent-id", signUpModel);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Không tìm thấy tài khoản với ID này", result.Message);
        }

        [Fact]
        public void SignUpOfTutor_NonExistentSubject_Failure()
        {
            // Arrange
            var contextMock = new Mock<DbminiCapstoneContext>();
            var configurationMock = new Mock<IConfiguration>();
            var imgLibMock = new Mock<IImageLibrary>();
            var signUpModel = new SignUpModelOfTutor
            {
                SpecializedSkills = "Mathematics",
                Experience = 5,
                QualificationName = "PhD in Mathematics",
                Type = "Degree",
                ImageQualification = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy file")), 0, 0, "Data", "dummy.jpg"),
                Subject = "NonExistentSubject"
            };

            var existingUser = new Account
            {
                Id = "valid-account-id"
            };

            var data = new List<Account> { existingUser }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            contextMock.Setup(c => c.Accounts).Returns(mockDbSet.Object);

            var subjectData = new List<Subject>().AsQueryable(); // Không có môn học nào tồn tại
            var subjectDbSet = new Mock<DbSet<Subject>>();
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.Provider).Returns(subjectData.Provider);
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.Expression).Returns(subjectData.Expression);
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.ElementType).Returns(subjectData.ElementType);
            subjectDbSet.As<IQueryable<Subject>>().Setup(m => m.GetEnumerator()).Returns(subjectData.GetEnumerator());
            contextMock.Setup(c => c.Subjects).Returns(subjectDbSet.Object);

            var service = new AccountRepository(contextMock.Object, configurationMock.Object, imgLibMock.Object);

            // Act
            var result = service.SignUpOftutor(existingUser.Id, signUpModel);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Không tìm thấy môn học nào với tên này", result.Message);
        }


    }
}
