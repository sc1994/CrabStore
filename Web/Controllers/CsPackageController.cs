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
        private readonly CsPackageBll _csPackageBll = new CsPackageBll();

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
            if (_csPackageBll.GetModelList($" and PackageName ='{model.PackageName}'").Any() &&
                model.PackageId == 0)
            {
                return Json(new ResModel
                {
                    Data = "已存在该套餐，请重新设置",
                    ResStatus = ResStatue.Warn
                });
            }
            ResStatue code;
            if (model.PackageId > 0)
            {
                model.OperationDate = DateTime.Now;
                code = _csPackageBll.Update(model) ? ResStatue.Yes : ResStatue.Warn;
            }
            else
            {
                model.PackageWeight = 0;
                model.PackageStock = 1000;
                model.PackageImage = "Images/10007.jpg";
                model.OperationDate = DateTime.Now;
                model.PackageType = 1;
                code = _csPackageBll.Add(model) > 0 ? ResStatue.Yes : ResStatue.Warn;
            }

            return Json(new ResModel
            {
                ResStatus = code,
                Data = code == ResStatue.Yes ? "更新成功" : "执行完成,但是没有数据受影响"
            });
        }
    }
}