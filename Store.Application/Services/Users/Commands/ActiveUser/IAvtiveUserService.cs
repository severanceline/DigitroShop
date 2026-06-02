using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.ActiveUser
{
    public interface IAvtiveUserService
    {
        ResultDto<ResultActiveDto> Execute(long UserId);
    }
}
