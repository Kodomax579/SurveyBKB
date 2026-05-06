using Contracts.Protos;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Database;

namespace UserService.Feature
{
    public class ClassService (UserContext userContext)
    {
        public async Task<List<ClassMessage>> GetAllClasses()
        {
            try
            {
                var classModelList = await userContext.Classes.ToListAsync();

                var classMessageList = new List<ClassMessage>();

                foreach (var classModel in classModelList)
                {
                    classMessageList.Add(new ClassMessage
                    {
                        Name = classModel.ClassName
                    });
                }
                return classMessageList;
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        public async Task<bool> CreateClass(ClassMessage classMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(classMessage.Name))
                {
                    return false;
                }

                var newClass = new ClassModel()
                {
                    ClassName = classMessage.Name,
                };

                userContext.Classes.Add(newClass);
                await userContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
