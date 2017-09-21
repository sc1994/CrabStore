function ajax(url, data, backCall) {
    axios.post(url, data)
        .then(function (d) {
            if (d.data === "<script>window.location.href = '/Login'</script>") {
                alert('个人信息异常,或登陆失效, 请重新登陆');
                window.location.href = '/Login'
                return;
            }
            var obj
            try {
                obj = JSON.parse(d.data)
            } catch (e) {
                obj = d.data
            }
            backCall(obj)
        })
        .catch(function (err) {
            console.log(err)
        })
}


// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// ReSharper disable once NativeTypePrototypeExtending
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
        if (o.hasOwnProperty(k))
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1,
                    (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function clone(obj) {
    var o, i, j;
    if (typeof (obj) != "object" || obj === null) return obj;
    if (obj instanceof(Array)) {
        o = [];
        i = 0;
        j = obj.length;
        for (; i < j; i++) {
            if (typeof (obj[i]) == "object" && obj[i] != null) {
                // ReSharper disable once CallerCalleeUsing
                o[i] = arguments.callee(obj[i]);
            } else {
                o[i] = obj[i];
            }
        }
    } else {
        o = {};
        for (i in obj) {
            if (obj.hasOwnProperty(i)) {
                if (typeof (obj[i]) == "object" && obj[i] != null) {
                    // ReSharper disable once CallerCalleeUsing
                    o[i] = arguments.callee(obj[i]);
                } else {
                    o[i] = obj[i];
                }
            }
        }
    }

    return o;
}

function getRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") !== -1) {
        var str = url.substr(1);
        var strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
req = getRequest();