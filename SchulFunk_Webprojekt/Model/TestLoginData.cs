using SchulFunk_Webprojekt.Feature.UserHandling;
using SchulFunk_Webprojekt.Feature.UserHandling.Model;

namespace SchulFunk_Webprojekt.Model;

public static class TestLoginData
{
    public static TestUserRole CurrentRole { get; set; } = TestUserRole.Admin;

    public static int CurrentUserId => CurrentRole switch
    {
        TestUserRole.Student => 4,
        TestUserRole.ClassRepresentative => 3,
        TestUserRole.StudentRepresentative => 2,
        TestUserRole.Teacher => 1,
        TestUserRole.Admin => 99,
        _ => 4
    };

    public static UserModel CurrentUser => CurrentRole switch
    {
        TestUserRole.Student => new UserModel
        {
            Firstname = "Lina",
            Lastname = "Schülerin",
            Email = "lina.schuelerin@schule.de",
            Class = new ClassModel { Classname = "10A" },
            Group = Groups.Student
        },

        TestUserRole.ClassRepresentative => new UserModel
        {
            Firstname = "Tom",
            Lastname = "Klassensprecher",
            Email = "tom.klassensprecher@schule.de",
            Class = new ClassModel { Classname = "10A" },
            Group = Groups.ClassRepresentatives
        },

        TestUserRole.StudentRepresentative => new UserModel
        {
            Firstname = "Mia",
            Lastname = "Schülersprecherin",
            Email = "mia.schuelersprecherin@schule.de",
            Class = new ClassModel { Classname = "SV" },
            Group = Groups.StudentRepresentatives
        },

        TestUserRole.Teacher => new UserModel
        {
            Firstname = "Max",
            Lastname = "Mustermann",
            Email = "max.mustermann@schule.de",
            Class = new ClassModel { Classname = "Lehrer" },
            Group = Groups.Teacher
        },

        TestUserRole.Admin => new UserModel
        {
            Firstname = "Ada",
            Lastname = "Admin",
            Email = "admin@schule.de",
            Class = new ClassModel { Classname = "Verwaltung" },
            Group = Groups.Admin
        },

        _ => new UserModel
        {
            Firstname = "Lina",
            Lastname = "Schülerin",
            Email = "lina.schuelerin@schule.de",
            Class = new ClassModel { Classname = "10A" },
            Group = Groups.Student
        }
    };
}

public enum TestUserRole
{
    Student,
    ClassRepresentative,
    StudentRepresentative,
    Teacher,
    Admin
}