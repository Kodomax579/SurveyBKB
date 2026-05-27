using SchulFunk_Webprojekt.Feature.UserHandling.Model;

namespace SchulFunk_Webprojekt.Model;

public enum Permission
{
    ReadNews,
    VoteInSurveys,

    OpenAdminPanel,

    CreateSurveys,
    ManageOwnSurveys,
    ManageAllSurveys,

    CreateNews,
    ManageOwnNews,
    ManageAllNews,

    EditStudentUsers,
    EditTeacherUsers,
    DeleteUsers,

    RegisterStudentUsers,
    RegisterTeacherUsers
}

public static class PermissionRules
{
    public static bool HasPermission(UserModel user, Permission permission)
    {
        return user.Group switch
        {
            Groups.Student => permission switch
            {
                Permission.ReadNews => true,
                Permission.VoteInSurveys => true,
                _ => false
            },

            Groups.ClassRepresentatives => permission switch
            {
                Permission.ReadNews => true,
                Permission.VoteInSurveys => true,
                Permission.OpenAdminPanel => true,
                Permission.CreateSurveys => true,
                Permission.ManageOwnSurveys => true,
                _ => false
            },

            Groups.StudentRepresentatives => permission switch
            {
                Permission.ReadNews => true,
                Permission.VoteInSurveys => true,
                Permission.OpenAdminPanel => true,
                Permission.CreateSurveys => true,
                Permission.ManageOwnSurveys => true,
                Permission.CreateNews => true,
                Permission.ManageOwnNews => true,
                _ => false
            },

            Groups.Teacher => permission switch
            {
                Permission.ReadNews => true,
                Permission.OpenAdminPanel => true,
                Permission.CreateSurveys => true,
                Permission.ManageOwnSurveys => true,
                Permission.CreateNews => true,
                Permission.ManageOwnNews => true,
                Permission.EditStudentUsers => true,
                Permission.RegisterStudentUsers => true,
                _ => false
            },

            Groups.Admin => permission switch
            {
                Permission.ReadNews => true,
                Permission.OpenAdminPanel => true,
                Permission.CreateSurveys => true,
                Permission.ManageOwnSurveys => true,
                Permission.ManageAllSurveys => true,
                Permission.CreateNews => true,
                Permission.ManageOwnNews => true,
                Permission.ManageAllNews => true,
                Permission.EditStudentUsers => true,
                Permission.EditTeacherUsers => true,
                Permission.DeleteUsers => true,
                Permission.RegisterStudentUsers => true,
                Permission.RegisterTeacherUsers => true,
                _ => false
            },

            _ => false
        };
    }

    public static string GetGroupLabel(Groups group)
    {
        return group switch
        {
            Groups.Student => "Schüler",
            Groups.ClassRepresentatives => "Klassensprecher",
            Groups.StudentRepresentatives => "Schülersprecher",
            Groups.Teacher => "Lehrer",
            Groups.Admin => "Admin",
            _ => "Unbekannt"
        };
    }
}