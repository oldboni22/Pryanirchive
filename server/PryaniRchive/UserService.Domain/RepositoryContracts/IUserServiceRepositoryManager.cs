using Common.Data;

namespace UserService.Domain.RepositoryContracts;

public interface IUserServiceRepositoryManager : IRepositoryManager 
{
    IUserSpacePermissionRepository UserSpacePermissionRepository { get; }
}
