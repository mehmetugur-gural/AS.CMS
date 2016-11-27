﻿using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;

namespace AS.CMS.Business.Interfaces
{
    public interface IEventProfessionQuotaService
    {
        bool SaveEventProfessionQuota(EventProfessionQuota eventProfessionQuotaEntity);
        PageResultSet<EventProfessionQuota> GetActiveEventProfessionQuotas(PagingFilter pagingFilter);
        EventProfessionQuota GetEventProfessionQuotaWithID(int EventProfessionQuotaID);
    }
}