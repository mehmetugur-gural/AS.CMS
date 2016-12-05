﻿using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Event;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Linq;

namespace AS.CMS.Business.Services
{
    public class EventService : IEventService
    {
        #region Repository Injection

        private IBaseRepository<Event> _eventRepository;
        private IBaseRepository<EventEmployee> _eventEmployeeRepository;

        public EventService(IBaseRepository<Event> eventRepository, IBaseRepository<EventEmployee> eventEmployeeRepository)
        {
            _eventRepository = eventRepository;
            _eventEmployeeRepository = eventEmployeeRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveEvent(Event eventEntity)
        {
            if (eventEntity.ID == 0)
            {
                _eventRepository.Create(eventEntity);
            }
            else
            {
                _eventRepository.Update(eventEntity);
            }

            return true;
        }

        public bool SaveEventEmployee(EventEmployee eventEmployeeEntity)
        {
            if (eventEmployeeEntity.ID == 0)
            {
                _eventEmployeeRepository.Create(eventEmployeeEntity);
            }
            else
            {
                _eventEmployeeRepository.Update(eventEmployeeEntity);
            }

            return true;
        }

        public IList<EventEmployee> GetActiveEventEmployees(int eventID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Event.ID", eventID));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Where(x => x.Event.IsActive == true).ToList();
        }

        public IList<EventEmployee> GetActiveEmployeeEvents(int employeeID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Employee.ID", employeeID));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria).Where(x => x.Event.IsActive == true).ToList();
        }

        public IList<EventEmployee> GetEventEmployeeWithID(int employeeID, int eventID)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<EventEmployee>();
            defaultCriteria.Add(Expression.Eq("Employee.ID", employeeID));
            defaultCriteria.Add(Expression.Eq("Event.ID", eventID));
            defaultCriteria.Add(Expression.Eq("IsActive", true));

            return _eventEmployeeRepository.GetWithCriteria(defaultCriteria);
        }

        public PageResultSet<Event> GetActiveEvents(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Event>("Name", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Event>("Name", pagingFilter.SearchText);

            return new PageResultSet<Event>()
            {
                PageData = _eventRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _eventRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Event GetEventWithID(int eventID)
        {
            return _eventRepository.GetById(eventID);
        }

        #endregion
    }
}