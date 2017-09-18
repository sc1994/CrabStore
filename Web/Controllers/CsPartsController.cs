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
    public class CsPartsController : BaseController
    {
        private readonly CsPartsBll _csPartsBll = new CsPartsBll();
        // GET: CsParts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCsPartsList(CsPartsView.CsPartsWhere para)
        {
            var sh = new SqlHelper<CsParts>();
            if (!para.PartName.IsNullOrEmpty())
                sh.AddWhere(CsPartsEnum.PartName, para.PartName, RelationEnum.Like);
            if (para.PartType != 0)
                sh.AddWhere(CsPartsEnum.PartType, para.PartType);
            if (para.PartState != -1)
                sh.AddWhere(CsPartsEnum.PartState, para.PartState);
            var list = sh.Select();
            return Json(new
            {
                data = list.Select(x => new CsPartsView.CsPartList
                {
                    PartName = $"{x.PartName}({x.PartNumber})",
                    PartState = ((RowStatus)x.PartState).ToString(),
                    PartType = ((PartType)x.PartType).ToString(),
                    PartId = x.PartId,
                    OperationDate = x.OperationDate.ToString("yyyy-M-d"),
                    PartPrice = x.PartPrice.ToString("0.00"),
                    PartWeight = x.PartWeight.ToString("0.000")
                }),
                sql = sh.SqlString.ToString()
            });
        }

        public ActionResult GetCsPartsModel(int id)
        {
            if (id == 0)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "Id无效"
                });
            }
            var model = _csPartsBll.GetModel(id);
            if (model == null)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "Id未能查询到有效数据"
                });
            }
            return Json(new ResModel
            {
                ResStatus = ResStatue.Yes,
                Data = new CsPartsView.CsPartList
                {
                    PartId = model.PartId,
                    PartName = model.PartName,
                    OperationDate = model.OperationDate.ToString("yyyy-M-d"),
                    PartState = model.PartState.ToString(),
                    PartType = model.PartType.ToString(),
                    PartWeight = model.PartWeight.ToString("0.000"),
                    PartPrice = model.PartPrice.ToString("0.00")
                }
            });
        }

        public ActionResult SubmitCsParts(CsParts model)
        {
            var sh = new SqlHelper<CsParts>();
            sh.AddUpdate(CsPartsEnum.OperationDate.ToString(), DateTime.Now);
            sh.AddUpdate(CsPartsEnum.PartWeight.ToString(), model.PartWeight);
            sh.AddUpdate(CsPartsEnum.PartPrice.ToString(), model.PartPrice);
            sh.AddUpdate(CsPartsEnum.PartState.ToString(), model.PartState);
            sh.AddWhere(CsPartsEnum.PartId, model.PartId);
            if (sh.Update() > 0)
            {
                return Json(new ResModel
                {
                    Data = "更新成功",
                    ResStatus = ResStatue.Yes
                });
            }
            return Json(new ResModel
            {
                Data = "出现异常, 请稍后再试",
                ResStatus = ResStatue.No
            });
        }
    }
}