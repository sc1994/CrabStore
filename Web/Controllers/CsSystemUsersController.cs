using System;
using System.Linq;
using System.Web.Mvc;
using BLL;
using Common;
using Model.DBModel;
using Model.ViewModel;
using SqlHelper;

namespace Web.Controllers
{
    public class CsSystemUsersController : BaseController
    {
        private readonly CsSystemUsersBll _csSystemUsersBll = new CsSystemUsersBll();
        // GET: CsSystemUsers
        public ActionResult Index()
        {
            if (CurrentUser.SysUserType == SysUserType.普通用户.GetHashCode())
            {
                return Redirect("~/");
            }
            else
            {
                return View();
            }
        }

        public ActionResult GetCsSystemUsersPage(CsSystemUsersView.CsSystemUsersWhere para)
        {
            var sh = new SqlHelper<CsSystemUsers>
            {
                PageConfig = new PageConfig
                {
                    PageSortField = CsSystemUsersEnum.SysUserId.ToString(),
                    SortEnum = SortEnum.Desc,
                    PageSize = PageSize,
                    PageIndex = para.CurrentPage
                }
            };
            if (!para.SysUserName.IsNullOrEmpty())
                sh.AddWhere(CsSystemUsersEnum.SysUserName, para.SysUserName, RelationEnum.Like);
            if (para.SysUserState > -1)
                sh.AddWhere(CsSystemUsersEnum.SysUserState, para.SysUserState);
            if (para.SysUserType > 0)
                sh.AddWhere(CsSystemUsersEnum.SysUserType, para.SysUserType);
            var list = sh.Select();

            return Json(new
            {
                data = list.Select(x => new CsSystemUsersView.CsSystemUsersPage
                {
                    SysUserName = x.SysUserName,
                    SysUserState = ((RowStatus)x.SysUserState).ToString(),
                    SysUserType = ((SysUserType)x.SysUserType).ToString(),
                    SysUserDate = x.SysUserDate.ToString("yyyy-M-d"),
                    SysUserId = x.SysUserId
                }),
                total = sh.Total,
                sql = sh.SqlString.ToString()
            });
        }

        public ActionResult SubmitCsSystemUsers(CsSystemUsers model)
        {
            if (model.SysUserType == SysUserType.管理员.GetHashCode())
            {
                if (_csSystemUsersBll.GetModelList(" AND SysUserType = 1 AND SysUserState = 1").Any())
                {
                    return Json(new ResModel
                    {
                        ResStatus = ResStatue.Warn,
                        Data = "最多只能存在一个管理员, 请勿重复设置管理员"
                    });
                }
            }
            ResStatue code;
            var msg = string.Empty;
            if (model.SysUserId > 0)
            {
                code = _csSystemUsersBll.Update(model) ? ResStatue.Yes : ResStatue.No;
            }
            else
            {
                model.SysUserDate = DateTime.Now;
                code = _csSystemUsersBll.Add(model) > 0 ? ResStatue.Yes : ResStatue.No;
            }
            return Json(new ResModel
            {
                ResStatus = code,
                Data = msg
            });
        }

        public ActionResult GetCsSystemUsersModel(int id)
        {
            if (id < 1)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "参数错误,请刷新页面重试"
                });
            }
            var model = _csSystemUsersBll.GetModel(id);
            if (model == null)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "参数错误,请刷新页面重试"
                });
            }
            return Json(new ResModel
            {
                ResStatus = ResStatue.Yes,
                Data = new CsSystemUsersView.CsSystemUsersPage
                {
                    SysUserId = model.SysUserId,
                    SysUserName = model.SysUserName,
                    SysUserDate = model.SysUserDate.ToString("yyyy-M-d"),
                    SysUserState = model.SysUserState.ToString(),
                    SysUserType = model.SysUserType.ToString(),
                    SysUserPassword = model.SysUserPassword,
                    DeleteDate = model.DeleteDate.ToString("yyyy-M-d hh:mm:ss"),
                    DeleteDescribe = model.DeleteDescribe
                }
            });
        }
    }
}