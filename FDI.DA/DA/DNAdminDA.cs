using System;
using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNAdminDA : BaseDA
    {
        #region Constructer
        public DNAdminDA()
        {
        }

        public DNAdminDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public DNAdminDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion
        public ModelAnalysisItem GetAllAnalysis(int agencyid, decimal datestart, decimal dateend)
        {
            var obj = new ModelAnalysisItem
            {

            };
            var count = FDIDB.Customer_Reward.Count(c => c.ID > 1 && (!c.Customer.IsDelete.HasValue || !c.Customer.IsDelete.Value) && c.AgencyID == agencyid);
            var Reward = FDIDB.Customer_Reward.Where(c => c.ID > 1 && (!c.Customer.IsDelete.HasValue || !c.Customer.IsDelete.Value) && c.AgencyID == agencyid).Sum(m=>m.PriceReward);
            var Receive = FDIDB.Customer_Reward.Where(c => c.ID > 1 && (!c.Customer.IsDelete.HasValue || !c.Customer.IsDelete.Value) && c.AgencyID == agencyid).Sum(m => m.PriceReceive);


            return obj;
        }
        public List<ModuleadminItem> getAllListSimple()
        {
            var query = from c in FDIDB.Modules
                        where c.ID > 1 && c.IsShow == true && c.IsDelete == false && c.PrarentID == 1
                        orderby c.Ord
                        select new ModuleadminItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Tag,
                            ClassCss = c.ClassCss,
                            PrarentID = c.PrarentID,
                            Module1 = c.Module1.Select(m => new ModuleItem
                            {
                                ID = m.ID,
                                NameModule = m.NameModule,
                                Tag = m.Tag,
                                ClassCss = m.ClassCss
                            })
                        };
            return query.ToList();
        }
        public List<ModuleadminItem> GetAllModuleByUserName(string username)
        {
            var query = from c in FDIDB.Modules
                        where c.ID > 1 && c.IsShow == true && c.IsDelete == false && c.aspnet_Users.Any(m => m.UserName == username)
                        orderby c.Ord
                        select new ModuleadminItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Tag,
                            ClassCss = c.ClassCss,
                            PrarentID = c.PrarentID,
                            Module1 = c.Module1.Select(m => new ModuleItem
                            {
                                ID = m.ID,
                                NameModule = m.NameModule,
                                Tag = m.Tag,
                                ClassCss = m.ClassCss
                            }),
                            Module2 = new ModulePrarentItem
                            {
                                ID = c.Module2.ID,
                                NameModule = c.Module2.NameModule,
                                Tag = c.Module2.Tag,
                                ClassCss = c.Module2.ClassCss
                            }
                        };

            if (!query.Any())
            {
                query = from c in FDIDB.Modules
                        where c.ID > 1 && c.IsShow == true && c.IsDelete == false && c.aspnet_Roles.Any(r => r.aspnet_Users.Any(m => m.UserName.ToLower() == username.ToLower()))
                        orderby c.Level, c.Ord
                        select new ModuleadminItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Tag,
                            ClassCss = c.ClassCss,
                            PrarentID = c.PrarentID,
                            Module1 = c.Module1.Select(m => new ModuleItem
                            {
                                ID = m.ID,
                                NameModule = m.NameModule,
                                Tag = m.Tag,
                                ClassCss = m.ClassCss
                            }),
                            Module2 = new ModulePrarentItem
                            {
                                ID = c.Module2.ID,
                                NameModule = c.Module2.NameModule,
                                Tag = c.Module2.Tag,
                                ClassCss = c.Module2.ClassCss
                            }
                        };
            }
            //var a = query.ToList();
            return query.ToList();
        }

        public aspnet_Roles getUser_Role_ModuleList(string nameRole)
        {
            var query = (from c in FDIDB.aspnet_Roles
                         where c.RoleName == nameRole
                         select c).FirstOrDefault();

            return query;
        }

        public aspnet_Users getUser_ModuleById(Guid userId)
        {
            var query = (from c in FDIDB.aspnet_Users
                         where c.UserId == userId
                         select c
                        ).FirstOrDefault();

            return query;
        }
    }
}
