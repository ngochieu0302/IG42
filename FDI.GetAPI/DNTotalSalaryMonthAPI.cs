using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNTotalSalaryMonthAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<DNTotalSalaryMonthItem> GetListByMonthYear(int month, int year)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/GetListByMonthYear?key={1}&month={2}&year={3}", _url, Keyapi, month, year);
            return GetObjJson<List<DNTotalSalaryMonthItem>>(urlJson);
        }

        public DNTotalSalaryMonthItem GetByUseridMonthYear(Guid userId, int month, int year)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/GetByUseridMonthYear?key={1}&userId={2}&month={3}&year={4}", _url, Keyapi, userId, month, year);
            return GetObjJson<DNTotalSalaryMonthItem>(urlJson);
        }

        public DNTotalSalaryMonthItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/GetItemById?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DNTotalSalaryMonthItem>(urlJson);
        }

        public List<DNUserSimpleItem> GetSimpleListUserSalary(int agencyId)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/GetSimpleListUserSalary?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNUserSimpleItem>>(urlJson);
        }

        public SalaryMonthDetailItem GetItemByID(int id, int month, int year)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/GetItemByID?key={1}&id&month={3}&year={4}", _url, Keyapi, id, month, year);
            return GetObjJson<SalaryMonthDetailItem>(urlJson);
        }

        public SalaryMonthDetailItem GetByUseridMonthYearDefault(Guid userId, int month, int year)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/GetByUseridMonthYearDefault?key={1}&userId={2}&month={3}&year={4}", _url, Keyapi, userId, month, year);
            return GetObjJson<SalaryMonthDetailItem>(urlJson);
        }

        public int Update(string json, Guid UserId)
        {
            var urlJson = string.Format("{0}DNTotalSalaryMonth/Update?key={1}&{2}&UserId={3}", _url, Keyapi, json, UserId);
            return GetObjJson<int>(urlJson);
        }
    }
}
