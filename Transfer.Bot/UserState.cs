using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Bot.Dtos;
using Transfer.Common;
using Transfer.Dal.Entities;

namespace Transfer.Bot
{
    internal static class UserState
    {
        public async static Task<UserStateDto> GetState(this IUnitOfWork unitOfWork, Guid AccountId)
        {
            return await unitOfWork.GetSet<DbTgActionState>()
                .Where(x => x.AccountId == AccountId)
                .Select(x => new UserStateDto { State = x.State, StateParams = x.StateProps, IsActive = x.IsActive }).FirstOrDefaultAsync();
        }

        public async static Task SetState(this IUnitOfWork unitOfWork, long ChatId, string State = "Done", string Props = null)
        {
            var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == ChatId.ToString())).Select(x => x.Id).FirstAsync();
            await unitOfWork.SetState(user, State, Props);
        }

        public async static Task SetState(this IUnitOfWork unitOfWork, Guid AccountId, string State = "Done", string Props = null)
        {
            var state = await unitOfWork.GetSet<DbTgActionState>()
                .Where(x => x.AccountId == AccountId).FirstOrDefaultAsync();

            if(state == null)
            {
                state = new DbTgActionState { 
                    State = State,
                    StateProps = Props,
                    AccountId = AccountId,
                    IsActive = true
                };
                await unitOfWork.AddEntityAsync(state);
            }
            else
            {
                state.State = State;
                state.StateProps = Props;
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
