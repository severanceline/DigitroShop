using Store.Application.Interfaces.Contexts;
using Store.Common;

namespace Store.Application.Services.Users.Queries.GetUsers
{
    public class GetUsersService : IGetUsersService
    {
        private readonly IDataBaseContext _context;
        public GetUsersService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultGetUserDto Execute(RequestGetUserDto request)
        {
            var users = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                users = users.Where(p => p.FullName.Contains(request.SearchKey)|| p.Email.Contains(request.SearchKey));
            }
            int rowsCount = 0;

            List<GetUsersDto> userDto = users.ToPaged(request.Page, request.PageSize, out rowsCount).Select(p => new GetUsersDto
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = p.Email,
                IsActive = p.IsActive
            }).ToList();

            return(new ResultGetUserDto { users = userDto, Rows = rowsCount });
        }
    }
}
 