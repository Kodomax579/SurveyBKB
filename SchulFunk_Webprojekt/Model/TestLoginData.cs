namespace SchulFunk_Webprojekt.Model;

public static class TestLoginData
{
    public static TestUserRole CurrentRole { get; set; } = TestUserRole.StudentRepresentative;

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
            Name = "Lina",
            Lastname = "Schülerin",
            Email = "lina.schuelerin@schule.de",
            Class = new ClassModel { ClassName = "10A" },
            Group = Groups.Student
        },

        TestUserRole.ClassRepresentative => new UserModel
        {
            Name = "Tom",
            Lastname = "Klassensprecher",
            Email = "tom.klassensprecher@schule.de",
            Class = new ClassModel { ClassName = "10A" },
            Group = Groups.ClassRepresentatives
        },

        TestUserRole.StudentRepresentative => new UserModel
        {
            Name = "Mia",
            Lastname = "Schülersprecherin",
            Email = "mia.schuelersprecherin@schule.de",
            Class = new ClassModel { ClassName = "SV" },
            Group = Groups.StudentRepresentatives
        },

        TestUserRole.Teacher => new UserModel
        {
            Name = "Max",
            Lastname = "Mustermann",
            Email = "max.mustermann@schule.de",
            Class = new ClassModel { ClassName = "Lehrer" },
            Group = Groups.Teacher
        },

        TestUserRole.Admin => new UserModel
        {
            Name = "Ada",
            Lastname = "Admin",
            Email = "admin@schule.de",
            Class = new ClassModel { ClassName = "Verwaltung" },
            Group = Groups.Admin
        },

        _ => new UserModel
        {
            Name = "Lina",
            Lastname = "Schülerin",
            Email = "lina.schuelerin@schule.de",
            Class = new ClassModel { ClassName = "10A" },
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