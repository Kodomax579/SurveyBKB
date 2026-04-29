using Contracts.Protos;
using Grpc.Core;

namespace UserService.Service
{
    public class UserGrpcService : User.UserBase
    {
        public override Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
        {
            var response = new GetAllUsersResponse();

            // (Zum Testen einfach mal einen Dummy-User hinzufügen)
            response.Users.Add(new UserMessage
            {
                Id = 1,
                Name = "Test",
                Lastname = "User"
            });

            // 3. Antwort zurückgeben
            return Task.FromResult(response);
        }
    }
}
