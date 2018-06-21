using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace POS.DataSvc
{
    public partial class POSSvcDbContext : DbContext
    {
        public Models.ActivityDo GetCurrentActivity(Models.FrontEndCriteriaDo criteria)
        {
            Models.ActivityDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_CurrentActivity]";
                
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);

                result = command.ToObject<Models.ActivityDo>();
            }));

            return result;
        }
        public Models.ActivityDo OpenDay(Models.OpenActivityDo entity)
        {
            Models.ActivityDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_OpenDay]";
                
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(decimal), "OpenAmount", entity.OpenAmount);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                List<Models.ActivityDo> res = command.ToList<Models.ActivityDo>();
                if (res != null)
                {
                    if (res.Count > 0)
                        result = res[0];
                }
            }));

            return result;
        }
        public void EndDay(Models.CloseActivityDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_EndDay]";
                
                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(decimal), "CloseAmount", entity.CloseAmount);

                string creditXml = Utils.ConvertUtil.ConvertToXml_Store<Models.CloseCreditDo>(entity.Credits);
                command.AddParameter(typeof(string), "CreditXML", creditXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        public void RePrintEndDay(Models.CloseActivityDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_RePrint_EndDay]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        public Models.ClientInfoDo GetClientInfo()
        {
            Models.ClientInfoDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_ClientInfo]";

                result = command.ToObject<Models.ClientInfoDo>();
            }));

            return result;
        }

        #region Table

        public List<Models.ZoneInBranchDo> GetTableInBranch(Models.FrontEndCriteriaDo criteria)
        {
            List<Models.ZoneInBranchDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_TableInBranch]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                List<Models.ZoneInBranchDo> zones = new List<Models.ZoneInBranchDo>();
                List<Models.TableInBranchDo> tables = command.ToList<Models.TableInBranchDo>();
                foreach (Models.TableInBranchDo table in tables)
                {
                    Models.ZoneInBranchDo zone = zones.Find(x => x.ZoneID == table.ZoneID);
                    if (zone == null)
                    {
                        zone = new Models.ZoneInBranchDo()
                        {
                            ZoneID = table.ZoneID,
                            BranchID = table.BranchID,
                            ZoneName = table.ZoneName
                        };
                        zone.Tables = new List<Models.TableInBranchDo>();

                        zones.Add(zone);
                    }

                    zone.Tables.Add(table);
                }

                result = zones;
            }));

            return result;
        }
        public List<Models.TableStatusInBranchDo> GetTableStatusInBranch(Models.TableStatusInBranchCriteriaDo criteria)
        {
            List<Models.TableStatusInBranchDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_TableStatusInBranch]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(int), "ZoneID", criteria.ZoneID);

                result = command.ToList<Models.TableStatusInBranchDo>();
            }));

            return result;
        }

        public List<Models.LockTableInBranchDo> GetLockTableInBranch(Models.FrontEndCriteriaDo criteria)
        {
            List<Models.LockTableInBranchDo> result = new List<Models.LockTableInBranchDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_LockTableInBranch]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                result = command.ToList<Models.LockTableInBranchDo>();
            }));

            return result;
        }
        public List<Models.LockTableInBranchDo> UnLockTable(Models.LockTableInBranchDo entity)
        {
            List<Models.LockTableInBranchDo> result = new List<Models.LockTableInBranchDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_UnLockTable]";
                
                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);
                command.AddParameter(typeof(int), "OrderID", entity.OrderID);

                result = command.ToList<Models.LockTableInBranchDo>();
            }));

            return result;
        }

        public Models.MergeTableResultDo MergeTable(Models.MergeTableDo entity)
        {
            Models.MergeTableResultDo result = new Models.MergeTableResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Merge_OrderTable]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(int), "TableID", entity.TableID);
                command.AddParameter(typeof(string), "LockKey", entity.LockKey);

                command.AddParameter(typeof(int), "MergeTableID", entity.MergeTableID);
                command.AddParameter(typeof(bool), "CloseTable", entity.CloseTable);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.CreateDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter mergeTableStatus = command.AddOutputParameter(typeof(string), "MergeTableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);
                Utils.SQL.ISQLDbParameter outStockDate = command.AddOutputParameter(typeof(DateTime), "LatestUpdate");

                System.Collections.IList[] dbls = command.ToList(
                     typeof(Models.OrderDo), typeof(Models.OrderTableDo),
                     typeof(Models.OrderItemDo), typeof(Models.DraftOrderPaymentDo),
                     typeof(Models.OutStockMenuInBranchDo));
                if (dbls != null)
                {
                    List<Models.OrderDo> dbos = dbls[0] as List<Models.OrderDo>;
                    List<Models.OrderTableDo> dbots = dbls[1] as List<Models.OrderTableDo>;
                    List<Models.OrderItemDo> dbois = dbls[2] as List<Models.OrderItemDo>;
                    List<Models.DraftOrderPaymentDo> dbdops = dbls[3] as List<Models.DraftOrderPaymentDo>;
                    List<Models.OutStockMenuInBranchDo> dbosms = dbls[4] as List<Models.OutStockMenuInBranchDo>;
                    if (dbos != null)
                    {
                        if (dbos.Count > 0)
                        {
                            result.Order = new Models.OrderResultDo();
                            result.Order.Order = dbos[0];

                            if (dbots != null)
                                result.Order.Tables = dbots;

                            result.Order.Items = new List<Models.OrderItemGroupDo>();
                            if (dbois != null)
                            {
                                foreach (Models.OrderItemDo item in dbois)
                                {
                                    if (item.ItemType == "ITEM" || item.ItemType == "SPCL")
                                    {
                                        Models.OrderItemGroupDo gitem = result.Order.Items.Find(x => x.ItemID == item.ItemID);
                                        if (gitem == null)
                                        {
                                            gitem = new Models.OrderItemGroupDo()
                                            {
                                                ItemID = item.ItemID
                                            };

                                            result.Order.Items.Add(gitem);
                                        }

                                        gitem.Item = item;
                                    }
                                    else
                                    {
                                        Models.OrderItemGroupDo gitem = result.Order.Items.Find(x => x.ItemID == item.ParentID);
                                        if (gitem != null)
                                        {
                                            if (item.ItemType == "COMT")
                                            {
                                                if (gitem.Comments == null)
                                                    gitem.Comments = new List<Models.OrderItemDo>();
                                                gitem.Comments.Add(item);
                                            }
                                            else if (item.ItemType == "DISC")
                                                gitem.DiscountItem = item;
                                            else if (item.ItemType == "VOID")
                                                gitem.VoidItem = item;
                                        }
                                    }
                                }
                            }

                            result.Order.DraftPayment = new Models.DraftOrderPaymentDo();
                            if (dbdops != null)
                            {
                                if (dbdops.Count > 0)
                                    result.Order.DraftPayment = dbdops[0];
                            }

                            if (dbosms != null)
                            {
                                if (dbosms.Count > 0)
                                    result.Order.Menus = dbosms;
                            }
                        }
                    }
                }
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (mergeTableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(mergeTableStatus.Value))
                        result.MergeTableStatus = (string)mergeTableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
                if (outStockDate != null)
                {
                    if (result.Order != null && !Utils.CommonUtil.IsNullOrEmpty(outStockDate.Value))
                        result.Order.LastUpdateMenu = (DateTime)outStockDate.Value;
                }
            }));

            return result;
        }
        public Models.MoveTableResultDo MoveTable(Models.MoveTableDo entity)
        {
            Models.MoveTableResultDo result = new Models.MoveTableResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Move_OrderTable]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(int), "TableID", entity.TableID);
                command.AddParameter(typeof(string), "LockKey", entity.LockKey);

                command.AddParameter(typeof(int), "MoveTableID", entity.MoveTableID);

                string itemXML = Utils.ConvertUtil.ConvertToXml_Store<Models.OrderItemDo>(entity.Items);
                command.AddParameter(typeof(string), "SplitOrderXML", itemXML);

                command.AddParameter(typeof(string), "MoveReason", entity.MoveReason);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.CreateDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter moveTableStatus = command.AddOutputParameter(typeof(string), "MoveTableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);
                Utils.SQL.ISQLDbParameter outStockDate = command.AddOutputParameter(typeof(DateTime), "LatestUpdate");

                System.Collections.IList[] dbls = command.ToList(
                        typeof(Models.OrderDo), typeof(Models.OrderTableDo),
                        typeof(Models.OrderItemDo), typeof(Models.DraftOrderPaymentDo),
                        typeof(Models.OutStockMenuInBranchDo));
                if (dbls != null)
                {
                    List<Models.OrderDo> dbos = dbls[0] as List<Models.OrderDo>;
                    List<Models.OrderTableDo> dbots = dbls[1] as List<Models.OrderTableDo>;
                    List<Models.OrderItemDo> dbois = dbls[2] as List<Models.OrderItemDo>;
                    List<Models.DraftOrderPaymentDo> dbdops = dbls[3] as List<Models.DraftOrderPaymentDo>;
                    List<Models.OutStockMenuInBranchDo> dbosms = dbls[4] as List<Models.OutStockMenuInBranchDo>;
                    if (dbos != null)
                    {
                        if (dbos.Count > 0)
                        {
                            result.Order = new Models.OrderResultDo();
                            result.Order.Order = dbos[0];

                            if (dbots != null)
                                result.Order.Tables = dbots;

                            result.Order.Items = new List<Models.OrderItemGroupDo>();
                            if (dbois != null)
                            {
                                foreach (Models.OrderItemDo item in dbois)
                                {
                                    if (item.ItemType == "ITEM" || item.ItemType == "SPCL")
                                    {
                                        Models.OrderItemGroupDo gitem = result.Order.Items.Find(x => x.ItemID == item.ItemID);
                                        if (gitem == null)
                                        {
                                            gitem = new Models.OrderItemGroupDo()
                                            {
                                                ItemID = item.ItemID
                                            };

                                            result.Order.Items.Add(gitem);
                                        }

                                        gitem.Item = item;
                                    }
                                    else
                                    {
                                        Models.OrderItemGroupDo gitem = result.Order.Items.Find(x => x.ItemID == item.ParentID);
                                        if (gitem != null)
                                        {
                                            if (item.ItemType == "COMT")
                                            {
                                                if (gitem.Comments == null)
                                                    gitem.Comments = new List<Models.OrderItemDo>();
                                                gitem.Comments.Add(item);
                                            }
                                            else if (item.ItemType == "DISC")
                                                gitem.DiscountItem = item;
                                            else if (item.ItemType == "VOID")
                                                gitem.VoidItem = item;
                                        }
                                    }
                                }
                            }

                            result.Order.DraftPayment = new Models.DraftOrderPaymentDo();
                            if (dbdops != null)
                            {
                                if (dbdops.Count > 0)
                                    result.Order.DraftPayment = dbdops[0];
                            }

                            if (dbosms != null)
                            {
                                if (dbosms.Count > 0)
                                    result.Order.Menus = dbosms;
                            }
                        }
                    }
                }
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (moveTableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(moveTableStatus.Value))
                        result.MoveTableStatus = (string)moveTableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
                if (outStockDate != null)
                {
                    if (result.Order != null && !Utils.CommonUtil.IsNullOrEmpty(outStockDate.Value))
                        result.Order.LastUpdateMenu = (DateTime)outStockDate.Value;
                }
            }));

            return result;
        }

        public Models.OrderResultDo CloseTable(Models.OrderCriteriaDo entity)
        {
            Models.OrderResultDo result = new Models.OrderResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_CloseTable]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);
                command.AddParameter(typeof(int), "TableID", entity.TableID);
                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);

                result.Tables = command.ToList<Models.OrderTableDo>();
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
            }));

            return result;
        }
        
        #endregion
        #region Menu

        public List<Models.MenuGroupInBranchDo> GetMenuInBranch(Models.MenuInBranchCriteriaDo criteria)
        {
            List<Models.MenuGroupInBranchDo> result = new List<Models.MenuGroupInBranchDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_MenuInBranch]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(int), "OffsetRow", criteria.OffsetRow);
                command.AddParameter(typeof(int), "NextRowCount", criteria.NextRowCount);

                List<Models.MenuGroupInBranchDo> groups = new List<Models.MenuGroupInBranchDo>();
                List<Models.MenuInBranchDo> menus = command.ToList<Models.MenuInBranchDo>();
                foreach (Models.MenuInBranchDo menu in menus)
                {
                    Models.MenuGroupInBranchDo group = groups.Find(x => x.GroupID == menu.GroupID
                                                                    && x.FlagTakeAway == menu.FlagTakeAway);
                    if (group == null)
                    {
                        group = new Models.MenuGroupInBranchDo()
                        {
                            GroupID = menu.GroupID,
                            Name = menu.GroupName,
                            FlagTakeAway = menu.FlagTakeAway
                        };
                        group.Categories = new List<Models.MenuCategoryInBranchDo>();

                        groups.Add(group);
                    }

                    Models.MenuCategoryInBranchDo category = group.Categories.Find(x => x.GroupID == menu.GroupID
                                                                                    && x.CategoryID == menu.CategoryID
                                                                                    && x.FlagTakeAway == menu.FlagTakeAway);
                    if (category == null)
                    {
                        category = new Models.MenuCategoryInBranchDo()
                        {
                            GroupID = menu.GroupID,
                            CategoryID = menu.CategoryID,
                            Name = menu.CategoryName,
                            FlagTakeAway = menu.FlagTakeAway
                        };
                        category.MenuSubs = new List<Models.MenuSubInBranchDo>();

                        group.Categories.Add(category);
                    }

                    Models.MenuSubInBranchDo sub = category.MenuSubs.Find(x => x.GroupID == menu.GroupID
                                                                                && x.CategoryID == menu.CategoryID
                                                                                && x.SubCategoryID == menu.SubCategoryID
                                                                                && x.FlagTakeAway == menu.FlagTakeAway);
                    if (sub == null)
                    {
                        sub = new Models.MenuSubInBranchDo()
                        {
                            GroupID = menu.GroupID,
                            CategoryID = menu.CategoryID,
                            SubCategoryID = menu.SubCategoryID,
                            Name = menu.SubCategoryName,
                            FlagHas1Menu = menu.FlagHas1Menu,
                            FlagTakeAway = menu.FlagTakeAway
                        };
                        sub.Menus = new List<Models.MenuInBranchDo>();

                        category.MenuSubs.Add(sub);
                    }

                    sub.Menus.Add(menu);
                }

                result = groups;
            }));

            return result;
        }
        public List<Models.SpecialMenuAutoComopleteDo> GetSpecialMenuAutoComplete(Models.SpecialMenuCriteriaDo criteria)
        {
            List<Models.SpecialMenuAutoComopleteDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Get_SpecialMenuAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(string), "MenuName", criteria.MenuName);
                command.AddParameter(typeof(bool), "FlagTakeAway", criteria.FlagTakeAway);

               result = command.ToList<Models.SpecialMenuAutoComopleteDo>();
            }));

            return result;
        }
        
        #endregion
        #region Menu Out Stock

        public List<Models.OutStockMenuInBranchDo> GetOutStockMenuInBranch(Models.MenuInBranchCriteriaDo criteria)
        {
            List<Models.OutStockMenuInBranchDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OutStockMenuInBranch]";
                
                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(DateTime), "UpdateDate", criteria.UpdateDate);

                result = command.ToList<Models.OutStockMenuInBranchDo>();
            }));

            return result;

        }
        public void UpdateOutStockMenuInBranch(Models.UpdateOutStockMenuInBranchDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Update_OutStockMenuInBranch]";
                
                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                string menuXML = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuInBranchDo>(entity.Menus);
                command.AddParameter(typeof(string), "MenuXML", menuXML);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
        #region Reason

        public List<Models.ReasonCategoryInBranchDo> GetReasonInBranch(Models.ReasonInBranchCriteriaDo criteria)
        {
            List<Models.ReasonCategoryInBranchDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_ReasonInBranch]";
                
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "ReasonGroupID", criteria.ReasonGroupID);
                command.AddParameter(typeof(int), "ReasonCategoryID", criteria.ReasonCategoryID);

                List<Models.ReasonCategoryInBranchDo> categories = new List<Models.ReasonCategoryInBranchDo>();
                List<Models.ReasonInBranchDo> reasons = command.ToList<Models.ReasonInBranchDo>();
                foreach (Models.ReasonInBranchDo reason in reasons)
                {
                    Models.ReasonCategoryInBranchDo category = categories.Find(x => x.ReasonGroupID == reason.ReasonGroupID
                                                                    && x.ReasonCategoryID == reason.ReasonCategoryID);
                    if (category == null)
                    {
                        category = new Models.ReasonCategoryInBranchDo()
                        {
                            ReasonGroupID = reason.ReasonGroupID,
                            ReasonCategoryID = reason.ReasonCategoryID,
                            ReasonCategoryName = reason.ReasonCategoryName
                        };
                        category.Reasons = new List<Models.ReasonInBranchDo>();

                        categories.Add(category);
                    }

                    category.Reasons.Add(reason);
                }

                result = categories;
            }));

            return result;
        }

        #endregion
        #region Order

        public Models.OrderResultDo GetOrder(Models.OrderCriteriaDo criteria)
        {
            Models.OrderResultDo result = new Models.OrderResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Get_Order]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(int), "TableID", criteria.TableID);
                command.AddParameter(typeof(string), "LockKey", criteria.LockKey);
                command.AddParameter(typeof(DateTime), "CreateDate", criteria.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", criteria.CreateUser);
                command.AddParameter(typeof(DateTime), "LatestUpdateMenu", criteria.LatestUpdateMenu);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);
                Utils.SQL.ISQLDbParameter outStockDate = command.AddOutputParameter(typeof(DateTime), "LatestUpdate");

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.OrderDo), typeof(Models.OrderTableDo),
                    typeof(Models.OrderItemDo), typeof(Models.OrderPromotionDo), typeof(Models.DraftOrderPaymentDo),
                    typeof(Models.OutStockMenuInBranchDo));
                if (dbls != null)
                {
                    List<Models.OrderDo> dbos = dbls[0] as List<Models.OrderDo>;
                    List<Models.OrderTableDo> dbots = dbls[1] as List<Models.OrderTableDo>;
                    List<Models.OrderItemDo> dbois = dbls[2] as List<Models.OrderItemDo>;
                    List<Models.OrderPromotionDo> dbops = dbls[3] as List<Models.OrderPromotionDo>;
                    List<Models.DraftOrderPaymentDo> dbdops = dbls[4] as List<Models.DraftOrderPaymentDo>;
                    List<Models.OutStockMenuInBranchDo> dbosms = dbls[5] as List<Models.OutStockMenuInBranchDo>;
                    if (dbos != null)
                    {
                        if (dbos.Count > 0)
                        {
                            result.Order = dbos[0];

                            if (dbots != null)
                                result.Tables = dbots;

                            result.Items = new List<Models.OrderItemGroupDo>();
                            if (dbois != null)
                            {
                                foreach (Models.OrderItemDo item in dbois)
                                {
                                    if (item.ItemType == "ITEM" || item.ItemType == "SPCL")
                                    {
                                        Models.OrderItemGroupDo gitem = result.Items.Find(x => x.ItemID == item.ItemID);
                                        if (gitem == null)
                                        {
                                            gitem = new Models.OrderItemGroupDo()
                                            {
                                                ItemID = item.ItemID
                                            };

                                            result.Items.Add(gitem);
                                        }

                                        gitem.Item = item;
                                    }
                                    else
                                    {
                                        Models.OrderItemGroupDo gitem = result.Items.Find(x => x.ItemID == item.ParentID);
                                        if (gitem != null)
                                        {
                                            if (item.ItemType == "COMT")
                                            {
                                                if (gitem.Comments == null)
                                                    gitem.Comments = new List<Models.OrderItemDo>();
                                                gitem.Comments.Add(item);
                                            }
                                            else if (item.ItemType == "DISC")
                                                gitem.DiscountItem = item;
                                            else if (item.ItemType == "VOID")
                                                gitem.VoidItem = item;
                                        }
                                    }
                                }
                            }

                            result.Promotions = new List<Models.OrderPromotionDo>();
                            if (dbops != null)
                                result.Promotions = dbops;

                            result.DraftPayment = new Models.DraftOrderPaymentDo();
                            if (dbdops != null)
                            {
                                if (dbdops.Count > 0)
                                    result.DraftPayment = dbdops[0];
                            }

                            if (dbosms != null)
                            {
                                if (dbosms.Count > 0)
                                    result.Menus = dbosms;
                            }
                        }
                    }
                }
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
                if (outStockDate != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(outStockDate.Value))
                        result.LastUpdateMenu = (DateTime)outStockDate.Value;
                }
            }));

            return result;
        }
        public Models.OrderResultDo CreateOrder(Models.NewOrderDo entity)
        {
            Models.OrderResultDo result = new Models.OrderResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Create_Order]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(int), "TableID", entity.TableID);
                command.AddParameter(typeof(int), "CustomerQty", entity.CustomerQty);
                command.AddParameter(typeof(string), "POSName", entity.POSName);
                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.OrderDo), typeof(Models.OrderTableDo));
                if (dbls != null)
                {
                    List<Models.OrderDo> dbos = dbls[0] as List<Models.OrderDo>;
                    List<Models.OrderTableDo> dbots = dbls[1] as List<Models.OrderTableDo>;
                    if (dbos != null)
                    {
                        if (dbos.Count > 0)
                        {
                            result.Order = dbos[0];

                            if (dbots != null)
                                result.Tables = dbots;

                            result.DraftPayment = new Models.DraftOrderPaymentDo();
                        }
                    }
                }
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
            }));

            return result;
        }
        public Models.OrderResultDo UpdateOrder(Models.UpdateOrderDo entity)
        {
            Models.OrderResultDo result = new Models.OrderResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Create_OrderItem]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "TransactionRefID", entity.TransactionRefID);

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(int), "OrderID", entity.OrderID);
                command.AddParameter(typeof(int), "TableID", entity.TableID);
                command.AddParameter(typeof(DateTime), "LatestUpdate", entity.LatestUpdate);
                command.AddParameter(typeof(string), "LockKey", entity.LockKey);

                string transactionXML = Utils.ConvertUtil.ConvertToXml_Store<Models.OrderTransactionDo>(entity.Transactions);
                command.AddParameter(typeof(string), "TransactionXML", transactionXML);

                string promotionXML = Utils.ConvertUtil.ConvertToXml_Store<Models.PromotionTransactionDo>(entity.Promotions);
                command.AddParameter(typeof(string), "PromotionTransactionXML", promotionXML);

                List<Models.DraftPaymentTransactionDo> drafts = new List<Models.DraftPaymentTransactionDo>();
                if (entity.DraftPayment != null)
                    drafts.Add(entity.DraftPayment);

                string paymentTransactionXML = Utils.ConvertUtil.ConvertToXml_Store<Models.DraftPaymentTransactionDo>(drafts);
                command.AddParameter(typeof(string), "PaymentTransactionXML", paymentTransactionXML);

                command.AddParameter(typeof(string), "FlagPrint", entity.FlagPrint);
                command.AddParameter(typeof(string), "FlagCheckBill", entity.FlagCheckBill);
                command.AddParameter(typeof(string), "FlagUnLock", entity.FlagUnLock);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);

                command.ExecuteNonQuery();
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
            }));

            return result;
        }
        public void UpdateCustomer(Models.UpdateOrderCustomerDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Update_OrderCustomerQty]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);
                command.AddParameter(typeof(int), "OrderID", entity.OrderID);
                command.AddParameter(typeof(int), "CustomerQty", entity.CustomerQty);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        public Models.OrderPaymentResultDo OrderPayment(Models.UpdateOrderDo entity)
        {
            Models.OrderPaymentResultDo result = new Models.OrderPaymentResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_OrderPayment]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "TransactionRefID", entity.TransactionRefID);

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(int), "OrderID", entity.OrderID);
                command.AddParameter(typeof(int), "TableID", entity.TableID);
                command.AddParameter(typeof(DateTime), "LatestUpdate", entity.LatestUpdate);
                command.AddParameter(typeof(string), "LockKey", entity.LockKey);

                string paymentXML = Utils.ConvertUtil.ConvertToXml_Store<Models.PaymentTransactionDo>(entity.Payments);
                command.AddParameter(typeof(string), "PaymentXML", paymentXML);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                Utils.SQL.ISQLDbParameter tableStatus = command.AddOutputParameter(typeof(string), "TableStatus", size: 4);
                Utils.SQL.ISQLDbParameter lockUser = command.AddOutputParameter(typeof(string), "LockUser", size: 100);

                result.Order = command.ToObject<Models.OrderPaymentDo>();
                result.ErrorParameter(error);

                if (tableStatus != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(tableStatus.Value))
                        result.TableStatus = (string)tableStatus.Value;
                }
                if (lockUser != null)
                {
                    if (!Utils.CommonUtil.IsNullOrEmpty(lockUser.Value))
                        result.LockUser = (string)lockUser.Value;
                }
            }));

            return result;
        }

        public List<Models.OrderItemHistoryDo> GetOrderItemHistory(Models.OrderCriteriaDo criteria)
        {
            List<Models.OrderItemHistoryDo> result = new List<Models.OrderItemHistoryDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Get_OrderItemHistory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                result = command.ToList<Models.OrderItemHistoryDo>();
            }));

            return result;
        }
        public List<Models.SummaryOrderItemDo> GetSummaryOrderItem(Models.OrderCriteriaDo criteria)
        {
            List<Models.SummaryOrderItemDo> result = new List<Models.SummaryOrderItemDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Get_SummaryOrderItem]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                result = command.ToList<Models.SummaryOrderItemDo>();
            }));

            return result;
        }
        public Models.SlipOrderDo GetSlipOrder(Models.SlipOrderCriteriaDo criteria)
        {
            Models.SlipOrderDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_SlipOrder]";

                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);
                command.AddParameter(typeof(string), "TaxNo", criteria.TaxNo);

                System.Collections.IList[] dbls = command.ToList(
                        typeof(Models.SlipOrderDo), typeof(Models.OrderTableDo),
                        typeof(Models.OrderItemDo));
                if (dbls != null)
                {
                    List<Models.SlipOrderDo> dbsos = dbls[0] as List<Models.SlipOrderDo>;
                    List<Models.OrderTableDo> dbots = dbls[1] as List<Models.OrderTableDo>;
                    List<Models.OrderItemDo> dbois = dbls[2] as List<Models.OrderItemDo>;
                    if (dbsos != null)
                    {
                        if (dbsos.Count > 0)
                        {
                            if (dbots != null)
                                dbsos[0].Tables = dbots;
                            if (dbois != null)
                            {
                                dbsos[0].Items = new List<Models.OrderItemGroupDo>();
                                foreach (Models.OrderItemDo item in dbois)
                                {
                                    if (item.ItemType == "ITEM" || item.ItemType == "SPCL")
                                    {
                                        Models.OrderItemGroupDo gitem = dbsos[0].Items.Find(x => x.ItemID == item.ItemID);
                                        if (gitem == null)
                                        {
                                            gitem = new Models.OrderItemGroupDo()
                                            {
                                                ItemID = item.ItemID
                                            };

                                            dbsos[0].Items.Add(gitem);
                                        }

                                        gitem.Item = item;
                                    }
                                    else
                                    {
                                        Models.OrderItemGroupDo gitem = dbsos[0].Items.Find(x => x.ItemID == item.ParentID);
                                        if (gitem != null)
                                        {
                                            if (item.ItemType == "COMT")
                                            {
                                                if (gitem.Comments == null)
                                                    gitem.Comments = new List<Models.OrderItemDo>();
                                                gitem.Comments.Add(item);
                                            }
                                            else if (item.ItemType == "DISC")
                                                gitem.DiscountItem = item;
                                            else if (item.ItemType == "VOID")
                                                gitem.VoidItem = item;
                                        }
                                    }
                                }
                            }


                            result = dbsos[0];
                        }
                    }
                }
            }));

            return result;
        }

        public void RePrintOrder(Models.RePrintOrderDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_RePrint_Order]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);
                command.AddParameter(typeof(int), "OrderID", entity.OrderID);
                command.AddParameter(typeof(string), "OrderType", entity.OrderType);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
        #region Discount

        public List<Models.DiscountInBranchDo> GetDiscountInBranch(Models.FrontEndCriteriaDo criteria)
        {
            List<Models.DiscountInBranchDo> result = new List<Models.DiscountInBranchDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_DiscountInBranch]";

                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);

                System.Collections.IList[] dbls = command.ToList(typeof(Models.DiscountInBranchDo), typeof(Models.DiscountUserGroupInBranchDo));
                if (dbls != null)
                {
                    List<Models.DiscountInBranchDo> dbds = dbls[0] as List<Models.DiscountInBranchDo>;
                    List<Models.DiscountUserGroupInBranchDo> dbdus = dbls[1] as List<Models.DiscountUserGroupInBranchDo>;
                    if (dbds != null)
                    {
                        if (dbds.Count > 0)
                        {
                            if (dbdus != null)
                            {
                                foreach (Models.DiscountInBranchDo discount in dbds)
                                {
                                    discount.Groups = dbdus.FindAll(x => x.DiscountID == discount.DiscountID);
                                }
                            }

                            result = dbds;
                        }
                    }
                }
            }));

            return result;
        }

        public List<Models.UserForDiscountDo> GetUserForDiscount()
        {
            List<Models.UserForDiscountDo> result = new List<Models.UserForDiscountDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_UserForDiscount]";
                
                result = command.ToList<Models.UserForDiscountDo>();
            }));

            return result;
        }
        
        #endregion
        #region Member

        public Models.MemberInBranchResultDo GetMemberInBranch(Models.MemberInBranchCriteriaDo criteria)
        {
            Models.MemberInBranchResultDo result = new Models.MemberInBranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_MemberInBranch]";

                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(string), "MemberCode", criteria.MemberCode);
                command.AddParameter(typeof(string), "MemberType", criteria.MemberType);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                result.Member = command.ToObject<Models.MemberInBranchDo>();
                result.ErrorParameter(error);
            }));

            return result;
        }

        #endregion
        #region Voucher

        public Models.VoucherInBranchResultDo GetVoucherInBranch(Models.VoucherInBranchCriteriaDo criteria)
        {
            Models.VoucherInBranchResultDo result = new Models.VoucherInBranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_VoucherInBranch]";
                
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(string), "VoucherNumber", criteria.VoucherNumber);
                command.AddParameter(typeof(string), "VoucherType", criteria.VoucherType);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                result.Voucher = command.ToObject<Models.VoucherInBranchDo>();
                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.ActivateVoucherResultDo ActivateVoucherInBranch(Models.ActivateVoucherDo entity)
        {
            Models.ActivateVoucherResultDo result = new Models.ActivateVoucherResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Activate_VoucherInBranch]";

                command.AddParameter(typeof(int), "BranchID", entity.BranchID);

                string voucherXML = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherDo>(entity.Vouchers);
                command.AddParameter(typeof(string), "VoucherXML", voucherXML);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
                result.Data = true;
            }));

            return result;
        }

        #endregion
        #region Promotion

        public Models.PromotionInBranchResultDo GetPromotionInBranch(Models.PromotionInBranchCriteriaDo criteria)
        {
            Models.PromotionInBranchResultDo result = new Models.PromotionInBranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_PromotionInBranch]";

                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(string), "PromotionNumber", criteria.PromotionNumber);
                command.AddParameter(typeof(string), "PromotionType", criteria.PromotionType);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                result.Promotion = command.ToObject<Models.PromotionInBranchDo>();
                result.ErrorParameter(error);
            }));

            return result;
        }

        #endregion
        #region Cash In/Out

        public List<Models.CashInOutDo> GetCashInOut(Models.CashInOutCriteriaDo criteria)
        {
            List<Models.CashInOutDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_CashInOut]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(string), "Type", criteria.Type);

                result = command.ToList<Models.CashInOutDo>();
            }));

            return result;
        }

        public void CreateCashInOut(Models.NewCashInOutDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Create_CashInOut]";

                command.AddParameter(typeof(string), "TransactionRefID", entity.TransactionRefID);

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(string), "Type", entity.Type);

                string itemXML = Utils.ConvertUtil.ConvertToXml_Store<Models.CashInOutDo>(entity.Items);
                command.AddParameter(typeof(string), "CashInOutXML", itemXML);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
        #region Summary

        public Models.SummaryActivityDo GetSummaryActivity(Models.SummaryActivityCriteriaDo criteria)
        {
            Models.SummaryActivityDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_SummaryActivity]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                result = command.ToObject<Models.SummaryActivityDo>();
            }));

            return result;
        }
        public Models.SummaryActivityDetailDo GetSummaryActivityDetail(Models.SummaryActivityCriteriaDo criteria)
        {
            Models.SummaryActivityDetailDo result = new Models.SummaryActivityDetailDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_SummaryActivityDetail]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                System.Collections.IList[] dbls = command.ToList(
                        typeof(Models.SummaryActivityMenuGroupDo),
                        typeof(Models.SummaryActivityPaymentDo), typeof(Models.SummaryActivityCashInOutDo),
                        typeof(Models.SummaryActivityDiscountDo));
                if (dbls != null)
                {
                    List<Models.SummaryActivityMenuGroupDo> dbsamgs = dbls[0] as List<Models.SummaryActivityMenuGroupDo>;
                    List<Models.SummaryActivityPaymentDo> dbsaps = dbls[1] as List<Models.SummaryActivityPaymentDo>;
                    List<Models.SummaryActivityCashInOutDo> dbsacs = dbls[2] as List<Models.SummaryActivityCashInOutDo>;
                    List<Models.SummaryActivityDiscountDo> dbsads = dbls[3] as List<Models.SummaryActivityDiscountDo>;

                    if (dbsamgs != null)
                    {
                        result.MenuGroups = dbsamgs.FindAll(x => !x.FlagTakeAway);
                        result.MenuGroupTakeAways = dbsamgs.FindAll(x => x.FlagTakeAway);
                    }
                    if (dbsaps != null)
                        result.Payments = dbsaps;
                    if (dbsacs != null)
                        result.CashInOuts = dbsacs;
                    if (dbsads != null)
                        result.Discounts = dbsads;
                }
            }));

            return result;
        }

        #endregion
        #region Print

        public List<Models.OrderItemForPrintDo> GetOrderItemForPrint(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.OrderItemForPrintDo> result = new List<Models.OrderItemForPrintDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OrderItemForPrint]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                result = command.ToList<Models.OrderItemForPrintDo>();
            }));

            return result;
        }
        public List<Models.OrderForBillDo> GetOrderForBill(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.OrderForBillDo> result = new List<Models.OrderForBillDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OrderItemForBill]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                System.Collections.IList[] dbls = command.ToList(
                       typeof(Models.OrderItemForBillDo), typeof(Models.OrderTableForBillDo),
                       typeof(Models.OrderRemarkForBillDo));
                if (dbls != null)
                {
                    List<Models.OrderItemForBillDo> dbois = dbls[0] as List<Models.OrderItemForBillDo>;
                    List<Models.OrderTableForBillDo> dbots = dbls[1] as List<Models.OrderTableForBillDo>;
                    List<Models.OrderRemarkForBillDo> dbors = dbls[2] as List<Models.OrderRemarkForBillDo>;
                    if (dbois != null)
                    {
                        foreach (Models.OrderItemForBillDo item in dbois)
                        {
                            Models.OrderForBillDo order = result.Find(x => x.TransactionRefID == item.TransactionRefID
                                                                            && x.FrontEndID == item.FrontEndID
                                                                            && x.BranchID == item.BranchID
                                                                            && x.SHIFT == item.SHIFT
                                                                            && x.OrderID == item.OrderID);
                            if (order == null)
                            {
                                order = new Models.OrderForBillDo()
                                {
                                    TransactionRefID = item.TransactionRefID,
                                    FrontEndID = item.FrontEndID,
                                    BranchID = item.BranchID,
                                    SHIFT = item.SHIFT,
                                    OrderID = item.OrderID,
                                    BranchName = item.BranchName,
                                    CustomerQty = item.CustomerQty,
                                    CreateUser = item.CreateUser,
                                    BillNo = item.BillNo,
                                    PrintDate = item.PrintDate,
                                    PrintNo = item.PrintNo,
                                    ApproveUser = item.ApproveUser,
                                    TotalAmt = item.TotalAmt,
                                    TotalLineDiscountAmt = item.TotalLineDiscountAmt,
                                    TotalPromotionDiscountAmt = item.TotalPromotionDiscountAmt,
                                    TotalPromotionVoucherAmt = item.TotalPromotionVoucherAmt,
                                    SubTotalAmt = item.SubTotalAmt,
                                    GrandTotalAmt = item.GrandTotalAmt,
                                    DiscountName = item.DiscountName,
                                    DiscountAmt = item.DiscountAmt,
                                    ServiceChargeRate = item.ServiceChargeRate,
                                    ServiceChargeAmt = item.ServiceChargeAmt,
                                    PrinterName = item.PrinterName
                                };
                                order.Items = new List<Models.OrderItemForBillDo>();

                                if (dbots != null)
                                {
                                    order.Tables = dbots.FindAll(x => x.TransactionRefID == order.TransactionRefID
                                                                        && x.FrontEndID == order.FrontEndID
                                                                        && x.BranchID == order.BranchID
                                                                        && x.SHIFT == order.SHIFT
                                                                        && x.OrderID == order.OrderID);
                                }
                                if (dbors != null)
                                {
                                    order.Remarks = dbors.FindAll(x => x.TransactionRefID == order.TransactionRefID
                                                                        && x.FrontEndID == order.FrontEndID
                                                                        && x.BranchID == order.BranchID
                                                                        && x.SHIFT == order.SHIFT
                                                                        && x.OrderID == order.OrderID);
                                }

                                result.Add(order);
                            }

                            order.Items.Add(item);
                        }
                    }
                }
            }));

            return result;
        }
        public List<Models.OrderForSlipDo> GetOrderForSlip(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.OrderForSlipDo> result = new List<Models.OrderForSlipDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OrderItemForSlip]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                System.Collections.IList[] dbls = command.ToList(
                       typeof(Models.OrderItemForSlipDo), typeof(Models.OrderTableForSlipDo));
                if (dbls != null)
                {
                    List<Models.OrderItemForSlipDo> dbois = dbls[0] as List<Models.OrderItemForSlipDo>;
                    List<Models.OrderTableForSlipDo> dbots = dbls[1] as List<Models.OrderTableForSlipDo>;
                    if (dbois != null)
                    {
                        foreach (Models.OrderItemForSlipDo item in dbois)
                        {
                            Models.OrderForSlipDo order = result.Find(x => x.TransactionRefID == item.TransactionRefID
                                                                            && x.FrontEndID == item.FrontEndID
                                                                            && x.BranchID == item.BranchID
                                                                            && x.SHIFT == item.SHIFT
                                                                            && x.OrderID == item.OrderID);
                            if (order == null)
                            {
                                order = new Models.OrderForSlipDo()
                                {
                                    TransactionRefID = item.TransactionRefID,
                                    FrontEndID = item.FrontEndID,
                                    BranchID = item.BranchID,
                                    SHIFT = item.SHIFT,
                                    OrderID = item.OrderID,
                                    BrandName = item.BrandName,
                                    BranchName = item.BranchName,
                                    BillNo = item.BillNo,
                                    PrintDate = item.PrintDate,

                                    TotalAmt = item.TotalAmt,
                                    TotalLineDiscountAmt = item.TotalLineDiscountAmt,
                                    TotalPromotionDiscountAmt = item.TotalPromotionDiscountAmt,
                                    TotalPromotionVoucherAmt = item.TotalPromotionVoucherAmt,
                                    SubTotalAmt = item.SubTotalAmt,
                                    GrandTotalAmt = item.GrandTotalAmt,

                                    DiscountName = item.DiscountName,
                                    DiscountAmt = item.DiscountAmt,
                                    ServiceChargeRate = item.ServiceChargeRate,
                                    ServiceChargeAmt = item.ServiceChargeAmt,
                                    Qty = item.Qty,
                                    PrinterName = item.PrinterName

                                };
                                order.Methods = new List<Models.OrderItemForSlipDo>();

                                if (dbots != null)
                                {
                                    order.Tables = dbots.FindAll(x => x.TransactionRefID == order.TransactionRefID
                                                                        && x.FrontEndID == order.FrontEndID
                                                                        && x.BranchID == order.BranchID
                                                                        && x.SHIFT == order.SHIFT
                                                                        && x.OrderID == order.OrderID);
                                }

                                result.Add(order);
                            }

                            order.Methods.Add(item);
                        }
                    }
                }
            }));

            return result;
        }
        public List<Models.CashInOutForPrintDo> GetCashInOutForPrint(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.CashInOutForPrintDo> result = new List<Models.CashInOutForPrintDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_CashInOutForPrint]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                result = command.ToList<Models.CashInOutForPrintDo>();
            }));

            return result;
        }
        public List<Models.SummaryEndDayForPrintDo> GetSummaryEndDayForPrint(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.SummaryEndDayForPrintDo> result = new List<Models.SummaryEndDayForPrintDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_SummaryEndDay]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                System.Collections.IList[] dbls = command.ToList(
                   typeof(Models.SummaryEndDayForPrintDo), 
                   typeof(Models.SummaryEndDayCashInOutForPrintDo),
                   typeof(Models.SummaryEndDayCreditForPrintDo), 
                   typeof(Models.SummaryEndDayDiscountForPrintDo),
                   typeof(Models.SummaryEndDayMenuGroupForPrintDo), 
                   typeof(Models.SummaryEndDaySalesForPrintDo),
                   typeof(Models.SummaryEndDayOrderTaxForPrintDo),
                   typeof(Models.SummaryEndDayPaymentForPrintDo),
                   typeof(Models.SummaryEndDayDiscountReasonForPrintDo));
                if (dbls != null)
                {
                    List<Models.SummaryEndDayForPrintDo> dbss = dbls[0] as List<Models.SummaryEndDayForPrintDo>;
                    List<Models.SummaryEndDayCashInOutForPrintDo> dbscs = dbls[1] as List<Models.SummaryEndDayCashInOutForPrintDo>;
                    List<Models.SummaryEndDayCreditForPrintDo> dbcds = dbls[2] as List<Models.SummaryEndDayCreditForPrintDo>;
                    List<Models.SummaryEndDayDiscountForPrintDo> dbds = dbls[3] as List<Models.SummaryEndDayDiscountForPrintDo>;
                    List<Models.SummaryEndDayMenuGroupForPrintDo> dbms = dbls[4] as List<Models.SummaryEndDayMenuGroupForPrintDo>;
                    List<Models.SummaryEndDaySalesForPrintDo> dbses = dbls[5] as List<Models.SummaryEndDaySalesForPrintDo>;
                    List<Models.SummaryEndDayOrderTaxForPrintDo> dbts = dbls[6] as List<Models.SummaryEndDayOrderTaxForPrintDo>;
                    List<Models.SummaryEndDayPaymentForPrintDo> dbps = dbls[7] as List<Models.SummaryEndDayPaymentForPrintDo>;
                    List<Models.SummaryEndDayDiscountReasonForPrintDo> dbdrs = dbls[8] as List<Models.SummaryEndDayDiscountReasonForPrintDo>;
                    if (dbss != null)
                    {
                        foreach (Models.SummaryEndDayForPrintDo summary in dbss)
                        {
                            if (dbscs != null)
                                summary.CashInOuts = dbscs.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT);

                            if (dbcds != null)
                                summary.Credits = dbcds.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT);

                            if (dbds != null)
                                summary.Discounts = dbds.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT);

                            if (dbms != null)
                                summary.MenuGroups = dbms.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                           && x.FrontEndID == summary.FrontEndID
                                                                           && x.BranchID == summary.BranchID
                                                                           && x.SHIFT == summary.SHIFT
                                                                           && x.FlagTakeAway == false);

                            if (dbms != null)
                                summary.MenuGroupTakeAways = dbms.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT
                                                                        && x.FlagTakeAway == true);

                            if (dbses != null)
                                summary.SalesGroup1 = dbses.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                           && x.FrontEndID == summary.FrontEndID
                                                                           && x.BranchID == summary.BranchID
                                                                           && x.SHIFT == summary.SHIFT
                                                                           && x.ItemGroup == 1);

                            if (dbses != null)
                                summary.SalesGroup2 = dbses.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                           && x.FrontEndID == summary.FrontEndID
                                                                           && x.BranchID == summary.BranchID
                                                                           && x.SHIFT == summary.SHIFT
                                                                           && x.ItemGroup == 2);

                            if (dbts != null)
                                summary.Taxes = dbts.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT);

                            if (dbps != null)
                                summary.Payments = dbps.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT);

                            if (dbdrs != null)
                                summary.DiscountReasons = dbdrs.FindAll(x => x.TransactionRefID == summary.TransactionRefID
                                                                        && x.FrontEndID == summary.FrontEndID
                                                                        && x.BranchID == summary.BranchID
                                                                        && x.SHIFT == summary.SHIFT);
                        }

                        result = dbss;
                    }
                }
            }));

            return result;
        }

        #endregion
        #region Sales

        public Models.SalesOrderResultDo CreateSalesOrder(Models.NewSalesOrderDo entity)
        {
            Models.SalesOrderResultDo result = new Models.SalesOrderResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_FE_Create_SalesOrder]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "TransactionRefID", entity.TransactionRefID);

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);

                command.AddParameter(typeof(int), "LoopNo", entity.LoopNo);
                command.AddParameter(typeof(int), "LoopTotal", entity.LoopTotal);

                string transactionXML = Utils.ConvertUtil.ConvertToXml_Store<Models.SalesOrderTransactionDo>(entity.Transactions);
                command.AddParameter(typeof(string), "TransactionXML", transactionXML);

                string paymentXML = Utils.ConvertUtil.ConvertToXml_Store<Models.SalesOrderPaymentDo>(entity.Payments);
                command.AddParameter(typeof(string), "PaymentXML", paymentXML);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                
                result.Data = command.ToObject<Models.SalesOrderDo>();
                result.ErrorParameter(error);
                
            }));

            return result;
        }

        public List<Models.SalesOrderForBillDo> GetSalesOrderForBill(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.SalesOrderForBillDo> result = new List<Models.SalesOrderForBillDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_SalesOrderItemForBill]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                System.Collections.IList[] dbls = command.ToList(
                       typeof(Models.SalesOrderItemForBillDo));
                if (dbls != null)
                {
                    List<Models.SalesOrderItemForBillDo> dbois = dbls[0] as List<Models.SalesOrderItemForBillDo>;
                    if (dbois != null)
                    {
                        foreach (Models.SalesOrderItemForBillDo item in dbois)
                        {
                            Models.SalesOrderForBillDo order = result.Find(x => x.TransactionRefID == item.TransactionRefID
                                                                            && x.FrontEndID == item.FrontEndID
                                                                            && x.BranchID == item.BranchID
                                                                            && x.SHIFT == item.SHIFT
                                                                            && x.OrderID == item.OrderID);
                            if (order == null)
                            {
                                order = new Models.SalesOrderForBillDo()
                                {
                                    TransactionRefID = item.TransactionRefID,
                                    FrontEndID = item.FrontEndID,
                                    BranchID = item.BranchID,
                                    SHIFT = item.SHIFT,
                                    OrderID = item.OrderID,
                                    BranchName = item.BranchName,
                                    CreateUser = item.CreateUser,
                                    MKTSeq = item.MKTSeq,
                                    PrintDate = item.PrintDate,
                                    PrintNo = item.PrintNo,
                                    ApproveUser = item.ApproveUser,
                                    TotalAmt = item.TotalAmt,
                                    DiscountAmt = item.DiscountAmt,
                                    GrandTotalAmt = item.GrandTotalAmt,
                                    PrinterName = item.PrinterName
                                };
                                order.Items = new List<Models.SalesOrderItemForBillDo>();
                                
                                result.Add(order);
                            }

                            order.Items.Add(item);
                        }
                    }
                }
            }));

            return result;
        }
        public List<Models.SalesOrderForSlipDo> GetSalesOrderForSlip(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.SalesOrderForSlipDo> result = new List<Models.SalesOrderForSlipDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_SalesOrderItemForSlip]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);

                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                System.Collections.IList[] dbls = command.ToList(
                       typeof(Models.SalesOrderItemForSlipDo));
                if (dbls != null)
                {
                    List<Models.SalesOrderItemForSlipDo> dbois = dbls[0] as List<Models.SalesOrderItemForSlipDo>;
                    if (dbois != null)
                    {
                        foreach (Models.SalesOrderItemForSlipDo item in dbois)
                        {
                            Models.SalesOrderForSlipDo order = result.Find(x => x.TransactionRefID == item.TransactionRefID
                                                                            && x.FrontEndID == item.FrontEndID
                                                                            && x.BranchID == item.BranchID
                                                                            && x.SHIFT == item.SHIFT
                                                                            && x.OrderID == item.OrderID);
                            if (order == null)
                            {
                                order = new Models.SalesOrderForSlipDo()
                                {
                                    TransactionRefID = item.TransactionRefID,
                                    FrontEndID = item.FrontEndID,
                                    BranchID = item.BranchID,
                                    SHIFT = item.SHIFT,
                                    OrderID = item.OrderID,
                                    BrandName = item.BrandName,
                                    BranchName = item.BranchName,
                                    MKTSeq = item.MKTSeq,
                                    PrintDate = item.PrintDate,

                                    TotalAmt = item.TotalAmt,
                                    DiscountAmt = item.DiscountAmt,
                                    GrandTotalAmt = item.GrandTotalAmt,

                                    Qty = item.Qty,
                                    PrinterName = item.PrinterName

                                };
                                order.Methods = new List<Models.SalesOrderItemForSlipDo>();

                                result.Add(order);
                            }

                            order.Methods.Add(item);
                        }
                    }
                }
            }));

            return result;
        }

        #endregion
        #region Cashdrawer

        public Models.CashdrawerInfoDo GetCashdrawerInfo()
        {
            Models.CashdrawerInfoDo result = null;
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_CashdrawerInfo]";

                result = command.ToObject<Models.CashdrawerInfoDo>();
            }));

            return result;
        }
        public Models.CashdrawerInfoDo UpdateCashdrawerInfo(Models.CashdrawerInfoDo entity)
        {
            Models.CashdrawerInfoDo result = null;
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Update_CashdrawerInfo]";

                command.AddParameter(typeof(string), "IPAddress", entity.IPAddress);
                command.AddParameter(typeof(string), "Username", entity.Username);
                command.AddParameter(typeof(string), "Password", entity.Password);
                command.AddParameter(typeof(string), "Path", entity.Path);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                result = command.ToObject<Models.CashdrawerInfoDo>();
            }));

            return result;
        }

        #endregion
    }
}
