function ajax(url, data, backCall) {
    ajaxsound({
        type: "post",
        url: url,
        data: data,
        success: function (d) {
            if (d.indexOf('script') > -1) {
                alert('个人信息异常,或登陆失效, 请重新登陆');
                window.location.href = '/Login'
                return;
            }
            var obj
            try {
                obj = JSON.parse(d)
            } catch (e) {
                obj = d
            }
            backCall(obj)
        },
        error: function () {
            console.log("error")
        }
    });
}

function ajaxsound() {
    var ajaxData = {
        type: arguments[0].type || "GET",
        url: arguments[0].url || "",
        async: arguments[0].async || "true",
        data: arguments[0].data || null,
        dataType: arguments[0].dataType || "text",
        contentType: arguments[0].contentType || "application/x-www-form-urlencoded",
        beforeSend: arguments[0].beforeSend || function () {},
        success: arguments[0].success || function () {},
        error: arguments[0].error || function () {}
    }
    ajaxData.beforeSend()
    var xhr = createxmlHttpRequest();
    xhr.responseType = ajaxData.dataType;
    xhr.open(ajaxData.type, ajaxData.url, ajaxData.async);
    xhr.setRequestHeader("Content-Type", ajaxData.contentType);
    xhr.send(convertData(ajaxData.data));
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                ajaxData.success(xhr.response)
            } else {
                ajaxData.error()
            }
        }
    }

    function createxmlHttpRequest() {
        if (window.ActiveXObject) {
            return new ActiveXObject("Microsoft.XMLHTTP");
        } else if (window.XMLHttpRequest) {
            return new XMLHttpRequest();
        }
    }

    function convertData(data) {
        if (typeof data === 'object') {
            var convertResult = "";
            for (var c in data) {
                if (data.hasOwnProperty(c)) {
                    convertResult += c + "=" + data[c] + "&";
                }
            }
            convertResult = convertResult.substring(0, convertResult.length - 1)
            return convertResult;
        } else {
            return data;
        }
    }
}


// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function clone(obj) {
    var o, i, j, k;
    if (typeof (obj) != "object" || obj === null) return obj;
    if (obj instanceof(Array)) {
        o = [];
        i = 0;
        j = obj.length;
        for (; i < j; i++) {
            if (typeof (obj[i]) == "object" && obj[i] != null) {
                o[i] = arguments.callee(obj[i]);
            } else {
                o[i] = obj[i];
            }
        }
    } else {
        o = {};
        for (i in obj) {
            if (typeof (obj[i]) == "object" && obj[i] != null) {
                o[i] = arguments.callee(obj[i]);
            } else {
                o[i] = obj[i];
            }
        }
    }

    return o;
}