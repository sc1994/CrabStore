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
    public class CsPackageController : BaseController
    {
        // GET: CsPackage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsPackageList(CsPackageView.CsPackageWhere para)
        {
            var sh = new SqlHelper<CsPackage>();
            if (!para.PackageName.IsNullOrEmpty())
                sh.AddWhere(CsPackageEnum.PackageName, para.PackageName, RelationEnum.Like);
            if (!para.PackageNumber.IsNullOrEmpty())
                sh.AddWhere(CsPackageEnum.PackageNumber, para.PackageNumber, RelationEnum.Like);
            if (!para.PackageState.IsNullOrEmpty())
                sh.AddWhere(CsPackageEnum.PackageState, para.PackageState.ToInt());

            return Json(new
            {
                data = sh.Select().Select(x => new
                {
                    x.PackageId,
                    PackageType = ((ProductType)x.PackageType).ToString(),
                    x.PackageName,
                    x.PackageNumber,
                    x.PackageImage,
                    x.PackageWeight,
                    PackagePrice = x.PackagePrice.ToString("N2"),
                    PackageState = ((ProductState)x.PackageState).ToString(),
                    OperationDate = x.OperationDate.ToString("yyyy-M-d HH:mm:ss"),
                    x.PackageStock
                }),
                sql = sh.SqlString.ToString(),
            });
        }

        public ActionResult GetCsPackageInfo(int id)
        {
            if (id <= 0)
            {
                return Json(new ResModel
                {
                    Data = "错误的参数,请刷新页面重试",
                    ResStatus = ResStatue.No
                });
            }
            var model = new CsPackageBll().GetModel(id);
            if (model == null)
            {
                return Json(new ResModel
                {
                    Data = "错误的参数,请刷新页面重试",
                    ResStatus = ResStatue.No
                });
            }
            return Json(new ResModel
            {
                Data = model,
                ResStatus = ResStatue.Yes
            });
        }

        public ActionResult SubmitCsPackageInfo(CsPackage model)
        {
            if (model.PackageId <= 0)
            {
                return Json(new ResModel
                {
                    Data = "错误的参数,请刷新页面重试",
                    ResStatus = ResStatue.No
                });
            }
            var sh = new SqlHelper<CsPackage>();
            sh.AddWhere(CsPackageEnum.PackageId, model.PackageId);
            sh.AddUpdate("PackageName", model.PackageName);
            sh.AddUpdate("PackagePrice", model.PackagePrice);
            sh.AddUpdate("PackageNumber", model.PackageNumber);
            sh.AddUpdate("PackageState", model.PackageState);
            sh.AddUpdate("OperationDate", DateTime.Now);
            var line = sh.Update();
            if (line > 0)
            {
                return Json(new ResModel
                {
                    Data = "更新成功",
                    ResStatus = ResStatue.Yes
                });
            }
            return Json(new ResModel
            {
                Data = "数据执行成功但是没有受影响行数",
                ResStatus = ResStatue.Warn
            });
        }
    }
}