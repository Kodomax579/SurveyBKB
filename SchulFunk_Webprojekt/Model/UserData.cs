namespace SchulFunk_Webprojekt.Model;

public static class UserData
{
    public static List<UserModel> Users { get; } = new()
    {
        new UserModel
        {
            Name = "Lisa",
            Lastname = "Schneider",
            Email = "lisa.schneider@schule.de",
            Password = "test",
            Class = new ClassModel { ClassName = "10A" },
            Group = Groups.Student
        },
        new UserModel
        {
            Name = "Tim",
            Lastname = "Müller",
            Email = "tim.mueller@schule.de",
            Password = "test",
            Class = new ClassModel { ClassName = "10B" },
            Group = Groups.ClassRepresentatives
        },
        new UserModel
        {
            Name = "Sara",
            Lastname = "Yilmaz",
            Email = "sara.yilmaz@schule.de",
            Password = "test",
            Class = new ClassModel { ClassName = "9A" },
            Group = Groups.StudentRepresentatives
        },
        new UserModel
        {
            Name = "Max",
            Lastname = "Mustermann",
            Email = "max.mustermann@schule.de",
            Password = "test",
            Class = new ClassModel { ClassName = "Lehrer" },
            Group = Groups.Teacher
        }
    };
}