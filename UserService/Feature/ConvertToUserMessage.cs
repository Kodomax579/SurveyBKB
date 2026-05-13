using Contracts.Protos;
using UserService.Data;

namespace UserService.Feature
{
    public static class ConvertService
    {
        public static UserMessage ConvertUserModelToUserMessage(UserModel userModel)
        {
            return new UserMessage
            {
                Name = userModel.Name,
                Lastname = userModel.Lastname,
                Email = userModel.Email,
                Group = (UserGroup)userModel.GroupId,
                Class = new ClassMessage
                {
                    Name = userModel.Class.ClassName
                },
                Password = userModel.PasswordHash,
            };
        }

        public static LoginMessageResponse ConvertUserModelToLoginMessageResponse(UserModel userModel)
        {
            return new LoginMessageResponse
            {
                Name = userModel.Name,
                Lastname = userModel.Lastname,
                Email = userModel.Email,
                Group = (UserGroup)userModel.GroupId,
                Class = new ClassMessage
                {
                    Name = userModel.Class.ClassName
                },
            };
        }

        public static UserModel ConvertUserModelToUserMessage(UserMessage userMessage)
        {
            return new UserModel
            {
                Name = userMessage.Name,
                Lastname = userMessage.Lastname,
                Email = userMessage.Email,
                GroupId = (int)userMessage.Group,
                PasswordHash = userMessage.Password,
                Class = new ClassModel
                {
                    ClassName = userMessage.Class.Name,
                }
            };
        }
    }
}
