﻿using AS.CMS.Business.Helpers;
using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base.Employee;
using AS.CMS.Domain.Common;
using NHibernate.Criterion;
using System.Linq;

namespace AS.CMS.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        #region Repository Injection

        private IBaseRepository<Employee> _employeeRepository;

        public EmployeeService(IBaseRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveEmployee(Employee employeeEntity)
        {
            if (employeeEntity.EmployeeJobExperience != null)
            {
                foreach (var jobExperienceItem in employeeEntity.EmployeeJobExperience)
                {
                    jobExperienceItem.Employee = employeeEntity;
                }
            }

            if (employeeEntity.EmployeeEducation != null)
            {
                foreach (var educationItem in employeeEntity.EmployeeEducation)
                {
                    educationItem.Employee = employeeEntity;
                }
            }

            if (employeeEntity.EmployeeCertificateAndLanguage != null)
            {
                foreach (var certificateItem in employeeEntity.EmployeeCertificateAndLanguage)
                {
                    certificateItem.Employee = employeeEntity;
                }
            }

            if (employeeEntity.EmployeeAvailability != null)
            {
                foreach (var availabilityItem in employeeEntity.EmployeeAvailability)
                {
                    availabilityItem.Employee = employeeEntity;
                }
            }

            if (employeeEntity.ID == 0)
            {
                _employeeRepository.Create(employeeEntity);
            }
            else
            {
                _employeeRepository.Update(employeeEntity);
            }

            return true;
        }

        public PageResultSet<Employee> GetActiveEmployees(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Employee>("FirstName", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Employee>("FirstName", pagingFilter.SearchText);

            rowCountcriteria.Add(Expression.Eq("Status", EmployeeStatus.Active));
            activeContentPagingcriteria.Add(Expression.Eq("Status", EmployeeStatus.Active));

            return new PageResultSet<Employee>()
            {
                PageData = _employeeRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _employeeRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public PageResultSet<Employee> GetApprovalEmployees(PagingFilter pagingFilter)
        {
            DetachedCriteria rowCountcriteria = CriteriaHelper.GetDefaultSearchCriteria<Employee>("FirstName", pagingFilter.SearchText);
            DetachedCriteria activeContentPagingcriteria = CriteriaHelper.GetDefaultSearchCriteria<Employee>("FirstName", pagingFilter.SearchText);

            rowCountcriteria.Add(Expression.Eq("Status", EmployeeStatus.NotActive));
            activeContentPagingcriteria.Add(Expression.Eq("Status", EmployeeStatus.NotActive));

            return new PageResultSet<Employee>()
            {
                PageData = _employeeRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter, CriteriaHelper.GetDefaultOrder()),
                Count = _employeeRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public int GetEmployeeCountByGender(GenderType gender)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<Employee>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));
            defaultCriteria.Add(Expression.Eq("Status", EmployeeStatus.Active));
            defaultCriteria.Add(Expression.Eq("Gender", gender));

            return _employeeRepository.GetRowCountWithCriteria(defaultCriteria);
        }

        public PageResultSet<Employee> GetActiveEmployeesFromSearch(Employee employeeSearchCriteria, PagingFilter pagingFilter)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<Employee>();
            defaultCriteria.Add(Expression.Eq("IsActive", true));
            defaultCriteria.Add(Expression.Eq("Status", EmployeeStatus.Active));

            if (!string.IsNullOrWhiteSpace(employeeSearchCriteria.FirstName))
            {
                defaultCriteria.Add(Expression.Eq("FirstName", employeeSearchCriteria.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(employeeSearchCriteria.LastName))
            {
                defaultCriteria.Add(Expression.Eq("LastName", employeeSearchCriteria.LastName));
            }

            if (!string.IsNullOrWhiteSpace(employeeSearchCriteria.Phone))
            {
                defaultCriteria.Add(Expression.Eq("Phone", employeeSearchCriteria.Phone));
            }

            if (employeeSearchCriteria.Height > 0)
            {
                defaultCriteria.Add(Expression.Eq("Height", employeeSearchCriteria.Height));
            }

            if (employeeSearchCriteria.Weight > 0)
            {
                defaultCriteria.Add(Expression.Eq("Weight", employeeSearchCriteria.Weight));
            }

            if (employeeSearchCriteria.Gender != GenderType.Unknown)
            {
                defaultCriteria.Add(Expression.Eq("Gender", employeeSearchCriteria.Gender));
            }

            DetachedCriteria rowCountcriteria = defaultCriteria;
            DetachedCriteria activeContentPagingcriteria = defaultCriteria;

            return new PageResultSet<Employee>()
            {
                PageData = _employeeRepository.GetWithCriteriaByPaging(activeContentPagingcriteria, pagingFilter),
                Count = _employeeRepository.GetRowCountWithCriteria(rowCountcriteria)
            };
        }

        public Employee GetEmployeeWithID(int employeeID)
        {
            return _employeeRepository.GetById(employeeID);
        }

        public Employee GetEmployeeWithMailAndPassword(string mail, string password)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<Employee>();
            defaultCriteria.Add(Expression.Eq("MailAddress", mail));
            defaultCriteria.Add(Expression.Eq("Password", UtilityHelper.GenerateMD5Hash(password)));
            defaultCriteria.Add(Expression.Eq("IsActive", true));
            defaultCriteria.Add(Expression.Eq("Status", EmployeeStatus.Active));

            return _employeeRepository.GetWithCriteria(defaultCriteria).FirstOrDefault();
        }

        public Employee GetEmployeeWithMail(string mail)
        {
            DetachedCriteria defaultCriteria = DetachedCriteria.For<Employee>();
            defaultCriteria.Add(Expression.Eq("MailAddress", mail));

            return _employeeRepository.GetWithCriteria(defaultCriteria).FirstOrDefault();
        }

        #endregion
    }
}