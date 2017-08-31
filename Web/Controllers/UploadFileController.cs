using System;
using Common;
using System.IO;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UploadFileController : Controller
    {
        public ActionResult UploadFile()
        {
            var files = Request.Files;
            if (files.Count < 1)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "未收到任何的文件请求"
                });
            }
            var file = files["file"];
            if (file == null)
            {
                return Json(new ResModel
                {
                    ResStatus = ResStatue.No,
                    Data = "文件为空"
                });
            }
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "upload/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            // ReSharper disable once PossibleNullReferenceException
            path = path + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "." + file.FileName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries)[1];
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                file.SaveAs(path);
                return Json(new ResModel
                {
                    ResStatus = ResStatue.Yes,
                    Data = path.Replace(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../")
                });
            }
            catch (Exception e)
            {
                LogHelper.Log(e.Message + "--------path:" + path, "文件上传异常");
            }
            return Json(new ResModel
            {
                ResStatus = ResStatue.No,
                Data = "文件保存异常,请查看日志"
            });
        }
    }
}